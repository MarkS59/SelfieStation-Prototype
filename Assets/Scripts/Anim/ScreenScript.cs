using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenScript : AnimObject
{
    public List<AnimObject> children = new List<AnimObject>();

    Vector2 startPosition;
    public UnityEvent onOpen;
    public UnityEvent onClose;

    public Button backButton;
    
    // Start is called before the first frame update
    
    void Start()
    {   
        startPosition = GetComponent<RectTransform>().anchoredPosition;

        if(!show) gameObject.SetActive(false);

        if(children.Count == 0) FillObject();
    }

    [ContextMenu("Fill object")]
    void FillObject(){
        children.Clear();
        for(int i = 0; i < transform.childCount; i++){
            if(transform.GetChild(i).GetComponent<AnimObject>())
                children.Add(transform.GetChild(i).GetComponent<AnimObject>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenScreen(){
        GameObject.FindObjectOfType<ScreenController>().OpenScreen(this);
    }

    DG.Tweening.Tween callback;
    public override void Open(){
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        show = true;
        gameObject.SetActive(true);
        foreach(var anim in children){
            anim.Open();
        }
        if(callback != null) callback.Kill(false);
    }

    public override void Close(){
        show = false;
        foreach(var anim in children){
            anim.Close();
        }
        callback = DOVirtual.DelayedCall(3, () => {
            gameObject.SetActive(false);
            GetComponent<RectTransform>().anchoredPosition = startPosition;
        });
    }

}
