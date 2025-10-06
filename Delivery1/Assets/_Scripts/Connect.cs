using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class Connect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Simulator simulator;

    void OnEnable()
    {
       
        Simulator.OnNewPlayer += OnPlayerCreated;
    }

    void OnDisable()
    {
       
        Simulator.OnNewPlayer -= OnPlayerCreated;
    }

   
    void OnPlayerCreated(string name, string country, int age, float gender, DateTime date)
    {
        Debug.Log($"Name: {name}");
        Debug.Log($"Country: {country}");
        Debug.Log($"Age: {age}");
        Debug.Log($"Gender (float): {gender}");
        Debug.Log($"Date: {date.ToString("yyyy-MM-dd HH:mm:ss")}");

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("user", name));
        formData.Add(new MultipartFormDataSection("country", country));
        formData.Add(new MultipartFormDataSection("age", age.ToString()));
        formData.Add(new MultipartFormDataSection("gender", gender.ToString()));
        formData.Add(new MultipartFormDataSection("register_date", date.ToString("yyyy-MM-dd HH:mm:ss")));

        
        StartCoroutine(Upload(formData));
        

    }
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

    IEnumerator Upload(List<IMultipartFormSection> formData) 
        {
            UnityWebRequest www = UnityWebRequest.Post("https://citmalumnes.upc.es/~jial/uploadData.php", formData);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {

                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log("Respuesta del PHP: " + www.downloadHandler.text);
        }
    }

}
