using System;
using UnityEngine;

//referenciamos un name space

namespace Sting_Project.Sting_Project.Scripts
{
    public class GameManager : MonoBehaviour
    {
        //eventos que representaran los estados de nuestra aplicacion
        public event Action OnMainMenu;
        public event Action OnItemsMenu;
        public event Action OnARPosition;

        public static GameManager instance;    //para poder llamar y suscribirnos a estos eventos nacesitamos un patron singleton
        //singleton es un patron de dise�o que describe una clase que es accesible globalmente y que solo se puede instanciar una vez

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);

            }
            else
            {
                instance = this;
            }
            //con esto nos estamos asegurando a que solo exista una instancia de gameManeger
        }


        // Start is called before the first frame update
        void Start()
        {
            MainMenu();

        }
        //ahora vamos a crear las funciones para poder llamar estos eventos
        public void MainMenu()
        {
            OnMainMenu?.Invoke();//el simbolo de pregunta lo que hace es constatar de que existe algo que esta suscrito a este evento
            Debug.Log("Main Menu Activated");

        }
        public void ItemsMenu()
        {
            OnItemsMenu?.Invoke();
            Debug.Log("Items Menu Activated");
        }

        public void ARPosition()
        {
            OnARPosition?.Invoke();
            Debug.Log("AR Position Activated");

        }
        public void CloseAPP()
        {
            Application.Quit();
        }

    
    }
}