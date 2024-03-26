using System;
using System.Collections;
using System.Collections.Generic;
using Sting_Project.Sting_Project.Scripts;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();//definimos los campos, en este caso es una lista de item
    [SerializeField] private GameObject buttonContainer;//este campo es el gameobject que va a contener los  botones
    [SerializeField] private ItemButtonManager itemButtonManager;//voy a referencia el script itemButtonManager el cual voy a llamar itemButtonManager

    void Start()
    {
        //suscribir y crear una funcion que creara los botones cuando el evento OnitemsMenu sea llamado
        Sting_Project.Sting_Project.Scripts.GameManager.instance.OnItemsMenu += CreateButtons;

        
    }

    //creando la funcion
    private void CreateButtons ()
    {
        foreach (var item in items)
        {
            ItemButtonManager itemButton;//creando variable itembuttonmanager que se va a llamar itembutton
            itemButton = Instantiate(itemButtonManager, buttonContainer.transform);//sera igual al boton que estoy creando
            itemButton.ItemName = item.ItemName;//asignando el valor para cada uno de los elementos del boton
            itemButton.ItemDescription = item.ItemDescription;
            itemButton.ItemImage = item.ItemImage;
            itemButton.Item3DModel = item.Item3DModel;
            itemButton.name = item.ItemName;//voy a nombrar cada boton como el nombre del item
        }
        Sting_Project.Sting_Project.Scripts.GameManager.instance.OnItemsMenu -= CreateButtons;//para que se desuscriba del evento onitemsmenu, con eso solo generara los botones una vez y no cada vez que se llame ese evento

    }


}
