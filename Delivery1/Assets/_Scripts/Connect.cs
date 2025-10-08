using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class Connect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnEnable()
    {
       
        Simulator.OnNewPlayer += OnPlayerCreated;
        Simulator.OnNewSession += OnNewSession;
        Simulator.OnBuyItem += OnBuyItem;
        Simulator.OnEndSession += OnEndSession;
    }

    void OnDisable()
    {
       
        Simulator.OnNewPlayer -= OnPlayerCreated;
        Simulator.OnNewSession -= OnNewSession;
        Simulator.OnBuyItem -= OnBuyItem;
        Simulator.OnEndSession -= OnEndSession;
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

        formData.Add(new MultipartFormDataSection("type", "NewPlayer"));

        StartCoroutine(UploadPlayer(formData));
    }

    void OnNewSession(DateTime date, uint playerId)
    {
        Debug.Log("New session started for player: " + playerId);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("user_id", playerId.ToString()));
        formData.Add(new MultipartFormDataSection("date_time", date.ToString("yyyy-MM-dd HH:mm:ss")));
        formData.Add(new MultipartFormDataSection("type", "NewSession"));

        StartCoroutine(UploadNewSession(formData));
    }

    //Aleix
    void OnBuyItem(int item, DateTime dateTime, uint sessionID)
    {
        //user ID, Item,  hora

        Debug.Log("New item buyed for player: " + sessionID);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("item", item.ToString()));
        formData.Add(new MultipartFormDataSection("session_id", sessionID.ToString()));
        formData.Add(new MultipartFormDataSection("date_time", dateTime.ToString("yyyy-MM-dd HH:mm:ss")));
        formData.Add(new MultipartFormDataSection("type", "NewItem"));

        StartCoroutine(UploadItem(formData));
    }

    //Guillem
    void OnEndSession(DateTime date, uint sessionID)
    {
        Debug.Log("End session for player: " + sessionID);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("session_id", sessionID.ToString()));
        formData.Add(new MultipartFormDataSection("session_time", date.ToString("yyyy-MM-dd HH:mm:ss")));
        formData.Add(new MultipartFormDataSection("type", "EndSession"));
        StartCoroutine(UploadEndSession(formData));
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

    IEnumerator UploadPlayer(List<IMultipartFormSection> formData) 
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
                CallbackEvents.OnAddPlayerCallback?.Invoke(uint.Parse(www.downloadHandler.text));
        }
    }

    IEnumerator UploadNewSession(List<IMultipartFormSection> formData)
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
            CallbackEvents.OnNewSessionCallback?.Invoke(uint.Parse(www.downloadHandler.text));

        }
    }
    IEnumerator UploadItem(List<IMultipartFormSection> formData)
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
            CallbackEvents.OnItemBuyCallback?.Invoke(uint.Parse(www.downloadHandler.text));

        }
    }
    IEnumerator UploadEndSession(List<IMultipartFormSection> formData)
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
            //CallbackEvents.OnNewSessionCallback?.Invoke(uint.Parse(www.downloadHandler.text));
            // CallbackEvents.OnItemBuyCallback?.Invoke(uint.Parse(www.downloadHandler.text));
             CallbackEvents.OnEndSessionCallback?.Invoke(uint.Parse(www.downloadHandler.text));
        }
    }
}