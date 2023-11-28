using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relics/SpaceExchange")]
public class SpaceExchangeBehaviour : Relic
{
    public override void OnObtained()
    {
        base.OnObtained();
        GoldSystem.instance.GainGold(15);
        GameManager.instance.GainLife(5);
        MapManager.instance.LoseTowerLimit();

    }
}