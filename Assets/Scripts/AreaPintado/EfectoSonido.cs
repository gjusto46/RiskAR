using UnityEngine;

public class EfectoSonido : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sonidos;
    
    public void EjecutarSonidoUnaVez(int indice)
    {
        audioSource.PlayOneShot(sonidos[indice]);
    }

    public void EjecutarSonidoVariasVeces(int indice)
    {
        audioSource.clip = sonidos[indice];
        audioSource.Play();
        audioSource.loop = true;
    }

    public void DetenerSonido()
    {
        audioSource.Stop();
        audioSource.loop = false;
    }
}
