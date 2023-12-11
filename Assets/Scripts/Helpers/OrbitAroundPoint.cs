using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAroundPoint : MonoBehaviour
{
    public Vector3 point;
    public float radius;
    public float speed;

    private float rot;

    private void Awake()
    {
        rot = Random.Range(0, 12);
    }

    void Update()
    {
        rot += Time.fixedDeltaTime * speed;
        transform.localPosition = new Vector3(Mathf.Cos(rot) * radius + point.x, Mathf.Sin(rot) * radius + point.y, point.z);
    }
}
