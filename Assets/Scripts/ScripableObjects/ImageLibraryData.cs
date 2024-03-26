using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[CreateAssetMenu(menuName = "ARImageLibraryData", fileName = "ARImageLibraryData")]
public class ImageLibraryData : ScriptableObject, ISerializationCallbackReceiver
{
    [Serializable]
    struct NamedPrefab
    {
        public string imageGuid;
        public GameObject imagePrefab;

        public NamedPrefab(Guid guid, GameObject prefab)
        {
            imageGuid = guid.ToString();
            imagePrefab = prefab;
        }
    }

    [SerializeField]
    // [HideInInspector]
    List<NamedPrefab> m_PrefabsList = new List<NamedPrefab>();

    public Dictionary<Guid, GameObject> prefabsDictionary = new Dictionary<Guid, GameObject>();

    [FormerlySerializedAs("m_ImageLibrary")]
    [SerializeField]
    [Tooltip("Reference Image Library")]
    XRReferenceImageLibrary _imageLibrary;

    /// <summary>
    /// Get the <c>XRReferenceImageLibrary</c>
    /// </summary>
    public XRReferenceImageLibrary imageLibrary
    {
        get => _imageLibrary;
        set => _imageLibrary = value;
    }

    public void OnBeforeSerialize()
    {
        m_PrefabsList.Clear();
        foreach (var kvp in prefabsDictionary)
        {
            m_PrefabsList.Add(new NamedPrefab(kvp.Key, kvp.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        prefabsDictionary = new Dictionary<Guid, GameObject>();
        foreach (var entry in m_PrefabsList)
        {
            prefabsDictionary.Add(Guid.Parse(entry.imageGuid), entry.imagePrefab);
        }
    }

    public GameObject GetPrefabForReferenceImage(XRReferenceImage referenceImage)
        => prefabsDictionary.TryGetValue(referenceImage.guid, out var prefab) ? prefab : null;


    
#if UNITY_EDITOR
    [CustomEditor(typeof(ImageLibraryData))]
    class PrefabImagePairManagerInspector : Editor
    {
        List<XRReferenceImage> _referenceImages = new List<XRReferenceImage>();
        bool isExpanded = true;

        bool HasLibraryChanged(XRReferenceImageLibrary library)
        {
            if (library == null)
                return _referenceImages.Count == 0;

            if (_referenceImages.Count != library.count)
                return true;

            for (int i = 0; i < library.count; i++)
            {
                if (_referenceImages[i] != library[i])
                    return true;
            }

            return false;
        }

        public override void OnInspectorGUI()
        {
            //customized inspector
            var behaviour = serializedObject.targetObject as ImageLibraryData;

            serializedObject.Update();
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            }

            var libraryProperty = serializedObject.FindProperty(nameof(_imageLibrary));
            EditorGUILayout.PropertyField(libraryProperty);
            var library = libraryProperty.objectReferenceValue as XRReferenceImageLibrary;

            //check library changes
            if (HasLibraryChanged(library))
            {
                if (library)
                {
                    var tempDictionary = new Dictionary<Guid, GameObject>();
                    foreach (var referenceImage in library)
                    {
                        tempDictionary.Add(referenceImage.guid, behaviour.GetPrefabForReferenceImage(referenceImage));
                    }
                    behaviour.prefabsDictionary = tempDictionary;
                }
            }

            // update current
            _referenceImages.Clear();
            if (library)
            {
                foreach (var referenceImage in library)
                {
                    _referenceImages.Add(referenceImage);
                }
            }

            //show prefab list
            isExpanded = EditorGUILayout.Foldout(isExpanded, "Prefab List");
            if (isExpanded)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUI.BeginChangeCheck();

                    var tempDictionary = new Dictionary<Guid, GameObject>();
                    foreach (var image in library)
                    {
                        var prefab = (GameObject) EditorGUILayout.ObjectField(image.name, behaviour.prefabsDictionary[image.guid], typeof(GameObject), false);
                        tempDictionary.Add(image.guid, prefab);
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(target, "Update Prefab");
                        behaviour.prefabsDictionary = tempDictionary;
                        EditorUtility.SetDirty(target);
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    
}
