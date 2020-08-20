using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    private float dist = 0;
    private Vector3 shootDir;
    public void Setup(Vector3 shootDir){
        this.shootDir = shootDir;
        Debug.Log("egg class pos is " + transform.position);
    }

    private void Update(){
        float ms = 10f;
        //transform.position += shootDir * ms * Time.deltaTime;
    }
}
