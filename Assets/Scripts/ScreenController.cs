using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenController : MonoBehaviour
{
    public bool clicked = false;
    public ScreenScript currentScreen;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var screen in GameObject.FindObjectsOfType<ScreenScript>()){
            if(screen != currentScreen){
                screen.show = false;
            }
        }
        StartCoroutine(_Start());
    }

    IEnumerator _Start (){
        currentScreen.onOpen.Invoke();
        yield return new WaitForSeconds(0.1f);
        currentScreen.Open();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackOpenScreen(ScreenScript screen){
        if(clicked) return;
        if(currentScreen.backButton)
            currentScreen.backButton.onClick.RemoveAllListeners();
        currentScreen.onClose.Invoke();
        _open(screen);
    }

    public void OpenScreen (ScreenScript screen){
        if(clicked) return;
        if(screen.backButton != null){
            ScreenScript lastScreen = currentScreen;
            screen.backButton.onClick.AddListener(() => this.BackOpenScreen(lastScreen));
        }
        _open(screen);
    }

    void _open(ScreenScript screen){
        clicked = true;
        foreach(var audio in GameObject.FindObjectsOfType<AudioSource>())
            audio.Stop();
        currentScreen.Close();
        currentScreen = screen;
        currentScreen.onOpen.Invoke();
        StartCoroutine(_OpenScreen());
    }

    public IEnumerator _OpenScreen (){
        yield return new WaitForSeconds(0.4f);
        currentScreen.Open();
        clicked = false;
    }
}
