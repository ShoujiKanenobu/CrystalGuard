using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relics/FruitOfLife")]
public class FruitOfLifeBehaviour : Relic
{
    public override void OnObtained()
    {
        base.OnObtained();
        GameManager.instance.GainLife(10);
    }
}
