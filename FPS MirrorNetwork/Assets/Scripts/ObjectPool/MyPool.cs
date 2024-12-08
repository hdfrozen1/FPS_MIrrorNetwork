using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPool 
{
    private static MyPool _myPool;
    public static MyPool Instance
    {
        get
        {
            if(_myPool != null) {return _myPool;}
            else{
                return new MyPool();
            }
        }
    }
    private static Stack<GameObject> stack = new Stack<GameObject>();
    private GameObject tmp;
    private ReturnToMyPool returnPool;


    public GameObject Get(GameObject gObject,Transform position)
    {
        while (stack.Count > 0)
        {
            tmp = stack.Pop();
            if (tmp != null)
            {
                tmp.SetActive(true);
                return tmp;
            } else
            {
                //Debug.LogWarning($"game object with key {baseObj.name} has been destroyed!");
            }
        }
        tmp = GameObject.Instantiate(gObject,position.position,Quaternion.identity);
        returnPool = tmp.AddComponent<ReturnToMyPool>();
        returnPool.pool = this;
        return tmp;
    }

    public void AddToPool(GameObject obj)
    {
        stack.Push(obj);
    }
}
