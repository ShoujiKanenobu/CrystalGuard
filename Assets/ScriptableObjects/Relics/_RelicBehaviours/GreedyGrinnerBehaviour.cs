using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relics/GreedyGrinner")]
public class GreedyGrinnerBehaviour : Relic
{
    public override void OnObtained()
    {
        base.OnObtained();
        if(GameManager.instance.GetLives() <= 10)
        {
            GameManager.instance.LoseLife(GameManager.instance.GetLives() - 1);
        }
        else
        {
            GameManager.instance.LoseLife(10);
        }
        GoldSystem.instance.GainGold(25);
    }
}
