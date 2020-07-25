using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowControl : MonoBehaviour
{
    private float startPos;
    private int firstCar = 0;
    private int trainLength;
    private bool traffic = true;

    public GameObject[] train;
    public float speed;
    
    void Start(){
        trainLength = train.Length;
        Debug.Log(trainLength);
        StartCoroutine(Move());
    }

    void Update(){
        if(train[firstCar].transform.position.x <= -128){
            int lastCar;
            if(firstCar == 0){
                lastCar = 5;
            }
            else {
                lastCar = firstCar - 1;
            }
            Debug.Log("lastCar is train[" + lastCar + "]");
            Debug.Log("OOB detected, shifting first car to position " + (train[lastCar].transform.position.x + 45));
            Debug.Log("Position of last car (" + (lastCar) + ") is " + train[lastCar].transform.position.x);
            train[0].transform.position += new Vector3(train[lastCar].transform.position.x + 68, 0, 0);
            if(firstCar + 1 == trainLength){
                firstCar = 0;
            }
            else {
                firstCar++;
            }
        }
    }

    private IEnumerator Move(){
        while(traffic){
            foreach(var car in train){
                car.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }
            //train[i].transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            yield return null;
        }
    }
}
