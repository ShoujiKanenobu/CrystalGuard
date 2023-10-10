using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(TowerStarUIController))]
public abstract class TowerBase : MonoBehaviour
{
    public abstract List<TowerDataBase> data { get; }
    public int level { get; private set; }
    private string typing;

    protected TowerStarUIController levelHandler;

    protected float nextAttack;

    protected void Init()
    {
        levelHandler = GetComponent<TowerStarUIController>();


        nextAttack = 0;
        typing = gameObject.name;
        level = 1;


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
}
