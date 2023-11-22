using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relics/EconomyRelic")]
public class GrowingEconomyBehaviour : Relic
{
    public override void OnRoundEnd()
    {
        base.OnObtained();
        int gained = (int)GoldSystem.instance.currentGold / 10;
        GoldSystem.instance.GainGold(gained);

    }
}