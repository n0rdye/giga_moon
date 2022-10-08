using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
    public Rigidbody rb;
    public float grav = 3,speed =0.02f,Fspeed =0.02f,Rspeed =0.1f,f=0.02f;
    public bool j=true,shift=true,g=true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey (KeyCode.A) && j){
            this.transform.Rotate (0f, -Rspeed, 0f);
        }
        if (Input.GetKey (KeyCode.W) && j){
            transform.Translate (0f, 0f, speed);
        }
        if (Input.GetKey (KeyCode.D) && j){
            this.transform.Rotate (0f, Rspeed, 0f);
        }
        if (Input.GetKey (KeyCode.S) && j){
            transform.Translate (0f, 0f, -speed);
        }
        if(Input.GetKey (KeyCode.W) && Input.GetKeyDown (KeyCode.Space) && j && g){
            rb.AddForce(transform.forward * grav);
            rb.AddForce(transform.up * grav);
            j=false;g=false;
        }
        else if (Input.GetKeyDown (KeyCode.Space) && j && g){
            rb.AddForce(transform.up * grav);
            j=false;g=false;
        }
        if (Input.GetKey (KeyCode.LeftShift) && j&&shift){
            speed=Fspeed+0.02f;
        }
        else{
            speed=Fspeed;
        }
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