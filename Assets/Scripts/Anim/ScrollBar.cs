using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBar : MonoBehaviour
{
    public Image bar;
    Vector2 startPosition;
    float height;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = bar.rectTransform.anchoredPosition;
        height = GetComponent<RectTransform>().sizeDelta.y + startPosition.y*2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPosition(float currentScroll, float maxScroll, float pageSize){
        bar.rectTransform.sizeDelta = new Vector2(bar.rectTransform.sizeDelta.x, height * pageSize/(maxScroll+pageSize));
        bar.rectTransform.anchoredPosition = startPosition + Vector2.down * currentScroll/(maxScroll+pageSize) * height;
    }

    public void SetVisible(bool value){
        bar.enabled = value;
        GetComponent<Graphic>().enabled = value;
    }
}
