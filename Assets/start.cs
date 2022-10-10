using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{
    public TMP_Text[] buttons;
    public PlayerControls controls;
    public bool but=false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(but){
            buttons[1].color = Color.red;
            buttons[0].color = Color.white;
        }
        else if(!but){
            buttons[0].color = Color.red;
            buttons[1].color = Color.white;
        }
    }

    void Awake(){
        controls= new PlayerControls();

        controls.player.forward.performed += ctx => but=false;
        controls.player.backward.performed += ctx => but=true;
        controls.buttons.enter.performed += ctx => click();
    }

    void click(){
        if(!but){
            SceneManager.LoadScene("main");
        }
        else if(but){
            Application.Quit();
        }
    }

    
    private void OnEnable() {
        controls.player.forward.Enable();
        controls.player.backward.Enable();
        controls.buttons.enter.Enable();
    }

    private void OnDisable() {
        controls.player.forward.Disable();
        controls.player.backward.Enable();
        controls.buttons.enter.Disable();
    }
}
