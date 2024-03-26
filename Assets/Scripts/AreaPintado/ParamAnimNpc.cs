using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamAnimNpc : MonoBehaviour
{
    [SerializeField] private Animator animator1;
    [SerializeField] private Animator animator2;
    [SerializeField] private Animator animator3;
    [SerializeField] private Animator animator4;
    private bool estaPermitido1 = true;
   
    public void ActivarAnimacionBool1(string boolParametro)
    {
        animator1.SetBool(boolParametro, true);
    }
    
    public void DesactivarAnimacionBool1(string boolParametro)
    {
        animator1.SetBool(boolParametro, false);
    }

    public void ActivarAnimacionTrigger1(string triggerParametro)
    {
        animator1.SetTrigger(triggerParametro);
    }
    public void ActivarAnimacionBool2(string boolParametro)
    {
        animator2.SetBool(boolParametro, true);
    }
    
    public void DesactivarAnimacionBool2(string boolParametro)
    {
        animator2.SetBool(boolParametro, false);
    }

    public void ActivarAnimacionTrigger2(string triggerParametro)
    {
        animator2.SetTrigger(triggerParametro);
    }
    public void ActivarAnimacionBool3(string boolParametro)
    {
        animator3.SetBool(boolParametro, true);
    }
    
    public void DesactivarAnimacionBool3(string boolParametro)
    {
        animator3.SetBool(boolParametro, false);
    }

    public void ActivarAnimacionTrigger3(string triggerParametro)
    {
        animator3.SetTrigger(triggerParametro);
    }
    public void ActivarAnimacionBool4(string boolParametro)
    {
        animator4.SetBool(boolParametro, true);
    }
    
    public void DesactivarAnimacionBool4(string boolParametro)
    {
        animator4.SetBool(boolParametro, false);
    }

    public void ActivarAnimacionTrigger4(string triggerParametro)
    {
        animator4.SetTrigger(triggerParametro);
    }

    public void PermitirAnimacionBool1(string boolParametro)
    {
        estaPermitido1 = !estaPermitido1;
        animator1.SetBool(boolParametro, estaPermitido1);
    }
}
