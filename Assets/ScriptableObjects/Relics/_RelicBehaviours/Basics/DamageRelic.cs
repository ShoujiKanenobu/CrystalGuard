 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relics/Basic/Damage")]
public class DamageRelic : Relic
{
    public float bonus;
    public override void OnObtained()
    {
        RelicBonusStatTracker.instance.DamageIncrease += bonus;
    }
}
