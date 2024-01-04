using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relics/Charity")]
public class CharityBehaviour : Relic
{
    public override void OnObtained()
    {
        GameManager.instance.GainLife(30);
    }
    public override void OnLifeLost(Vector3 position)
    {
        base.OnLifeLost(position);
        GoldSystem.instance.ForceLoseGold(1);
    }
}