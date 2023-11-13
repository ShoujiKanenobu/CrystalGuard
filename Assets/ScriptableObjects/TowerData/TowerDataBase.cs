using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TowerDataBase : ScriptableObject
{
    public Sprite shopIcon;
    public float range;
    public DebuffInfo debuff;
    public int damage;
    public float attackspeed;
    [MultiLineProperty(10)]
    public string description;
}
