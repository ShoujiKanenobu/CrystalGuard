using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relics/LifeThorns")]
public class ThornsLifeLostBehaviour : Relic
{
    public GameObject ThornNova;
    public override void OnLifeLost(Vector3 position)
    {
        base.OnObtained();
        Instantiate(ThornNova, position, Quaternion.identity);
    }
}
