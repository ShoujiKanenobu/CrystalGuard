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
        GameObject t = Instantiate(ThornNova, position, Quaternion.identity);
        t.GetComponent<NovaController>().damage = EnemySpawnManager.instance.GetWaveNumber() * 2;
    }
}
