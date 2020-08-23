using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stoat : MonoBehaviour
{
    bool piwiAlive = true;
    bool isCoolingDown = false;
    private bool angry = false;
    private float COOLDOWN = 0.5f;
    int sprintHash = Animator.StringToHash("angry");

    public Kiwi piwi;
    AnimatorStateInfo state;
    public Animator anim;
    public SpriteRenderer mySpriteRenderer;
    
    private void Awake(){
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isCoolingDown){
            return;
        }
        if(!angry){
            CheckForPiwi();
        }

        state = anim.GetCurrentAnimatorStateInfo(0);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "egg"){
            Destroy(gameObject);
        }
        else if(other.tag == "kiwi"){
            piwiAlive = false;
            anim.SetBool("angry", false);
            Debug.Log("should be not angry in anim");
            angry = false;
        }
    }

    private void CheckForPiwi(){
        float piwiX = piwi.transform.position.x;
        float piwiY = piwi.transform.position.y;
        float x = transform.position.x;
        float y = transform.position.y;

        // check horizontal dist
        if (x - piwiX < 18f){
            // check kiwi above stoat
            if(piwiY > y){
                if(Mathf.Abs(piwiY) - Mathf.Abs(y) < 2.7f){
                    StartCoroutine(Charge());
                }
            }
            // check kiwi below stoat
            else if(y > piwiY) {
                if(Mathf.Abs(y - piwiY) < 2f){
                    StartCoroutine(Charge());
                }
            }
        }
        //piwiY - transform.position.y < 2.71 | y - (-piwiY) < 2.49
    }

    private IEnumerator Charge(){
        anim.SetBool("angry", true);
        angry = true;
        float piwiX = piwi.transform.position.x;
        Vector3 target = new Vector3(piwi.transform.position.x, transform.position.y, transform.position.z);

        var start = transform.position;
        var end = target;
        var time = 0f;
        float mul = 7f; // changes speed
        float animMul = 7f;
        anim.SetFloat("speedMul", animMul);
        anim.speed = 1f;

        Debug.Log("start pos is " + start);
        Debug.Log("end pos is " + end);

        while(time < 1f){
            transform.position = Vector3.Lerp(start, end, time);
            Debug.Log(time);

            if (time < 0.02f){
                time += Time.deltaTime / mul;
                Debug.Log("Stage ranapden");
                mul -= 0.03f;
                animMul = 2f;
            }
            else if (time >= 0.02f && time < 0.1f){
                animMul = 0.5f;
                time += Time.deltaTime / mul;
                Debug.Log("Stage -1");
                mul -= 0.01f;
            }
            else if (time >= 0.1f && time < 0.3f){
                animMul = 0.6f;
                time += Time.deltaTime / mul;
                Debug.Log("Stage 0");
                mul -= 0.022f;
            }
            else if (time >= 0.3f && time < 0.33f){
                animMul = 0.65f;
                time += Time.deltaTime / mul;
                Debug.Log("Stage 1");
                mul -= 0.033f;
            }
            else if (time >= 0.33f && time < 0.7f){
                animMul = 0.75f;
                time += Time.deltaTime / mul;
                Debug.Log("Stage 2");
                mul -= 0.022f;
            }
            else if (time >= 0.7f && time < 0.72f){
                animMul = 0.9f;
                time += Time.deltaTime / mul;
                Debug.Log("Stage 3");
                mul -= 0.005f;
            }
            else if (time >= 0.72f) {
                animMul = 1.2f;
                time += Time.deltaTime / mul;
                Debug.Log("Stage 4");
            }
            anim.SetFloat("speedMul", animMul);
            if(!angry){
                yield break;
            }
            yield return null;
        }
        transform.position = end;
        anim.SetBool("angry", false);
        anim.SetBool("coolingOff", true);
        mySpriteRenderer.flipX = true;
        // walk back slowly
        time = 0f;
        mul = 3f;
        while(time < 1f){
            transform.position = Vector3.Lerp(end, start, time);
            
            if(time > 0.8f){
                time += Time.deltaTime / mul;
            }
            else {
                time += Time.deltaTime / (mul/2f);
            }
            yield return null;
        }
        anim.SetBool("coolingOff", false);
        mySpriteRenderer.flipX = false;
        angry = false;
        isCoolingDown = false;
    }

}
