using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FeatheringEffect : MonoBehaviour
{
    [SerializeField] private float _featheringWidth = .1f;

    private ARPlaneMeshVisualizer _planeMeshVisualizer;
    private Material _material;
    private ARPlane _arPlane;
    static List<Vector3> _featheringUVs = new List<Vector3>();
    static List<Vector3> _vertices = new List<Vector3>();
    private static readonly int ShortestUVMapping = Shader.PropertyToID("_ShortestUVMapping");

    private void Awake()
    {
        _planeMeshVisualizer = GetComponent<ARPlaneMeshVisualizer>();
        _material = GetComponent<MeshRenderer>().material;
        _arPlane = GetComponent<ARPlane>();
    }

    private void OnEnable()
    {
        _arPlane.boundaryChanged += ArPlaneOnboundaryChanged;
    }

    private void ArPlaneOnboundaryChanged(ARPlaneBoundaryChangedEventArgs obj)
    {
        var numVertex = _planeMeshVisualizer.mesh.vertexCount;
        _featheringUVs.Clear();
        if (_featheringUVs.Capacity < numVertex )
        {
            _featheringUVs.Capacity = numVertex;
        }
        _planeMeshVisualizer.mesh.GetVertices(_vertices);
        var centerInPlaneSpace = _vertices[_vertices.Count - 1];
        var uv = Vector3.zero;
        var shortUVMapping = float.MaxValue;
        for (int i = 0; i < numVertex -1; i++)
        {
            var vertexDist = Vector3.Distance(_vertices[i], centerInPlaneSpace);
            var uvMap = vertexDist / Mathf.Max(vertexDist - _featheringWidth, .001f);
            uv.x = uvMap;
            if (shortUVMapping > uvMap)
            {
                shortUVMapping = uvMap;
            }
            _featheringUVs.Add(uv);
        }
        _material.SetFloat(ShortestUVMapping, shortUVMapping);
        uv.Set(0,0,0);
        _featheringUVs.Add(uv);
        
        _planeMeshVisualizer.mesh.SetUVs(1, _featheringUVs);
        _planeMeshVisualizer.mesh.UploadMeshData(false);
        
    }
}
