using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Sting_Project.Sting_Project.Scripts
{
    public class ServerManager : MonoBehaviour
    {

        [SerializeField] private string jsonURL;//el campo para asignar el link del archivo json
        [SerializeField] private string jsonURLPruebas;//el campo para asignar el link del archivo json para pruebas
        [SerializeField] private ItemButtonManager itemButtonManager;
        [SerializeField] private GameObject buttonsContainer;//para poder visualizar la estructura en el inspector
        
        [Serializable]
        public struct Items//creando misma estructura que tiene el archivo json
        {
            [Serializable]//para poder visualizar la estructura en el inspector
            public struct Item
            {
                public string Name;
                public string Description;
                public string URLBundleModel;//el link para descargar el asset bundle
                public string URLImageModel;//el link para descargar la imagen del modelo
            }

            public Item[] items;

            

        }
        public Items newItemsCollection = new Items();


        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(GetJsonData());
            //Creo y suscribo la funcion que creara los botones
            global::Sting_Project.Sting_Project.Scripts.GameManager.instance.OnItemsMenu += CreateButtons;

        }

        private void CreateButtons()
        {
            foreach (var item in newItemsCollection.items)//entonces que asigne la informacion que se encuentra en el json
            {
                ItemButtonManager itemButton;
                itemButton = Instantiate(itemButtonManager, buttonsContainer.transform);//sera igual al nuevo boton que ha sido creado
                itemButton.name = item.Name;
                itemButton.ItemName = item.Name;
                itemButton.ItemDescription = item.Description;
                //itemButton.URLBundleModel = item.URLBundleModel;
                StartCoroutine(GetBundleImage(item.URLImageModel, itemButton));

            }
            global::Sting_Project.Sting_Project.Scripts.GameManager.instance.OnItemsMenu -= CreateButtons;
        }

      

        //funcion que descargar el archivo json y lo asignara a la estructura que hemos creado
        IEnumerator GetJsonData()//creamos corutina
        {
            UnityWebRequest serverRequest = UnityWebRequest.Get(jsonURL);//le pasare el link del archivo json
            yield return serverRequest.SendWebRequest();//hare que esta corutina espere hasta que tenga una respuesta al request
            if (serverRequest.result==UnityWebRequest.Result.Success)//ahora compruebo si el resultado request ha sido exitoso
            {
                newItemsCollection = JsonUtility.FromJson<Items>(serverRequest.downloadHandler.text);//y si ha sido asi asignare la informacion del json dentro de la estructura que hemos creado
            }
            else
            {
                Debug.Log("Error :c");
            }

        }
        //corutina para descargar la imagen
        IEnumerator GetBundleImage(string urlImage, ItemButtonManager button)
        {
            UnityWebRequest serverRequest = UnityWebRequest.Get(urlImage);//le paso el link de la imagen
            serverRequest.downloadHandler = new DownloadHandlerTexture();

            yield return serverRequest.SendWebRequest();//hare que esta corutina espere hasta que tenga una respuesta al request
            if (serverRequest.result == UnityWebRequest.Result.Success)//ahora compruebo si el resultado request ha sido exitoso
            {
                //button.ImageBundle.texture = ((DownloadHandlerTexture)serverRequest.downloadHandler).texture;//aqui asigno la imagen
            }
            else
            {
                Debug.Log("Error :c");
            }

        }


    }

}
