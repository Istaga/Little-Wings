using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    private float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time % 1 >= 0.5){
            // increase
            transform.localScale += new Vector3(0.00035f, 0.00035f, 0);
        }
        else {
            // decrease
            transform.localScale -= new Vector3(0.00035f, 0.00035f, 0);
        }


        time += Time.deltaTime;
    }
}
