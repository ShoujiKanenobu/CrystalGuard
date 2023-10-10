using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletTowerData", menuName = "TowerData/BulletTowerData")]
public class BulletTowerData : TowerDataBase
{
    public float projSpeed;
    public Color projColor;
    public GameObject projectile;
}
