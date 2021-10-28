using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;

public class ServerCommunicator : MonoBehaviour
{
    public string currentStatus = "available";
    string url = "";
    string stand_id = "";
    string token = "";

    public Action[] actions = {
        new Action("available"),
        new Action("photo")
    }; 

    // Start is called before the first frame update
    void Start()
    {
        this.url = LoadSettings.instance.get("server_url");
        this.stand_id = LoadSettings.instance.get("stand_id");
        this.token = LoadSettings.instance.get("stand_token");
        StartCoroutine(AjaxRequest());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendPhoto(Texture2D image) {
        StartCoroutine(SendPhotoRequest(image));
    }

    IEnumerator AjaxRequest() {
        while(true){
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url+"/stands/"+stand_id))
            {
                webRequest.SetRequestHeader("Authorization", "Bearer "+token);
                yield return webRequest.SendWebRequest();
                
                if(webRequest.isNetworkError || webRequest.isHttpError){
                    Debug.Log(webRequest.downloadHandler.text);
                    if(webRequest.isNetworkError)
                        Debug.Log("Host " + url + " unavailable");
                    break;
                }
                
                string text = webRequest.downloadHandler.text;
                AjaxResponse response = JsonUtility.FromJson<AjaxResponse>(text);
                if(response.status != currentStatus){
                    foreach(var action in actions){
                        if(action.status == response.status)
                            action.action.Invoke();
                    }
                    currentStatus = response.status;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator SendPhotoRequest (Texture2D image){

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("title", "yet one photo"));
        formData.Add(new MultipartFormFileSection("file", image.EncodeToJPG(90), "photo.jpg", "image/jpeg"));

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url+"/files?fields=id", formData))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer "+token);
            yield return webRequest.SendWebRequest();
            string text = webRequest.downloadHandler.text;
            
            byte[] utf8text = Encoding.UTF8.GetBytes(text);

            using (UnityWebRequest webRequest2 = new UnityWebRequest(url+"/stands/"+stand_id+"/upload", "POST"))
            {
                webRequest2.uploadHandler = new UploadHandlerRaw(utf8text);
                webRequest2.downloadHandler = new DownloadHandlerBuffer();
                webRequest2.SetRequestHeader("Authorization", "Bearer "+token);
                webRequest2.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
                
                yield return webRequest2.SendWebRequest();
                string resp = webRequest2.downloadHandler.text;

                Debug.Log(resp);
            }
        }
    
    }
}

[Serializable]
public class Action {
    public string status;
    public UnityEvent action;

    public Action(string status){
        this.status = status;
    }
}

[Serializable]
public class AjaxResponse {
    public string status;
    public string photo;
}