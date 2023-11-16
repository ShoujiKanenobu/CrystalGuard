using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3EventListener : MonoBehaviour
{
    public delegate void GamEventDelegate(Vector3 position);

    public GameEvent Event;
    public GamEventDelegate response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }
    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Vector3 position)
    {
        if (response != null)
            response.Invoke(position);
    }

}