using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ShareScreenShot : MonoBehaviour
{
	[SerializeField] private GameObject mainMenuCanvas;//campo para desactivar la interfaz
	private ARPointCloudManager aRPointCloudManager;//campo para nube de puntos

	    
    void Start()
    {
		aRPointCloudManager = FindObjectOfType<ARPointCloudManager>();//referenciando la nube de puntos
    }



	public void TakeScreenShot()
	{
		TurnOnOffARContents();//antes de tomar el screenshot para que desactive los elementos
	
		StartCoroutine(TakeScreenshotAndShare());//llamando corutina
    }


	
	
	private void TurnOnOffARContents()//funcion para ocultar nube de puntos cuando la foto es tomada
    {
		var points = aRPointCloudManager.trackables;//desactivar cada punto donde se encuentra la nuba de puntos
        foreach (var point  in points)
        {
			point.gameObject.SetActive(!point.gameObject.activeSelf);

        }
		mainMenuCanvas.SetActive(!mainMenuCanvas.activeSelf);//esto me permite que si esta activado se desactive y viceversa

    }



	private IEnumerator TakeScreenshotAndShare()
	{
		yield return new WaitForEndOfFrame();

		Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
		File.WriteAllBytes(filePath, ss.EncodeToPNG());

		// To avoid memory leaks
		Destroy(ss);

		new NativeShare().AddFile(filePath)
			.SetSubject("Subject goes here").SetText("Hey!...Like and suscribe")
			.SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
			.Share();
		TurnOnOffARContents();//y la volvere a llamar cuando la aplicacion haya tomado el screenshot para que active los elementos


		// Share on WhatsApp only, if installed (Android only)
		//if( NativeShare.TargetExists( "com.whatsapp" ) )
		//	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
	}


}
