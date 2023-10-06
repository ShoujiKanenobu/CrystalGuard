using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NonUISpriteButton : MonoBehaviour
{
    public UnityEvent Response;
    public bool respondNextFrameLate;
    private void Awake()
    {
        respondNextFrameLate = false;
    }

    private void OnMouseDown()
    {
        respondNextFrameLate = true;
    }

    private void LateUpdate()
    {
        if(respondNextFrameLate)
        {
            Response.Invoke();
            respondNextFrameLate = false;
        }
    }

}
