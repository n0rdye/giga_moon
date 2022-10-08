using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class moving : MonoBehaviour
{
    public Rigidbody rb;
    public float grav = 3,speed =0.02f,Fspeed =0.02f,Rspeed =0.1f,f=0.02f;
    public float power=600,health=6,con;
    public bool shift=true;
    public PlayerControls controls;
    public bool fw=false,bw=false,rt=false,lt=false,sh=false;
    public TMP_Text text,conect,powertext,speedtext;
    public Transform b;
    public double Distance=0;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake() {
        
        controls= new PlayerControls();

        controls.player.forward.performed += ctx => fw=true;
        controls.player.forward.canceled += ctx => fw=false;
        controls.player.left.performed += ctx => lt=true;
        controls.player.left.canceled += ctx => lt=false;
        controls.player.right.performed += ctx => rt=true;
        controls.player.right.canceled += ctx => rt=false;
        controls.player.backward.performed += ctx => bw=true;
        controls.player.backward.canceled += ctx => bw=false;
        controls.player.shift.performed += ctx => sh=true;
        controls.player.shift.canceled += ctx => sh=false;
    }

    void healthcheck(){
        text.text="";
        for(int i=0;i<health;i++){
            text.text+="<3";
        }
    }

    void dist(){
        conect.text="";
        Distance=Vector3.Distance(this.transform.position, b.position);
        for(int i=0;i<Distance/6;i++){
            conect.text+="|";
        }
    }

    // Update is called once per frame
    void Update()
    {
        speedtext.text="";
        for(int i=0;i<speed*250;i++){
            speedtext.text+="/";
        }
        if (power > 0)
        {
            powertext.text="";
            power -= Time.deltaTime;
            for(int i=0;i<Math.Pow(power,1)/60;i++){
                powertext.text+="[]";
            }
        }
        dist();
        if(health<=0){
            Debug.Log("dead");
        }
        if(fw){
            transform.Translate (0f, 0f, speed);
        }
        if(bw){
            transform.Translate (0f, 0f, -speed);
        }
        if(lt){
            this.transform.Rotate (0f, -Rspeed, 0f);
        }
        if(rt){
            this.transform.Rotate (0f, Rspeed, 0f);
        }

        if (sh&&shift){
            speed=Fspeed+0.02f;
        }
        else{
            speed=Fspeed;
        }

        //jump
        if(Input.GetKey (KeyCode.W) && Input.GetKeyDown (KeyCode.Space)){
            rb.AddForce(transform.forward * grav);
            rb.AddForce(transform.up * grav);
        } else
        if (Input.GetKeyDown (KeyCode.Space)){
            rb.AddForce(transform.up * grav);
        }
    }

    private void OnEnable() {
        controls.player.forward.Enable();
        controls.player.left.Enable();
        controls.player.right.Enable();
        controls.player.backward.Enable();
        controls.player.shift.Enable();
    }

    private void OnDisable() {
        controls.player.forward.Disable();
        controls.player.left.Disable();
        controls.player.right.Disable();
        controls.player.backward.Disable();
        controls.player.shift.Disable();
    }
    
    private IEnumerator OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="border"){
            sh=false;shift=false;
            float r=Rspeed, s=speed;
            float r2=r/2, s2=s/2;

            if ((Fspeed-s2)>0&&s<=Fspeed){
                health-=1;
                Fspeed-=s2;Rspeed-=r2;
                healthcheck();
                yield return new WaitForSeconds(4);
                Fspeed+=s2;Rspeed+=r2;
                
            }
            else if((Fspeed-s2)<0&&s>=Fspeed){
                health-=1;
                Fspeed=0;Rspeed=0;
                healthcheck();
                yield return new WaitForSeconds(4);
                Fspeed=f;Rspeed=r;
            }

            shift=true;
        }
    }
}