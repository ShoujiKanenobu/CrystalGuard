using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RelicManager : MonoBehaviour
{
    public static RelicManager instance;

    [SerializeField]
    private GameObject relicPanel;
    [SerializeField]
    private GameObject relicIcon;

    [SerializeField]
    private GameEventListener OnRoundEndListener;
    [SerializeField]
    private Vector3EventListener OnEnemyKillListener;
    [SerializeField]
    private Vector3EventListener OnLifeLostListener;

    [SerializeField]
    private Relic TuneUpRelic;

    [SerializeField]
    public List<Relic> obtainedRelics  = new List<Relic>();
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

    //This is so bad
    public bool ContainsTuneUp()
    {
        return ContainsRelic(TuneUpRelic);
    }

    public void AddRelic(Relic r)
    {
        if(!obtainedRelics.Contains(r))
        {
            obtainedRelics.Add(r);
            r.OnObtained();
            GameObject temp = Instantiate(relicIcon, relicPanel.transform);
            temp.GetComponent<Image>().sprite = r.sprite;
            temp.GetComponent<RelicTooltipActivator>().relic = r;
        }
    }

    public void RemoveRelic(Relic r)
    {
        obtainedRelics.Remove(r);
    }

    public void ClearRelics()
    {
        obtainedRelics.Clear();
        foreach(Transform child in relicPanel.transform)
        {
            Destroy(child.gameObject);
        }
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
