using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPos : MonoBehaviour
{
    void Update()
    {
        Debug.Log("GameObject:" + this.gameObject.name + "Pos:" 
            + transform.position.x + ", "
            + transform.position.y + ", " 
            + transform.position.z + ", ");
    }
}
