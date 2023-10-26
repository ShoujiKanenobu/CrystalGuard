using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    damage = 0, attackspeed = 1, range = 2
}

[CreateAssetMenu(fileName = "BuffTowerData", menuName = "TowerData/BuffTowerData")]
public class BuffTowerData : TowerDataBase
{
    public BuffType buff;
    public float buffAmount;
}
