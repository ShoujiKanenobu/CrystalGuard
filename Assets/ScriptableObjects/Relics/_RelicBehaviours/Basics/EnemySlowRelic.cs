using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relics/Basic/EnemySlow")]
public class EnemySlow : Relic
{
    public float bonus;
    public override void OnObtained()
    {
        RelicBonusStatTracker.instance.EnemySlow += bonus;
    }
}