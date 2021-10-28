using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string deviceName = WebCamTexture.devices[0].name;
        WebCamTexture texture = new WebCamTexture(deviceName, 1920, 1080, 30);
        GetComponent<RawImage>().texture = texture;
        texture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
