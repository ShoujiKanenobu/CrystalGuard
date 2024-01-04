using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relics/Basic/EnemyHealth")]
public class EnemyHealthRelic : Relic
{
    public float bonus;
    public override void OnObtained()
    {
        RelicBonusStatTracker.instance.EnemyHealthDecrease += bonus;
    }
}