using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sting_Project.Sting_Project.Scripts; //referenciamos el name space del paquete que hemos descargado
public class UIManager : MonoBehaviour

{

    [SerializeField] private GameObject mainMenuCanvas;//Creamos los campos de los GameObjetc que contiene nuestra interfaz
    [SerializeField] private GameObject itemsMenuCanvas;
    [SerializeField] private GameObject ARPositionCanvas;

    // Start is called before the first frame update
    void Start()//vamos a crear las funciones que van a estar suscritos a lso eventos de nuestro gameManager
    {
        Sting_Project.Sting_Project.Scripts.GameManager.instance.OnMainMenu += ActivateMainMenu;
        Sting_Project.Sting_Project.Scripts.GameManager.instance.OnItemsMenu += ActivateItemsMenu;
        Sting_Project.Sting_Project.Scripts.GameManager.instance.OnARPosition += ActivateARPosition;
        
    }



    private void ActivateMainMenu()//creamos la funcion ActivateMainMenu, con estas funciones vamos a empezar a animar nuestra interfaz
    {
        mainMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(1, 1, 1), 0.3f);
        mainMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(1, 1, 1), 0.3f);
        mainMenuCanvas.transform.GetChild(2).transform.DOScale(new Vector3(1, 1, 1), 0.3f);
            
        itemsMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(0, 0, 0), 0.5f);
        itemsMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(0, 0, 0), 0.3f);
        itemsMenuCanvas.transform.GetChild(1).transform.DOMoveY(180, 0.3f);

        ARPositionCanvas.transform.GetChild(0).transform.DOScale(new Vector3(0, 0, 0), 0.3f);
        ARPositionCanvas.transform.GetChild(1).transform.DOScale(new Vector3(0, 0, 0), 0.3f);

    }
   
    private void ActivateItemsMenu()//creamos la funcion ActivateItemsMenu

    {
        mainMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(0, 0, 0), 0.3f);
        mainMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(0, 0, 0), 0.3f);
        mainMenuCanvas.transform.GetChild(2).transform.DOScale(new Vector3(0, 0, 0), 0.3f);

        itemsMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(1, 1, 1), 0.5f);
        itemsMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(1, 1, 1), 0.5f);
        itemsMenuCanvas.transform.GetChild(1).transform.DOMoveY(300, 0.3f);



    }                                               


    private void ActivateARPosition()//creamos la funcion ActivateARPosition

    {

        mainMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(0, 0, 0), 0.3f);
        mainMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(0, 0, 0), 0.3f);
        mainMenuCanvas.transform.GetChild(2).transform.DOScale(new Vector3(0, 0, 0), 0.3f);

        itemsMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(0, 0, 0), 0.5f);
        itemsMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(0, 0, 0), 0.3f);
        itemsMenuCanvas.transform.GetChild(1).transform.DOMoveY(180, 0.3f);

        ARPositionCanvas.transform.GetChild(0).transform.DOScale(new Vector3(1, 1, 1), 0.3f);
        ARPositionCanvas.transform.GetChild(1).transform.DOScale(new Vector3(1, 1, 1), 0.3f);

    }

}
