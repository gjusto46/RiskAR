using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using ScripableObjects;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ServerManager : MonoBehaviour
{
    [SerializeField] private ObjectsPositionInfo _objectsPositionInfo;
    [SerializeField] private ConnectionData _connectionData;
    [SerializeField] private bool isLocal;
    [SerializeField] private UnityEvent _onInitLoadData;
    [SerializeField] private UnityEvent _onSuccessLoadWithData;
    [SerializeField] private UnityEvent _onSuccessLoadWithoutData;
    [SerializeField] private UnityEvent _onFailedLoad;
    

    private void Start()
    {
        GetInfo();
    }

    public void SaveInfo()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        var stringJson = JsonUtility.ToJson(_objectsPositionInfo._listPositions);
#if UNITY_EDITOR
        var data = reference.Child(_connectionData.referenceLocal);
#else
        var data = reference.Child(_connectionData.referenceProduction);
#endif
        
        // Debug.Log(data.GetValueAsync());
        data.SetValueAsync(stringJson);
    }
    public void GetInfo()
        
    {
        _onInitLoadData?.Invoke();
#if !UNITY_EDITOR
        var data = FirebaseDatabase.DefaultInstance.GetReference(_connectionData.referenceProduction);
#else
        var data = FirebaseDatabase.DefaultInstance.GetReference(_connectionData.referenceLocal);
#endif
        
        if (data != null)
        { 
            data.GetValueAsync().ContinueWithOnMainThread(
             task =>
             {
                 switch (task.Status)
                 {
                     case TaskStatus.Faulted:
                         Debug.Log("No existe valores");
                         break;
                     case TaskStatus.Canceled:
                         Debug.Log("Cancelado");
                         break;
                     case TaskStatus.Running:
                         Debug.Log("consultando");
                         break;
                     case TaskStatus.RanToCompletion:
                         try
                         {
                             DataSnapshot dataSnapshot = task.Result;
                             if (dataSnapshot.Exists)
                             {
                                 var jsonObject = JsonUtility.FromJson<ListPositions>(dataSnapshot.Value.ToString());
                                 _objectsPositionInfo._listPositions._PositionInfos = jsonObject._PositionInfos;
                                 for (int i = 0; i < _objectsPositionInfo._ModelsToInstanciates.Count; i++)
                                 {
                                     var idObject = jsonObject._PositionInfos.Exists(x =>
                                         x.idObject == _objectsPositionInfo._ModelsToInstanciates[i].idObject);
                                     _objectsPositionInfo._ModelsToInstanciates[i].instantiated = idObject;
                                 }
                                 _objectsPositionInfo.isSavedInfo = true;
                                 _onSuccessLoadWithData?.Invoke();
                             }
                             else
                             {
                                 _objectsPositionInfo._listPositions._PositionInfos.Clear();
                                 for (int i = 0; i < _objectsPositionInfo._ModelsToInstanciates.Count; i++)
                                 {
                                     _objectsPositionInfo._ModelsToInstanciates[i].instantiated = false;
                                 }
                                 _objectsPositionInfo.isSavedInfo = false;
                                 _onSuccessLoadWithoutData?.Invoke();
                             }
                             
                         }
                         catch (Exception e)
                         {
                             _onFailedLoad?.Invoke();
                             throw;
                         }
                         
                         break;
                     case TaskStatus.Created:
                         break;
                     case TaskStatus.WaitingForActivation:
                         break;
                     case TaskStatus.WaitingForChildrenToComplete:
                         break;
                     case TaskStatus.WaitingToRun:
                         break;
                     default:
                         throw new ArgumentOutOfRangeException();
                 }
             }
             );   
        }
        else
        {
            _onFailedLoad?.Invoke();
        }
        
    }
    
}
