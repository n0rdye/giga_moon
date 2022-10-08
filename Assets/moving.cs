using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class moving : MonoBehaviour
{
    public Rigidbody rb;
    public float grav = 3,speed =0.02f,Fspeed =0.02f,Rspeed =0.1f,f=0.02f;
    public bool j=true,shift=true,g=true;
    public PlayerControls controls;
    public bool fw=false,bw=false,rt=false,lt=false,sh=false;
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

    // Update is called once per frame
    void Update()
    {
        // float x = Input.GetAxis("Horizontal");
        // float z = Input.GetAxis("Vertical");
 
        // Vector3 Move = transform.right * x + transform.forward * z;
 
        // controller.Move(Move * speed * Time.deltaTime);
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

        if (sh && j){
            speed=Fspeed+0.02f;
        }
        else{
            speed=Fspeed;
        }

        //jump
        if(Input.GetKey (KeyCode.W) && Input.GetKeyDown (KeyCode.Space) && j && g){
            rb.AddForce(transform.forward * grav);
            rb.AddForce(transform.up * grav);
            j=false;g=false;
        } else
        if (Input.GetKeyDown (KeyCode.Space) && j && g){
            rb.AddForce(transform.up * grav);
            j=false;g=false;
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
        float r=Rspeed;float s=speed;
        float r2=r/2;float s2=s/2;
        if (collision.gameObject.tag == "ground"&& (Fspeed-s2)>0&&s<=Fspeed){
            Fspeed-=s2;Rspeed-=r2;
            shift=false;g=false;j=true;
            yield return new WaitForSeconds(3);
            Fspeed+=s2;Rspeed+=r2;
            g=true;shift=true;
        }
        else{
            Fspeed=0;Rspeed=0;
            shift=false;g=false;j=true;
            yield return new WaitForSeconds(3);
            Fspeed+=f;Rspeed+=r;
            g=true;shift=true;
        }
    }
}