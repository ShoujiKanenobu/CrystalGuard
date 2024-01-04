using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relics/Basic/AOE")]
public class AoeRelic : Relic
{
    public float bonus;
    public override void OnObtained()
    {
        RelicBonusStatTracker.instance.AOEIncrease += bonus;
    }
}