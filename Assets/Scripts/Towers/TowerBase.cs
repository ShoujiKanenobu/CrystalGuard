using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BuffInfo
{
    
    public BuffType bufftype;
    public float amount;
    public BuffInfo(BuffType b, float a) 
    {
        bufftype = b;
        amount = a;
    }
}

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(TowerStarUIController))]
public abstract class TowerBase : MonoBehaviour
{
    public abstract List<TowerDataBase> data { get; }
    public int level { get; private set; }
    
    private string typing;

    protected TowerStarUIController levelHandler;

    protected float nextAttack;

    public Dictionary<string, BuffInfo> buffs = new Dictionary<string, BuffInfo>();

    protected float bonusAttackSpeed;
    protected float bonusDamage;
    protected float bonusRange;

    protected void Init()
    {
        levelHandler = GetComponent<TowerStarUIController>();

        nextAttack = 0;
        typing = gameObject.name;
        level = 1;

        bonusAttackSpeed = 0;
        bonusDamage = 0;
        bonusRange = 0;

        levelHandler.CheckStars(level);
    }
    public string GetTowerType()
    {
        return typing + level;
    }
    public void increaseLevel()
    {
        if (level < 3)
            level++;
        levelHandler.CheckStars(level);
    }

    public void ApplyBuff(BuffType b, float amount, string source)
    {
        if(!buffs.ContainsKey(source))
        {
            buffs.Add(source, new BuffInfo(b, amount));
        }
        RecalculateBuffs();
    }

    public void RemoveBuff(string source)
    {
        if (buffs.ContainsKey(source))
            buffs.Remove(source);

        RecalculateBuffs();
    }
    
    public void RecalculateBuffs()
    {
        bonusAttackSpeed = 0;
        bonusDamage = 0;
        bonusRange = 0;

        foreach (var entry in buffs)
        {
            switch(entry.Value.bufftype)
            {
                case BuffType.attackspeed:
                    bonusAttackSpeed += entry.Value.amount;
                    break;
                case BuffType.damage:
                    bonusDamage += entry.Value.amount;
                    break;
                case BuffType.range:
                    bonusRange += entry.Value.amount; 
                    break;
            }
        }
    }
}
