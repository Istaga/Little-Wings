using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snek : MonoBehaviour
{
    bool isCoolingDown = false;
    private float COOLDOWN = 0.4f;

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
        SneakDiss(new Vector3(33f,33f,0));
        state = anim.GetCurrentAnimatorStateInfo(0);
    }

    private IEnumerator SneakDiss(Vector3 v){
        isCoolingDown = true;
        //anim.SetTrigger(walkHash);

        var start = transform.position;
        var end = start + v;
        var time = 0f;

        while(time < 1f){
            transform.position = Vector3.Lerp(start, end, time);
            time = time + Time.deltaTime / COOLDOWN;
            yield return null;
        }

        // there's a slight delay before the walk starts, maybe we can speed it up very briefly?
        // up to t=0.1, then normal speed?
        // seems to be ok now

        transform.position = end;
        isCoolingDown = false;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "egg"){
            Destroy(gameObject);
        }
    }
}
