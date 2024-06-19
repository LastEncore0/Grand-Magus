using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public GameObject prefab;
    private Queue<GameObject> pool = new Queue<GameObject>();

    public ObjectPool(GameObject prefab)
    {
        this.prefab = prefab;
    }
    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            var obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return Instantiate(prefab);
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
