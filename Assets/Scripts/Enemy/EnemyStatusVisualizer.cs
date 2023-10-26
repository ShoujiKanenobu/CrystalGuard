using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Copied from the EnemyStatusController. Purely for reference
 public enum StatusType
{
    Speed = 0, AttackSpeed = 1, DamageAmp = 2, Poison = 3, Darkness = 4, Bleed = 5
}
*/

[System.Serializable]
public struct StatusVisual
{
    public GameObject visual;
    public StatusType type;
}

public class EnemyStatusVisualizer : MonoBehaviour
{
    //Each statustype needs a visual indicator
    public List<StatusVisual> visuals = new List<StatusVisual>();

    public void ApplyVisual(StatusType t)
    {
        foreach(StatusVisual v in visuals)
        {
            if (v.type == t)
                v.visual.SetActive(true);
        }
    }

    public void RemoveVisual(StatusType t)
    {
        foreach (StatusVisual v in visuals)
        {
            if (v.type == t)
                v.visual.SetActive(false);
        }
    }

    private void OnDisable()
    {
        foreach(StatusVisual v in visuals)
        {
            v.visual.SetActive(false);
        }
    }
}
