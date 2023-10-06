using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> pool;
    public GameObject pooledObject;
    public int initialPoolSize;
    private int poolSize;

    private void Start()
    {
        pool = new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < initialPoolSize; i++)
        {
            temp = Instantiate(pooledObject);
            temp.SetActive(false);
            pool.Add(temp);
        }

        poolSize = initialPoolSize;
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i < poolSize; i++)
        {
            if (!pool[i].activeInHierarchy)
                return pool[i];
        }
        return null;
    }
    
    public void ExpandPool(GameObject newObject)
    {
        //I'm not checking if the newly pooled object is what this pool is for
        if (newObject == null)
            return;

        poolSize++;
        pool.Add(newObject);
    }
}
