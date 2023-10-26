using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PierceTowerData", menuName = "TowerData/PierceTowerData")]
public class PierceTowerData : TowerDataBase
{
    public float radius;
    public float duration;
    public float projSpeed;
    public GameObject projectile;
}
