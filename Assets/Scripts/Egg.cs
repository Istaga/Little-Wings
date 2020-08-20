using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    private float timeAlive = 0f;
    private boolean forward;
    private Vector3 shootDir;
    private float ms = 10f;

    public void Setup(boolean dir){
        this.forward = dir
    }

    private void Update(){
        timeFlying += Time.deltaTime;
        if( timeFlying > 3f ){
            Destroy(this);
        }

        Vector3 pew = (shootDir * ms * Time.deltaTime);
        transform.position += pew;
        Debug.Log("additional vel is " + pew);
    }
}
