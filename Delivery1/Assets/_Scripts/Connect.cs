using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class Connect : MonoBehaviour
{

    //Nos añadimos a las acciones
    void OnEnable()
    {
       
        Simulator.OnNewPlayer += OnPlayerCreated;
        Simulator.OnNewSession += OnNewSession;
        Simulator.OnBuyItem += OnBuyItem;
        Simulator.OnEndSession += OnEndSession;
    }

    //Nos desvinculamos a las acciones
    void OnDisable()
    {
       
        Simulator.OnNewPlayer -= OnPlayerCreated;
        Simulator.OnNewSession -= OnNewSession;
        Simulator.OnBuyItem -= OnBuyItem;
        Simulator.OnEndSession -= OnEndSession;
    }

    //Al crear un player nuevo
    void OnPlayerCreated(string name, string country, int age, float gender, DateTime date)
    {
        //INFORMACION DE DEBUG
        //Debug.Log($"Name: {name}");
        //Debug.Log($"Country: {country}");
        //Debug.Log($"Age: {age}");
        //Debug.Log($"Gender (float): {gender}");
        //Debug.Log($"Date: {date.ToString("yyyy-MM-dd HH:mm:ss")}");

        //Rellenamos los datos
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("user", name));
        formData.Add(new MultipartFormDataSection("country", country));
        formData.Add(new MultipartFormDataSection("age", age.ToString()));
        formData.Add(new MultipartFormDataSection("gender", gender.ToString()));
        formData.Add(new MultipartFormDataSection("register_date", date.ToString("yyyy-MM-dd HH:mm:ss")));
        formData.Add(new MultipartFormDataSection("type", "NewPlayer"));

        //Enviamos los datos y decimos que hacer con la respuesta
        StartCoroutine(UploadData(formData, CallbackEvents.OnAddPlayerCallback));
    }

    void OnNewSession(DateTime date, uint playerId)
    {
        //INFORMACION
        Debug.Log("New session started for player: " + playerId);

        //Rellenamos los datos
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("user_id", playerId.ToString()));
        formData.Add(new MultipartFormDataSection("date_time", date.ToString("yyyy-MM-dd HH:mm:ss")));
        formData.Add(new MultipartFormDataSection("type", "NewSession"));

        //Enviamos los datos y decimos que hacer con la respuesta
        StartCoroutine(UploadData(formData, CallbackEvents.OnNewSessionCallback));
    }

   
    void OnBuyItem(int item, DateTime dateTime, uint sessionID)
    {

        //INFORMACION
        Debug.Log("New item buyed for player: " + sessionID);

        //Rellenamos los datos
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("item", item.ToString()));
        formData.Add(new MultipartFormDataSection("session_id", sessionID.ToString()));
        formData.Add(new MultipartFormDataSection("date_time", dateTime.ToString("yyyy-MM-dd HH:mm:ss")));
        formData.Add(new MultipartFormDataSection("type", "NewItem"));

        //Enviamos los datos y decimos que hacer con la respuesta
        StartCoroutine(UploadData(formData, CallbackEvents.OnItemBuyCallback));
    }

 
    void OnEndSession(DateTime date, uint sessionID)
    {
        //INFORMACION
        Debug.Log("End session for player: " + sessionID);

        //Rellenamos los datos
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("session_id", sessionID.ToString()));
        formData.Add(new MultipartFormDataSection("session_time", date.ToString("yyyy-MM-dd HH:mm:ss")));
        formData.Add(new MultipartFormDataSection("type", "EndSession"));

        //Enviamos los datos y decimos que hacer con la respuesta
        StartCoroutine(UploadData(formData, CallbackEvents.OnEndSessionCallback));
    }

    //Enviado de los datos
    IEnumerator UploadData(List<IMultipartFormSection> formData, Action<uint> onOk)
    {
        //Enviamos los datos utilizando POST (mas seguro que el GET)
        UnityWebRequest www = UnityWebRequest.Post("https://citmalumnes.upc.es/~jial/uploadData.php", formData);
        yield return www.SendWebRequest();

        //Si hay algun error, mostramos la informacion
        if (www.result != UnityWebRequest.Result.Success)
        {

            Debug.Log(www.error);
            yield break;
        }
        else
        {
            //Mostramos la informacion conseguida del php, y invocamos el metodo que haga falta para leer los datos 
            //Debug.Log("Form upload complete!");
            Debug.Log("Respuesta del PHP: " + www.downloadHandler.text);
            onOk.Invoke(uint.Parse(www.downloadHandler.text));
        }
    }
}