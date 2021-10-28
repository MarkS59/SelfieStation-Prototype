using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    float lastTime = 0;
    public int seconds = 5;
    bool started = false;
    public Text text;

    public UnityEvent onFinal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(started){
            float time = Mathf.Ceil(lastTime+seconds-Time.time);
            text.text = "Ждите " + time.ToString() + " секунд";
            if(time <= 0){
                text.text = "Фото";
                started = false;
                onFinal.Invoke();
                return;
            }
        }
    }

    public void StartTimer(){
        this.lastTime = Time.time;
        started = true;
    }
}
