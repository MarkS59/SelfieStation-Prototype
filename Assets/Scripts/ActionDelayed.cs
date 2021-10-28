using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionDelayed : MonoBehaviour
{
    public ActionList[] actionList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Action(){
        gameObject.SetActive(true);
        StartCoroutine(_Action());
    }

    IEnumerator _Action(){
        for(int i = 0; i < actionList.Length; i++){
            if(actionList[i].delay > 0)
                yield return new WaitForSeconds(actionList[i].delay);
            actionList[i].action.Invoke();
        }
        
    }
}

[Serializable]
public class ActionList {
    public float delay = 2;
    public UnityEvent action;
}