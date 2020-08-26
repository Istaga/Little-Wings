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
    public bool horiz;
    public bool pos;
    
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

    // move in 1-tile increments until "obs" hit, then turn around

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "egg"){
            Destroy(gameObject);
        }
    }




    // Helper FNs


    private bool checkMove(Vector3 A, bool vert, bool pos){
        // (A, B) fire raycast
        float x = 0;
        float y = 0;
        float rad = 4f;
        if(vert){
            if(pos){
                y = rad;
            }
            else{
                y = -rad;
            }
        }
        else {
            if(pos){
                x = rad;
            }
            else{
                x = -rad;
            }
        }
        // check if raycast collided with tag == "obs"
        // A is transform.position
        // B is transform.position, direction, dist, where dist is fixed
        Vector3 C = new Vector3(transform.position.x + A.x, transform.position.y + A.y, transform.position.z + A.z);
        Vector3 B = new Vector3(C.x + x, C.y + A.y + y, C.z);
        RaycastHit2D hit = Physics2D.Raycast(C, C-B, 5f);
        Debug.DrawLine(C, B, Color.blue);
        Debug.Log("We hit " + hit.collider.name);
        if(hit.collider.tag == "obs") return false;
        return true;
    }
}
