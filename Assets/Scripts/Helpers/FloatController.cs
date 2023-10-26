using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatController : MonoBehaviour
{
    [SerializeField]
    private float travelDistance;
    [SerializeField]
    private float speed;

    private Vector3 originPoint;
    private Vector3 nextPos;

    private float seed = 0;
    private void Start()
    {
        seed = Random.Range(0, 3);
        originPoint = this.transform.position;
    }

    void Update()
    {
        nextPos = originPoint;
        nextPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * speed + seed) * travelDistance;
        this.transform.position = nextPos;
    }
}
