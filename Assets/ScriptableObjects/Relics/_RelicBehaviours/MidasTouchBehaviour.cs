using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relics/MidasTouch")]
public class MidasTouchBehaviour : Relic
{
    public GameObject GoldPop;
    public override void OnEnemyKilled(Vector3 position)
    {
        base.OnObtained();
        if (Random.value < 0.1f)
        {
            GoldSystem.instance.GainGold(1);
            Instantiate(GoldPop, position, Quaternion.identity);
        }
            
    }
}