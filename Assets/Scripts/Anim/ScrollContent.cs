using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollContent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    bool touched = false;
    float speedScroll;

    public float currentScroll;
    public float maxScroll = 0;

    public float marginBottom;

    Vector2[] startPoints;
    Vector2 top;

    float scrollVelocity;

    public ScrollBar scrollBar;
    // Start is called before the first frame update
    public void Start()
    {
        startPoints = new Vector2[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
            startPoints[i] = transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
        
        top = new Vector2(transform.up.x, transform.up.y);
        currentScroll = 0;

        if(maxScroll == 0){
            RecalculateScroll();
            marginBottom = -maxScroll;
            maxScroll = 1080;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if(maxScroll < 0){
            speedScroll = 0;
            currentScroll = 0;
            return;
        }
        if(Mathf.Abs(speedScroll) > 0.01f){
            currentScroll += speedScroll;
            currentScroll = Mathf.Clamp(currentScroll, 0, maxScroll+marginBottom);
            UpdateObjects();
        }

        if(touched){
            speedScroll = 0;
            scrollVelocity = 0;
        }else
            speedScroll = Mathf.SmoothDamp(speedScroll, 0, ref scrollVelocity, 0.3f);
    }

    public void UpdateObjects(){
        for(int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = startPoints[i] + Vector2.up*currentScroll;
        }

        if(scrollBar) scrollBar.SetPosition(currentScroll, maxScroll+marginBottom, GetComponent<RectTransform>().rect.height);
    }

    public void MoveObjects (float height){
        for(int i = 0; i < transform.childCount; i++){
            startPoints[i] += Vector2.up * height;
        }
    }

    public void RecalculateScroll(){ 
        currentScroll = 0;
        UpdateObjects();

        RectTransform rectTransform = GetComponent<RectTransform>();
        float bottom = Vector2.Dot(rectTransform.rect.size, top)/2;
        
        RectTransform childRectTransform = transform.GetChild(startPoints.Length-1).GetComponent<RectTransform>();
        float bottomChild = Vector2.Dot(-childRectTransform.anchoredPosition+childRectTransform.rect.size, top);

        maxScroll = bottomChild-bottom;
        if(scrollBar) {
            scrollBar.SetPosition(0, maxScroll+marginBottom, GetComponent<RectTransform>().rect.height);
    
            if(maxScroll < 0)
                scrollBar.SetVisible(false);
            else
                scrollBar.SetVisible(true);
        }
    }

    public void OnPointerDown(PointerEventData data){
        touched = true;
    }

    public void OnBeginDrag (PointerEventData data){
        touched = true;
    }

    public void OnDrag(PointerEventData data) {
        speedScroll += Vector2.Dot(data.delta, top) / (Screen.height / 1080f);
    }

    public void OnEndDrag(PointerEventData data){
        speedScroll += Vector2.Dot(data.delta, top);
        touched = false;
    }

    public void Restart(){
        currentScroll = 0;
        speedScroll = 0;
        Start();
    }
}
