using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float timeToDestroy;
    private float EndTime;
    void Start()
    {
        EndTime = Time.time + timeToDestroy;
    }

    void Update()
    {
        if (Time.time > EndTime)
            Destroy(this.gameObject);
    }
}
