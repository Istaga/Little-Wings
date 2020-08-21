using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    private bool fwd;
    private float x = 75;
    private float time = 0f;

    public void Setup(bool dir){
        this.fwd = dir;
        Destroy(gameObject, 0.7f);
    }

    private void Update(){
        // rotate faster at the start, it looks more natural
        if(time < 0.1f){
            x-= Time.deltaTime * 600;
            transform.position += (new Vector3(20f, 3f, 0) * Time.deltaTime);
        }
        else if(time < 0.3){
            x-= Time.deltaTime * 400;
            transform.position += (new Vector3(20f, 2f, 0) * Time.deltaTime);
        }
        else {
            x-= Time.deltaTime * 300;
            transform.position += (new Vector3(20f, -1f, 0) * Time.deltaTime);
        }

        transform.rotation = Quaternion.Euler(0, 0, x);
        time += Time.deltaTime;
    }
}
