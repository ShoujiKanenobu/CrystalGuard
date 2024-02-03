using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> toScale;

    [SerializeField]
    private float scaleSpeed;

    public void Update()
    {
        if (toScale[0].localScale.x == 1)
            return;

        foreach(Transform t in toScale)
        {
            t.localScale = Vector3.MoveTowards(t.localScale, Vector3.one, scaleSpeed);
        }
    }

    public void StartScaling()
    {
        foreach(Transform t in toScale)
        {
            t.localScale = Vector3.zero;
        }
    }
}
