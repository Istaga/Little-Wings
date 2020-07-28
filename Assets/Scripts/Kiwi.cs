
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiwi : MonoBehaviour
{
    public SpriteRenderer mySpriteRenderer;
    public GameObject kiwiSprite;
    public Animator anim;

    int walkHash = Animator.StringToHash("walk");

    private static readonly float COOLDOWN = 0.6f;
    private bool isCoolingDown = false;
    private bool facingForward = true;

    private void Awake(){
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start(){
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if( isCoolingDown ){
            return;
        }

        var horiz = Input.GetAxis("Horizontal");
        var vert = Input.GetAxis("Vertical");

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

    private IEnumerator Move(Vector3 v){
        isCoolingDown = true;
        anim.SetTrigger(walkHash);

        var start = transform.position;
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
