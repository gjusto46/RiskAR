using System;
using System.Collections;
using System.Collections.Generic;
using Sting_Project.Sting_Project.Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class ItemButtonManager : MonoBehaviour
{
    //creando atributos
    private string itemName;//no lo ponemos publico porque otros podrian acceder y podria traer problemas
    //vamos a utilizar propiedades para controlar el acceso a este atributo

    private string itemDescription;
    private Sprite itemImage;
    private GameObject item3DModel;
    private ARInteractionsManager interactionsManager;
    //private string urlBundleModel;//campo para asignar el link de descarga de asset bundle
    //private RawImage imageBundle;//otro para asignar la imagen que ha sido desacrgada



    public string ItemName    {//get significa que puedo acceder al valor de este atributo y set que puedo camboar?p, se les dice getters and setters
        set
        {
            itemName = value;

        }
    
    }
    //creando propiedad para asignar el valor de cada uno de los atributos hechos arriba
    public string ItemDescription { set => itemDescription=value; }
    public Sprite ItemImage { set => itemImage = value; }
    public GameObject Item3DModel { set => item3DModel = value; }
    //public string URLBundleModel { set => urlBundleModel = value; }
    //public RawImage ImageBundle { get => imageBundle; set => imageBundle = value; }



    // Start is called before the first frame update
    void Start()
    {
        //para que cuando se cree el boton este automaticamente asigne los valores
        transform.GetChild(0).GetComponent<Text>().text = itemName;//tomara el valor de itemName
        transform.GetChild(1).GetComponent<RawImage>().texture = itemImage.texture;
        //imageBundle = transform.GetChild(1).GetComponent<RawImage>();
        transform.GetChild(2).GetComponent<Text>().text = itemDescription;

        var button = GetComponent<Button>();
        button.onClick.AddListener(Sting_Project.Sting_Project.Scripts.GameManager.instance.ARPosition);//cuando presione el boton se agregara un evento llamado gamemanager.instance.arposition
        button.onClick.AddListener(Create3DModel);//llamamos a la funcion para crear el modelo 3d elegido

        interactionsManager = FindObjectOfType<ARInteractionsManager>();

    }

    private void Create3DModel()//funcion que crea los modelos 3d
    {

        //quiero que una vez el modelo sea creado lo asigne en arinteractionmanager 
        interactionsManager.Item3DModel=Instantiate(item3DModel); //que sea igual al modelo que hemos creado
        //StartCoroutine(DownloadAssetBundle(urlBundleModel));


    }

    //corutina que descargar el assetbundle
    //IEnumerator DownloadAssetBundle(string urlAssetBundle)
    //{
        //UnityWebRequest serverRequest = UnityWebRequestAssetBundle.GetAssetBundle(urlAssetBundle);
        //yield return serverRequest.SendWebRequest();
        //if (serverRequest.result==UnityWebRequest.Result.Success)//ahora comprobare si el request ha sido exitoso
        //{
            //AssetBundle model3D = DownloadHandlerAssetBundle.GetContent(serverRequest);
            //if(model3D!=null)//comprobare que este asset bundle no este vacio
            //{
                //interactionsManager.Item3DModel = Instantiate(model3D.LoadAsset(model3D.GetAllAssetNames()[0]) as GameObject);//as gameobject para que sea un gameobject

            //}
            //else
            //{
                //Debug.Log("Not a valid Asset Bundle");

            //}

        //}
        //else
        //{
            //Debug.Log("Error");
        //}
         


    //}

    
}
