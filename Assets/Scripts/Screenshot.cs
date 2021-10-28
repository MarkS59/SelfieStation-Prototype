using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screenshot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Texture2D scr;
    public bool grab = false;

    public Texture2D ramka;

    public void TakeScreen()
    {
        grab = true;
    }
    private void OnPostRender()
    {
        if (grab)
        {
            int width = UnityEngine.Screen.width;
            int height = UnityEngine.Screen.height;
            //int height = (int)(width*1.5f);
            
            RenderTexture activeRenderTexture = RenderTexture.active;
            RenderTexture.active = GetComponent<Camera>().activeTexture;

            scr = new Texture2D(width, height, TextureFormat.RGB24, false);
            scr.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);

            /*if (ramka != null && width == ramka.width)
            {
                var cols = scr.GetPixels32();
                var _ramka = ramka.GetPixels32();
                Color colos;
                for (int i = 0; i < cols.Length; i++)
                {
                    colos = Color.Lerp(cols[i], _ramka[i], _ramka[i].a / 255f);
                    colos.a = 1;
                    scr.SetPixel(i % width, i / width, colos);
                }
            }*/

            scr.Apply();
            GameObject.FindObjectOfType<ImageLoader>().LoadImage(scr);
            grab = false;
            
            RenderTexture.active = activeRenderTexture;
        }
    }
}
