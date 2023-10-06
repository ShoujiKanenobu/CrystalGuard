using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletTowerData", menuName = "BulletTowerData")]
public class BulletTowerData : ScriptableObject
{
    public Color towerColor;
    public float range;
    public int damage;
    public float attackspeed;
    public float projSpeed;
    public Color projColor;
    public GameObject projectile;
    public DebuffInfo debuff;
}
