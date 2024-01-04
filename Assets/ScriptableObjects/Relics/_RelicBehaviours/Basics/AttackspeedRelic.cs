using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relics/Basic/Attackspeed")]
public class AttackspeedRelic : Relic
{
    public float bonus;
    public override void OnObtained()
    {
        RelicBonusStatTracker.instance.AttackspeedIncrease += bonus;
    }
}