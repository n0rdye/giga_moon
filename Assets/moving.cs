using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class moving : MonoBehaviour
{
    public Rigidbody rb;
    public float grav = 3,speed =0.02f,Fspeed =0.02f,Rspeed =0.1f;
    public float power=600,health=6,wakespeed=3,powermult=0;
    public bool shift=true;
    public PlayerControls controls;
    private float count=3;
    public bool fw=false,bw=false,rt=false,lt=false,sh=false,interact=false,interact2=false,interact3=false,mine=false,mined=false,mine_end=false,end=false;
    public TMP_Text text,conect,powertext,speedtext;
    public Transform b;
    public double Distance=0;
    public Camera cam;
    public TMP_Text deb_text;
    public float owerload=0;
    public TMP_Text[] buttons;
    public GameObject buttons_obj;
    public int buttons_rand= 0;
    public int buttons_num=-1,right=0,need=10;
    public Animator radar_anim,cristals_anim;
    public GameObject end_trigger,crist_trigger;
    public sound sound;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        missions(1);
        buttons[buttons_rand].color = Color.red;
        buttons_obj.SetActive(false);
    }

    private void Awake() {
        end_trigger.SetActive(false);
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

        controls.buttons.interact.performed += ctx => interact=true;
        controls.buttons.interact.canceled += ctx => interact=false;
        controls.buttons.interact2.performed += ctx => interact2=true;
        controls.buttons.interact2.canceled += ctx => interact2=false;
        controls.buttons.interact3.performed += ctx => interact3=true;
        controls.buttons.interact3.canceled += ctx => interact3=false;

        controls.buttons.up.performed += ctx =>  debuging_mission(0);
        controls.buttons.left.performed += ctx =>  debuging_mission(2);
        controls.buttons.right.performed += ctx =>  debuging_mission(1);
        controls.buttons.down.performed += ctx =>  debuging_mission(3);
        controls.buttons.exit.performed += ctx =>  Application.Quit();
        healthcheck();
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
        for(int i=0;i<Distance/7;i++){
            conect.text+="|";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(mine){
            Fspeed=0f;Rspeed=0f;
            cristals_anim.ResetTrigger("start");
            cristals_anim.SetTrigger("start");
            if(interact2){
                cristals_anim.SetBool("mining",true);
                mined=true;
            }
            else if(!interact2){
                cristals_anim.SetBool("mining",false);
            }
            if(mined){
                if(interact3){
                    cristals_anim.SetBool("grab",true);
                }
                else if(!interact3){
                    cristals_anim.SetBool("grab",false);
                }
            }
        }
        if (!crist_trigger.activeSelf&&mined){
            mine=false;
            deb_text.text = "Great now got to beacon to end mission {go to the red plus on the map}";
            Fspeed=0.06f;Rspeed=0.8f;
            mined=false;
        }

        if(right==need){
            right=0;
            buttons_obj.SetActive(false);
            StartCoroutine("end_radar");
        }
        //stats, death
        speedtext.text="";
        for(int i=0;i<speed*250;i++){
            speedtext.text+="/";
        }
        if (power > 0)
        {
            powertext.text="";
            power -= Time.deltaTime;
            power -= powermult;
            for(int i=0;i<power/60;i++){
                powertext.text+="[]";
            }
        }
        else if (power<=0&&!end){
            sound.clip="dead";
            StartCoroutine(death("No power"));
        }
        dist();
        if(health<=0&&!end){
            sound.clip="dead";
            StartCoroutine(death("Hit"));
        }
        if(Distance>170&&!end){
            sound.clip="dead";
            StartCoroutine(death("No conection"));
        }

        if(count>0){
            count -= Time.deltaTime;
        }
        if(fw||bw||lt||rt){
            sound.riding = true;
        }
        else if(!fw||!bw||!lt||!rt){
            sound.riding = false;
        }

        //moving
        if(fw){
            sound.riding = true;
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
        if(owerload>20){
            deb_text.text = "\n Warning owerload";
            shift=false;
        }
        else if(owerload<20){
            shift=true;
        }
        if (fw&&sh&&shift){
            string textb =deb_text.text;
            powermult=0.1f;
            speed=Fspeed+Fspeed*0.65f;
            deb_text.text = textb;
        }
        else{
            speed=Fspeed;
            powermult=0;
        }
        if(sh){
            owerload += Time.deltaTime;
        }
        if(!fw){
            owerload=0;
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

    void missions(int m){
        switch(m){
            case 1:
                deb_text.text = "Radar having some problems go to the radar for debug (red circle on your map)";
                break;
            case 2: 
                deb_text.text = "Good job now we need to collect moon cristals {green cross on your map}";
                break;
        }
    }

    void debuging_mission(int buttons_num){
        if(buttons_num==buttons_rand){
            buttons[buttons_rand].color = Color.white;
            buttons_rand=UnityEngine.Random.Range(0, 4);
            buttons[buttons_rand].color = Color.red;
            right+=1;
            sound.clip="click";
        }
    }

    bool radar_comp=false;
    IEnumerator end_radar(){
        radar_comp=true;
        deb_text.text+= "\n Radar debuging complete";
        Fspeed=0.06f;Rspeed=0.8f;
        yield return new WaitForSeconds(2);
        missions(2);
        radar_anim.ResetTrigger("fixing");
        radar_anim.SetTrigger("fixing");
        sound.clip="radar";
    }
    
    IEnumerator start_radar(){
        deb_text.text+= "\n Conecting \n";
        for(int i=0;i<4;i++){
            deb_text.text+= ".";
            yield return new WaitForSeconds(2);
        }
        yield return new WaitForSeconds(1);
        deb_text.text+= "\n Conected \n {push button on [D-pad/arrow-keys] in same order as on screen }";
        buttons_obj.SetActive(true);
        Fspeed=0;Rspeed=0;
    }

    IEnumerator the_end(){
        end=true;
        Debug.Log("end");
        deb_text.text ="Congrats you complited all missions {thanks fo playing ^w^}";
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("end");
    }

    IEnumerator death(string type){
        deb_text.text = "Mission failed \n Conection lost \n Reason "+type;
        Fspeed=0f;Rspeed=0;
        yield return new WaitForSeconds(3);
        cam.enabled = false;
        deb_text.text = "Please wait 5 seconds to restart";
        yield return new WaitForSeconds(5);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

    }

    private void OnEnable() {
        controls.player.forward.Enable();
        controls.player.left.Enable();
        controls.player.right.Enable();
        controls.player.backward.Enable();
        controls.player.shift.Enable();

        controls.buttons.left.Enable();
        controls.buttons.right.Enable();
        controls.buttons.up.Enable();
        controls.buttons.down.Enable();
        controls.buttons.interact.Enable();
        controls.buttons.interact2.Enable();
        controls.buttons.interact3.Enable();
        controls.buttons.exit.Enable();
    }

    private void OnDisable() {
        controls.player.forward.Disable();
        controls.player.left.Disable();
        controls.player.right.Disable();
        controls.player.backward.Disable();
        controls.player.shift.Disable();

        controls.buttons.left.Disable();
        controls.buttons.right.Disable();
        controls.buttons.up.Disable();
        controls.buttons.down.Disable();
        controls.buttons.interact.Disable();
        controls.buttons.interact2.Disable();
        controls.buttons.interact3.Disable();
        controls.buttons.exit.Disable();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "radar"){
            StartCoroutine("start_radar");
        }
        if(other.gameObject.tag =="end"){
            StartCoroutine("the_end");
        }
    }
    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "cristals"&&radar_comp&&!mine){
            deb_text.text = "Press [A/E] button to start mining \n To grab the cristal hold [Right Trigger/Right Mouse Button] \n To mine the ground press [Left Trigger/Left Mouse Button]";
            // StartCoroutine("crist_mine");
            if(interact){
                mine=true;
            }
        }
        else if(other.gameObject.tag == "cristals"&&!radar_comp){
            deb_text.text="You need to fix radar {go to the red circle on your map}";
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "radar"&&radar_comp==false){
            buttons_obj.SetActive(false);
            StopCoroutine("start_radar");
            deb_text.text+= "\n Conection failed";
        }
    }
    
    private IEnumerator OnCollisionEnter(Collision collision)
    {
        float r=Rspeed,s=Fspeed;
        if(collision.gameObject.tag=="border"&&count<=0){
            sound.clip="rock";
            count=wakespeed;shift=false;
            Rspeed=Rspeed/2;Fspeed=Fspeed/2;
            // sh=false;shift=false;
            // float r=Rspeed, s=speed;
            // float r2=r/2, s2=s/2;
            // if ((Fspeed-s2)>0&&s<=Fspeed){
            //     Fspeed-=s2;Rspeed-=r2;
            //     yield return new WaitForSeconds(4);
            //     Fspeed+=s2;Rspeed+=r2;
            health-=1;
            healthcheck();
            yield return new WaitForSeconds(wakespeed);
            // }
            // else if((Fspeed-s2)<0&&s>=Fspeed){
            //     Fspeed=0;Rspeed=0;
            //     yield return new WaitForSeconds(4);
            //     Fspeed=f;Rspeed=r;
            // }
            Rspeed=Rspeed*2;Fspeed=Fspeed*2;
            shift=true;
            // shift=true;
        }
    }
}