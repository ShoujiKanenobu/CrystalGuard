using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadContainer : MonoBehaviour
{
    public GameObject point1;
    public GameObject point2;
    public float offset;
    public float width;
    // Start is called before the first frame update
    void Update()
    {
        Vector3 midPoint = (point1.transform.position + point2.transform.position) / 2f;

        transform.position = midPoint;

        Vector3 dir = point2.transform.position - point1.transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);


        float distance = Vector3.Distance(point1.transform.position, point2.transform.position);

        transform.GetChild(0).localScale = new Vector3(distance - offset, width, 1f);


        
    }
}
