using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMyPool : MonoBehaviour
{
    public MyPool pool;
    private float _lastAppear;
    private void Update() {
        
        if(Time.time - 0.1 >= _lastAppear){
            gameObject.SetActive(false);
        }
    }

    public void OnDisable()
    {
        pool.AddToPool(gameObject);
    }
    private void OnEnable() {
        _lastAppear = Time.time;
    }
}
