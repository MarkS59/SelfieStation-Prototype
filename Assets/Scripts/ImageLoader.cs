using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ImageLoader : MonoBehaviour
{
    public RawImage previewPhoto;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadImage(Texture2D image){
        previewPhoto.texture = image;
        GameObject.FindObjectOfType<ServerCommunicator>().SendPhoto(image);
    }

}
