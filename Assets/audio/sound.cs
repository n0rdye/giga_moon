using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sound : MonoBehaviour
{
    public AudioSource[] source;
    public AudioClip[] audio;
    public string clip="";
    public bool riding=false;
    // Start is called before the first frame update
    void Start()
    {
        source[1].clip=audio[3];
        source[1].Play();
    }

    // Update is called once per frame
    void Update()
    {        
        if(riding){
            source[2].gameObject.SetActive(true);
        }
        else if(!riding){
            source[2].gameObject.SetActive(false);
        }

        if(clip=="click"){
            StartCoroutine("ButtonClick");
        }
        else if(clip=="rock"){
            StartCoroutine("rock");
        }
        else if(clip=="radar"){
            radar();
        }
        else if(clip=="dead"){
            StartCoroutine("dead");
        }
    }


    IEnumerator ButtonClick()
    {
        clip="";
        source[0].clip=audio[1];
        source[0].Play();
        yield return new WaitForSeconds(3);
        source[0].Stop();
    }

    IEnumerator rock(){
        clip="";
        source[0].clip=audio[0];
        source[0].Play();
        yield return new WaitForSeconds(3);
        source[0].Stop();
    }

    IEnumerator dead(){
        clip="";
        source[0].clip=audio[4];
        source[0].Play();
        yield return new WaitForSeconds(2);
        source[0].Stop();
    }

    void radar(){
        source[1].clip=audio[2];
        source[1].Play();
        clip="";
    }
}
