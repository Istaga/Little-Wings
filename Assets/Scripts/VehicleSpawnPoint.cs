using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VehicleSpawnPoint : MonoBehaviour
{
    protected List<IAction> Actions { get; } = new List<IAction>();

    public GameObject[] pool;
    public bool running = true;

    [HideInInspector]
    public int _poolIndex = 0;

    IEnumerator Start(){
        var index = 0;
        while(running){
            var action = Actions[index];
            yield return action.Execute();
            index++;
            index %= Actions.Count;
        }
    }
}
