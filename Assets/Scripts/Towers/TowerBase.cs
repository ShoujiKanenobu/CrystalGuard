using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(TowerStarUIController))]
public class TowerBase : MonoBehaviour
{
    public int level { get; private set; }
    private string typing;

    public List<BulletTowerData> data;
    protected TowerStarUIController levelHandler;

    protected float nextAttack;

    // Start is called before the first frame update
    void Start()
    {
        levelHandler = GetComponent<TowerStarUIController>();


        nextAttack = 0;
        typing = gameObject.name;
        level = 1;


        levelHandler.CheckStars(level);
        this.GetComponent<SpriteRenderer>().color = data[0].towerColor;
    }

    // Update is called once per frame
    void Update()
    {
        
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
