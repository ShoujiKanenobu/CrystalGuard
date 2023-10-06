using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAtMouseGridCenter : MonoBehaviour
{
    public Vector3 offset = new Vector3(0.5f, 0.5f, 0);
    private void OnEnable()
    {
        this.transform.position = MapManager.instance.MousePositionGrid + offset;
    }
}
