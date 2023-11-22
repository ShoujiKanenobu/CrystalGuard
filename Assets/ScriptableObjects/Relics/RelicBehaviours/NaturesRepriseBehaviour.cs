using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relics/NaturesReprise")]
public class NaturesRepriseBehaviour : Relic
{
    public override void OnRoundEnd()
    {
        base.OnObtained();
        GameManager.instance.GainLife(1);

    }
}