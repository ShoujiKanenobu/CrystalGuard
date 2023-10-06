using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCoordinatePrinter : MonoBehaviour
{
    private int count;

    private void Start()
    {
        count = 0;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log(count + ": " + MapManager.instance.MousePositionGrid);
            count++;
        }
    }
}
