using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Connect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://citmalumnes.upc.es/~jial/login.php");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("Respuesta del PHP: " + www.downloadHandler.text);
        }
    }


}
