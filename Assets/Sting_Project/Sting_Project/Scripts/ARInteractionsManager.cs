using System;
using System.Collections;
using System.Collections.Generic;
using Sting_Project.Sting_Project.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARInteractionsManager : MonoBehaviour
{
    [SerializeField] private Camera aRCamera;//creando campos comenzando por camara
    private ARRaycastManager aRRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();//creando una lista de arraycast hit que voy a llamar hits
    
    //definiendo dos campos
    private GameObject aRPointer;//campo para el gameobject del pointer
    private GameObject item3DModel;//para el modelo que ha sido creado
    private GameObject itemSelected;
         
    private bool isInitialPosition;
    private bool isOverUI;
    private bool isOver3DModel;//para saber si estoy o no sobre un modelo 3D, si el touch ha sido sobre un modeo 3d

    private Vector2 initialTouchPos;

    //construyeno propiedad para item3dmodel
    public GameObject Item3DModel
    {
        set
        {
            item3DModel = value;
            item3DModel.transform.position = aRPointer.transform.position;//una vez sea asignado el modelo 3D este tome la posicion del pointer
            item3DModel.transform.parent = aRPointer.transform;//que el modelo 3d sea un hijo del pointer para asi poderlo desplazar
            isInitialPosition = true;//necesito dar una posicion inicial

        }
    }
    

    void Start()
    {
        aRPointer = transform.GetChild(0).gameObject;
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        Sting_Project.Sting_Project.Scripts.GameManager.instance.OnMainMenu += SetItemPosition;//creando funcion que permita fijar el modelo 3d, con+=suscribo la funcion
    }

    void Update()
    {
        //lo que haremoe en update es darle la posicion inicial al modelo 3d que ha sido creado, usaremos el raycast para encontrar un plano de los que han sido detectados y cuando haya sido encontrado vamos a posicionar el modelo3d sobre ese plano y en ese punto
        if (isInitialPosition)
        {
            Vector2 middlePointScreen = new Vector2(Screen.width / 2, Screen.height / 2);//defino la mitad de la pantalla
            aRRaycastManager.Raycast(middlePointScreen, hits, TrackableType.Planes);//ahora le pasare esto a raycast manager
            if (hits.Count>0)//es decir que encontro un plano que se encuentra dentro de la mitad de la pantalla
            {
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;//lo mismo para la rotacion
                aRPointer.SetActive(true);//ahora que ha encontrado el punto voy a mostrar el pointer
                isInitialPosition = false;//como ya dio la posicion inicial lo vuelvo false

            }
        }

        //verificamos que haya un input, osea que se toco l apantalla
        if (Input.touchCount > 0)
        {
            Touch touchOne = Input.GetTouch(0);//creando variable Touch que se llama touchOne y le asignare el touch que ha ocurrido

            if (touchOne.phase==TouchPhase.Began)//verificar que ese touch no ha sido en un boton de la interfaz/Began es decir cuando ha comenzado el touch
            {
                var touchPosition = touchOne.position;
                isOverUI = isTapOverUI(touchPosition);
                isOver3DModel = isTapOver3DModel(touchPosition);//compruebo si el touch ha sido o no sobre la interfaz
            }
            if (touchOne.phase==TouchPhase.Moved)//quiere decir que estoy moviendo el dedo en la pantalla
            {
                if (aRRaycastManager.Raycast(touchOne.position,hits,TrackableType.Planes))//verificar que ese movimiento ha sido dentro de los planos que se han detectado en realidad aumentada
                {
                    Pose hitPose = hits[0].pose;
                    if(!isOverUI&&isOver3DModel)//si el touch no ha sido sobre un boton y ha sido sobre un modelo 3d
                    {
                        transform.position = hitPose.position;//entonces voy a cambiar la posicion del modelo 3d
                        transform.rotation = hitPose.rotation;
                    }
                }

            }

            if (Input.touchCount==2)//si han existido los 2 inputs para hacer la rotacion con 2 dedos
            {
                Touch touchTwo = Input.GetTouch(1);//creare un nuevo touch
                if (touchOne.phase==TouchPhase.Began||touchTwo.phase==TouchPhase.Began)//verificar que alguno de los touch ha iniciado
                {
                    initialTouchPos = touchTwo.position - touchOne.position;

                }

               if (touchOne.phase==TouchPhase.Moved||touchTwo.phase==TouchPhase.Moved)//ahora debo saber si el usuario esta moviendo los dedos, osea si alguno de los touch se ha movido
                {
                    Vector2 currentTouchPos = touchTwo.position - touchOne.position;//si ha sido asi entonces existe una nueva posicion para los touch qe debo guardar
                    float angle = Vector2.SignedAngle(initialTouchPos, currentTouchPos);//calculando el angulo entre la posicion inicial y la actual
                    item3DModel.transform.rotation = Quaternion.Euler(0, item3DModel.transform.eulerAngles.y - angle, 0);//aplicando la rotacion al modelo 3d, solo rotara en el eje y no en x ni en z
                    initialTouchPos = currentTouchPos;

                }
            }
            if (isOver3DModel && item3DModel ==null&& !isOverUI)//verificamos que le touch haya sido sobre el modelo 3d, que item 3d model no tenga ningun modelo 3d asignado y que el tap no haya sido en la interfaz
            {
                Sting_Project.Sting_Project.Scripts.GameManager.instance.ARPosition();//llamare al evento ar position
                item3DModel = itemSelected;
                itemSelected = null;//para que no tenga ningun modelo 3d
                aRPointer.SetActive(true);//activo el pointer
                transform.position = item3DModel.transform.position;
                item3DModel.transform.parent = aRPointer.transform;//ahorahago que el modelo 3d seleccionado sea hijo del pointer para poderlo desplazar

            }


        }        
    }

    private bool isTapOver3DModel(Vector2 touchPosition)//funcion que me permite saber si el touch ha sido sobre un modelo 3d
    {
        Ray ray = aRCamera.ScreenPointToRay(touchPosition);//le paso la posicion del touch o sea touchposition
        if (Physics.Raycast(ray,out RaycastHit hit3DModel))//utilizare physissc para saber si el ray ha tocado o no un collider
        {
            if (hit3DModel.collider.CompareTag("Item"))
            {
                itemSelected = hit3DModel.transform.gameObject;
                return true;
            }

        }
        return false;

    }

    public static bool isTapOverUI(Vector2 touchPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);//estoy asignando el eventdata de la interfaz que hicimos
        eventData.position = new Vector2(touchPosition.x, touchPosition.y);//pasar la posicion del touch
        List<RaycastResult> result = new List<RaycastResult>();//creando una nueva lista
        EventSystem.current.RaycastAll(eventData, result);//verifico si existe algun evento donde ha sido el touch
        for(int i = 0; i < result.Count; i++)
            if (result[i].gameObject.layer != LayerMask.NameToLayer("UI"))
            {
                result.RemoveAt(i);
                i--;
            }
        return result.Count > 0;//y regreso


    }

    private void SetItemPosition()
    {
        if (item3DModel !=null)//primero verificamos que haya sido asignado un modelo 3d con el fin de evitar errores
        {
            item3DModel.transform.parent = null;//como ha sido fijado entonces ya no tiene porque ser hijo del pointer
            aRPointer.SetActive(false);//desactivando porque ya no lo necesito pues el modelo ha sido fijado
            item3DModel = null;

        }
    }
    public void DeleteItem()//funcion para eliminar el modelo 3d que esta en AR

    {
        Destroy(item3DModel);
        aRPointer.SetActive(false);//desactivo el pointer pues ya nohabra modelo 3d
        Sting_Project.Sting_Project.Scripts.GameManager.instance.MainMenu();//llamare automaticamente al estado OnMainmenu

    }
}
