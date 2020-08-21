using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    private bool shootDir;
    private float x = 75;

    public void Setup(bool dir){
        this.shootDir = dir;
        Destroy(gameObject, 0.7f);
        transform.position += (new Vector3(1f, 0, 0));
    }

    private void Update(){

        transform.position += (new Vector3(20f, -1f, 0) * Time.deltaTime);
        // decrease z over time
        x -= Time.deltaTime * 300;
        transform.rotation = Quaternion.Euler(0, 0, x);
    }
}
