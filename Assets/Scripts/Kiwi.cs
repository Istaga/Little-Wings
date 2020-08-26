using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiwi : MonoBehaviour
{
    [SerializeField] private Transform Egg;

    private Rigidbody2D rb;
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
    private static readonly float COOLDOWN = 0.5f;
    private static readonly float ACTION_COOLDOWN = 1f;
    private float invisFade = 0f;
    private bool isCoolingDown = false;
    private bool facingForward = true;
    private bool grounded;
    private bool canMove;

    private void Awake(){
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Start(){
        grounded = true;
        canMove = true;
        anim = GetComponent<Animator>();
        GLOBAL_TIME = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        state = anim.GetCurrentAnimatorStateInfo(0);

        if( !canMove ){
            return;
        }


        //GLOBAL_TIME += Time.deltaTime;
        // if ( GLOBAL_TIME >= ACTION_COOLDOWN ){
        //     //return;
        // }

        if( isCoolingDown || state.nameHash != stillStateHash ){
            Debug.Log(state.nameHash);
            return;
        }

        if( grounded ){
            checkHazards();
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

        if (Mathf.Abs(vert) > 0){
            bool up = (Mathf.Sign(vert) == 1) ? true : false;
            Debug.Log("up is " + up);
            Vector3 target = new Vector3(0, Mathf.Sign(vert) *  2.6f, 0);
            if(checkMove(target, true, up)){
                StartCoroutine(Move(target));
            }
        }
        else if (Mathf.Abs(horiz) > 0){
            if(mySpriteRenderer != null){
                Vector3 target = new Vector3(Mathf.Sign(horiz) * 3.5f, 0, 0);

                // TODO : IF FACING LEFT THEN TURN RIGHT INSTEAD OF MOVING RIGHT

                if(horiz < 0){
                    if(!mySpriteRenderer.flipX){
                        mySpriteRenderer.flipX = true;
                    }
                    else {
                        if(checkMove(target, false, false)){
                            StartCoroutine(Move(target));
                        }
                    }
                }
                else {
                    mySpriteRenderer.flipX = false;
                    if(checkMove(target, false, true)){
                            StartCoroutine(Move(target));
                    }
                }
            }
        }
    }

    private void EggToss(){
        canMove = false;
        // calculate kiwi versus egg position
        Vector3 kiwiPos = kiwiSprite.transform.position;
        Vector3 beakPos = kiwiPos + new Vector3(2.53f, 0.5f, 0);
        Transform eggTransform = Instantiate(Egg, beakPos, Quaternion.Euler(0, 0, 75));
        eggTransform.GetComponent<Egg>().Setup(mySpriteRenderer.flipX);
        canMove = true;
    }


    // TODO: Add slight rotation during jump
    public IEnumerator Jump(){
        canMove = false;
        isCoolingDown = true;
        grounded = false;
        anim.SetTrigger(jumpHash);

        Vector3 v = new Vector3(7f, 0, 0);
        Vector3 h = new Vector3(4f, 2.4f, 0);
        Vector3 down = new Vector3(3f, -2.4f, 0);
        if( mySpriteRenderer.flipX == true ){
            v = new Vector3(-7f, 0, 0);
            h = new Vector3(-4f, 2.4f, 0);
            down = new Vector3(-3f, -2.4f, 0);
        }

        // Rotation occurs in the z component of transform.rotation

        // First determine the halfway point
        // Note: We're always jumping forward
        // lerp to halfway, lerp to end

        var current = kiwiSprite.transform.position;
        var start = current;
        var halfpoint = current + h;
        var time = 0f;

        // Adding a slight delay before movement, since the kiwi crouches before "flying"
        // The jump animation is 13 frames, the first/last 4 are spent standing still crouching

        // We want time to pass faster while crouching, slowing while jumping
        //anim.speed = 25f; // Crouch fast
        // We want to play
        anim.speed = 10f;
        while( time < 0.05f ){
            time = time + Time.deltaTime;
            yield return null;
        }

        time = 0f;

        anim.speed = 0.15f; // Fly slower


        while( time < 1f ){
            transform.position = Vector3.Lerp(current, halfpoint, time);
            time = time + Time.deltaTime / COOLDOWN;

            // change anim speed to be slower at the top and ending half
            if (time >= 0.3f && time < 0.33f){
                anim.speed = 0.1f;
            }
            if (time >= 0.7f && time < 0.72f){
                anim.speed = 0.1f;
            }
            

            yield return null;
        }

        //current = kiwiSprite.transform.position;
        current = halfpoint;
        time = 0f;
        Vector3 end = halfpoint + down;

        anim.speed = 0.3f; // Land faster

        while( time < 1f ){
            transform.position = Vector3.Lerp(current, end, time);
            time = time + Time.deltaTime / COOLDOWN;
            if (time >= 0.5f && time <= 0.55f){
                anim.speed = 1.5f;
            }
            yield return null;
        }
        transform.position = end; // Ensures consistent movement
        anim.speed = 1f; // Reset to normal
        grounded = true;
        canMove = true;
        isCoolingDown = false;
    }

    private IEnumerator Move(Vector3 v){
        canMove = false;
        isCoolingDown = true;
        anim.SetTrigger(walkHash);
        var start = kiwiSprite.transform.position;
        var end = start + v;
        var time = 0f;
        anim.speed = 2.5f;

        while(time < 1f){
            transform.position = Vector3.Lerp(start, end, time);
            time = time + Time.deltaTime / COOLDOWN;
            if(time >= 0.1f){
                anim.speed = 1f;
            }
            yield return null;
        }

        transform.position = end;
        canMove = true;
        isCoolingDown = false;
    }


    void OnTriggerEnter2D(Collider2D other){
        if( other.tag == "enemy" | (grounded && other.tag == "hole") ){
            canMove = false;
            anim.SetTrigger(deathHash);
            RestInPieces();
        }
        else if( other.tag == "Finish" ){
            //da
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if( other.tag == "enemy" | (grounded && other.tag == "hole") ){
            canMove = false;
            anim.SetTrigger(deathHash);
            RestInPieces();
        }
        else if( other.tag == "camo" ){
            float time = Time.deltaTime / 2f;
            invisFade += time;
            if( invisFade <= 0.5f ){
                lowerAlpha(time);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if( other.tag == "camo" ){
            restoreAlpha();
        }
    }

    private IEnumerator RestInPieces(){
        Destroy(gameObject);
        yield return null;
    }












    // HELPER FUNCTIONS


    private bool checkMove(Vector3 A, bool vert, bool pos){
        // (A, B) fire raycast
        float x = 0;
        float y = 0;
        if(vert){
            if(pos){
                y = 5f;
            }
            else{
                y = -5f;
            }
        }
        else {
            if(pos){
                x = 5f;
            }
            else{
                x = -5f;
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

    private void checkHazards(){
        //Debug.Log("entered check hazards");
    }

    private void lowerAlpha(float time){
        Color tmp = mySpriteRenderer.color;
        tmp.a -= time;
        mySpriteRenderer.color = tmp;
    }

    private void restoreAlpha(){
        Color tmp = mySpriteRenderer.color;
        tmp.a = 1f;
        mySpriteRenderer.color = tmp;
        invisFade = 0f;
    }
}