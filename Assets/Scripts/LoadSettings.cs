using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadSettings : MonoBehaviour
{   
    public static LoadSettings instance;
    public Texture2D ramka;
    public Dictionary<string, string> settings = new Dictionary<string, string>();
    // Start is called before the first frame update
    void Start()
    {
        if (File.Exists(Application.dataPath + "/../settings.txt"))
        {
            var arr = File.ReadAllLines(Application.dataPath + "/../settings.txt");

            foreach (string str in arr)
            {
                if (!str.Contains(":"))
                    continue;

                string _str = str.Split(';')[0];
                int i = _str.IndexOf(':');

                string key = _str.Substring(0, i).Trim();
                string value = _str.Substring(i+1).Trim();

                settings.Add(key, value);
            }
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            bool fs = Screen.fullScreen;
            Screen.fullScreen = !fs;
            if (!fs)
                Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public string get(string key, string defaultValue = "")
    {
        if(settings.ContainsKey(key))
            return settings[key];
        return defaultValue;
    }

    public bool getBool(string key, bool defaultValue = false)
    {
        if (settings.ContainsKey(key))
            return settings[key] == "true";
        return defaultValue;
    }

    public int getInt(string key, int defaultValue = 0)
    {
        int iset;
        if(settings.ContainsKey(key) && int.TryParse(settings[key], out iset))
            return iset;

        return defaultValue;
    }

    public float getFloat(string key, float defaultValue = 0f)
    {
        float iset;
        if (settings.ContainsKey(key) && float.TryParse(settings[key], out iset))
            return iset;

        return defaultValue;
    }
}
