using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public static RelicManager instance;

    [SerializeField]
    protected List<Relic> obtainedRelics;
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    public bool ContainsRelic(Relic r)
    {
        return obtainedRelics.Contains(r);
    }

    public void AddRelic(Relic r)
    {
        if(!obtainedRelics.Contains(r))
        {
            obtainedRelics.Add(r);
        }
    }

    public void RemoveRelic(Relic r)
    {
        obtainedRelics.Remove(r);
    }
}
