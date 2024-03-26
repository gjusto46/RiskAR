using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoAnim : MonoBehaviour
{
    [SerializeField] private GameObject[] objeto;
    
    public void ActivarObjeto(int indice)
    {
        if (objeto.Length > indice-1)
        {
            objeto[indice].SetActive(true);
        }
        else
        {
            Debug.Log($"Indice {indice} fuera de rango");
        }
    }
    
    public void DesactivarObjeto(int indice)
    {
        if (objeto.Length > indice)
        {
            objeto[indice].SetActive(false);
        }
        else
        {
            Debug.Log($"Indice {indice} fuera de rango");
        }
    }
}
