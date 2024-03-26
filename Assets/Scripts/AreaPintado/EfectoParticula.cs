using UnityEngine;

public class EfectoParticula : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] efectoParticula;

    public void EjecutarEfectoParticula(int indice)
    {
        var particle = efectoParticula[indice];
        particle.Play(true);
        var emision = particle.emission;
        emision.enabled = true;

    }

    public void DetenerEfectoParticula(int indice)
    {
        var particle = efectoParticula[indice];
        particle.Clear(true);
        particle.Stop(true);
        var emision = particle.emission;
        emision.enabled = false;
        
    }
}
