using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicBonusStatTracker : MonoBehaviour
{
    public static RelicBonusStatTracker instance;

    public float DamageIncrease;
    public float RangeIncrease;
    public float AttackspeedIncrease;
    public float EnemySlow;
    public float AOEIncrease;
    public float ProjectileSpeedIncrease;
    public float EnemyHealthDecrease;

    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;

        ResetStats();
    }

    public void ResetStats()
    {
        DamageIncrease = 0;
        RangeIncrease = 0;
        AttackspeedIncrease = 0;
        EnemySlow = 0;
        AOEIncrease = 0;
        ProjectileSpeedIncrease = 0;
        EnemyHealthDecrease = 0;
    }


}
