using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class GameEventListener : MonoBehaviour
{
    public delegate void GamEventDelegate();

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

    public void OnEventRaised()
    {
        if(response != null)
            response.Invoke();
    }

}
