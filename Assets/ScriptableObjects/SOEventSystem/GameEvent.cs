using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
//I feel like I don't really need to credit this bit of code but come on. Ryan Hipple the goat

[CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();
    private List<Vector3EventListener> vec3Listeners = new List<Vector3EventListener>();

    [Button]
    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    [Button]
    public void Raise(Vector3 position)
    {
        for (int i = vec3Listeners.Count - 1; i >= 0; i--)
        {
            vec3Listeners[i].OnEventRaised(position);
        }
    }

    public void RegisterListener(Vector3EventListener listener)
    {
        vec3Listeners.Add(listener);
    }
    public void UnregisterListener(Vector3EventListener listener)
    {
        vec3Listeners.Remove(listener);
    }

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }
    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}
