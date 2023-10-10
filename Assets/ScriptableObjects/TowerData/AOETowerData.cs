using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AOETowerData", menuName = "TowerData/AOETowerData")]
public class AOETowerData : TowerDataBase
{
    public float radius;
    public float duration;
    public float projSpeed;
    public Color projColor;
    public Color aoeColor;
    public GameObject projectile;
}