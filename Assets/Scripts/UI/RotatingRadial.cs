using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingRadial : MonoBehaviour
{
    public RectTransform rt;
    public float rotationSpeed;
    void Update()
    {
        rt.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }
}
