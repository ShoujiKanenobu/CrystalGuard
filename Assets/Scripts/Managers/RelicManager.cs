using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public static RelicManager instance;

    [SerializeField]
    private GameEventListener OnRoundEndListener;
    [SerializeField]
    private Vector3EventListener OnEnemyKillListener;
    [SerializeField]
    private Vector3EventListener OnLifeLostListener;

    [SerializeField]
    protected List<Relic> obtainedRelics;
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;

        OnRoundEndListener.response = OnRoundEnd;
        OnEnemyKillListener.response = OnEnemyKilled;
        OnLifeLostListener.response = OnLifeLost;
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
            r.OnObtained();
        }
    }

    public void RemoveRelic(Relic r)
    {
        obtainedRelics.Remove(r);
    }

    public void OnRoundEnd()
    {
        foreach(Relic r in obtainedRelics)
        {
            r.OnRoundEnd();
        }
    }

    public void OnLifeLost(Vector3 position)
    {
        foreach (Relic r in obtainedRelics)
        {
            r.OnLifeLost(position);
        }
    }

    public void OnEnemyKilled(Vector3 position)
    {
        foreach(Relic r in obtainedRelics)
        {
            r.OnEnemyKilled(position);
        }
    }
}
