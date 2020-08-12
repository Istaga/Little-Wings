
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiwi : MonoBehaviour
{
    public SpriteRenderer mySpriteRenderer;
    public GameObject kiwiSprite;
    public Animator anim;
    AnimatorStateInfo state;

    int walkHash = Animator.StringToHash("Walk");
    int eggHash = Animator.StringToHash("Egg");
    int jumpHash = Animator.StringToHash("Jump");
    int deathHash = Animator.StringToHash("Dead");
    int stillStateHash = Animator.StringToHash("Base Layer.Still");

    private static float GLOBAL_TIME;
    private static readonly float COOLDOWN = 0.6f;
    private static readonly float ACTION_COOLDOWN = 1f;
    private bool isCoolingDown = false;
    private bool facingForward = true;

    private void Awake(){
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start(){
        anim = GetComponent<Animator>();
        GLOBAL_TIME = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        state = anim.GetCurrentAnimatorStateInfo(0);
        GLOBAL_TIME += Time.deltaTime;
        if ( GLOBAL_TIME >= ACTION_COOLDOWN ){
            //return;
        }

        if( isCoolingDown || state.nameHash != stillStateHash ){
            return;
        }

        if( Input.GetKeyUp("x") ){
            anim.SetTrigger(eggHash);
            return;
        }

        var horiz = Input.GetAxis("Horizontal");
        var vert = Input.GetAxis("Vertical");

        if( facingForward && Input.GetKeyUp("j") ){
            StartCoroutine(Jump());
            return;
        }

        if( Input.GetKeyUp("b") ){
            anim.SetTrigger(deathHash);
            return;
        }

        if (Mathf.Abs(vert) > 0){
            if(vert > 0){
                //frogSprite.transform.rotation = Quaternion.identity;
            }
            else {
                //frogSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Sign(vert) * 180));
            }
            StartCoroutine(Move(new Vector3(0, Mathf.Sign(vert) *  2.6f, 0)));
        }
        else if (Mathf.Abs(horiz) > 0){
            //frogSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Sign(horiz) * 180));
            if(mySpriteRenderer != null){
                if(horiz < 0){
                    mySpriteRenderer.flipX = true;
                }
                else {
                    mySpriteRenderer.flipX = false;
                }
            }

            StartCoroutine(Move(new Vector3(Mathf.Sign(horiz) * 3.5f, 0, 0)));
        }
    }

    private IEnumerator Jump(){

        isCoolingDown = true;
        anim.SetTrigger(jumpHash);

        Vector3 v = new Vector3(7f, 0, 0);
        Vector3 h = new Vector3(3.5f, 3.5f, 0);
        Vector3 down = new Vector3(3.5f, -3.5f, 0);

        // Rotation occurs in the z component of transform.rotation

        // First determine the halfway point
        // Note: We're always jumping forward
        // lerp to halfway, lerp to end

        var current = kiwiSprite.transform.position;
        var halfpoint = current + h;
        var time = 0f;

        while( time < 1f ){
            transform.position = Vector3.Lerp(current, halfpoint, time);
            time = time + Time.deltaTime / COOLDOWN;
            yield return null;
        }

        current = kiwiSprite.transform.position;
        time = 0f;
        Vector3 end = halfpoint + down;

        while( time < 1f ){
            transform.position = Vector3.Lerp(current, end, time);
            time = time + Time.deltaTime / COOLDOWN;
            yield return null;
        }
        isCoolingDown = false;
    }

    private IEnumerator Move(Vector3 v){
        isCoolingDown = true;
        anim.SetTrigger(walkHash);

        var start = kiwiSprite.transform.position;
        var end = start + v;
        var time = 0f;
        while(time < 1f){
            transform.position = Vector3.Lerp(start, end, time);
            time = time + Time.deltaTime / COOLDOWN;
            yield return null;
        }
        transform.position = end;
        isCoolingDown = false;
    }
}
