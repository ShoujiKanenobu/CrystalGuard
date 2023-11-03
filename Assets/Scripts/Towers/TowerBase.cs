using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
public abstract class TowerBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public abstract List<TowerDataBase> data { get; }
    public int level { get; private set; }

    private int xp;
    private string typing;

    protected TowerStarUIController levelHandler;

    protected float nextAttack;

    public HPBarController xpBar;

    public Dictionary<string, BuffInfo> buffs = new Dictionary<string, BuffInfo>();

    protected float bonusAttackSpeed;
    protected float bonusDamage;
    protected float bonusRange;

    private Vector3 lastLocation;
    private bool firstRun = true;
    
    public void Init()
    {
        
        if (!firstRun)
            return;
        levelHandler = GetComponent<TowerStarUIController>();
        xpBar = GetComponentInChildren<HPBarController>();
        nextAttack = 0;
        typing = gameObject.name;
        level = 1;
        xp = 0;

        bonusAttackSpeed = 0;
        bonusDamage = 0;
        bonusRange = 0;

        levelHandler.CheckStars(level);
        firstRun = false;

        UpdateXPBar();


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

    public bool GainXP(int otherXp)
    {
        if (level >= 3)
            return false;
        xp += otherXp + 1;
        if(xp >= 2)
        {
            xp = 0;
            increaseLevel();
        }
            
        UpdateXPBar();
        return true;
    }

    public int GetXP()
    {
        return xp;
    }

    public void UpdateXPBar()
    {
        xpBar.SetBarHP(xp, 2);

        if (xp == 0)
            xpBar.gameObject.SetActive(false);
        else
            xpBar.gameObject.SetActive(true);
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

    public Vector3 GetLastLocation()
    {
        return lastLocation;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        MapManager.instance.RemoveTower(transform.position);
        lastLocation = this.transform.position;
        this.transform.position = new Vector3(999, 999, 0);
        GameObject previewObj = TowerBenchController.instance.previewObj;
        
        if(level >= 1)
            previewObj.GetComponent<TowerRangeIndicator>().ShowRadius(data[level - 1].range);
        else
            previewObj.GetComponent<TowerRangeIndicator>().ShowRadius(data[0].range);
        previewObj.GetComponent<SpriteRenderer>().sprite = data[0].shopIcon;
        previewObj.SetActive(true);
        UpdateXPBar();
    }

    public void OnDrag(PointerEventData eventData)
    {
        GameObject previewObj = TowerBenchController.instance.previewObj;
        Vector3 nextPos = MapManager.instance.MousePositionWorld;
        nextPos.z = 0;
        previewObj.transform.position = nextPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject previewObj = TowerBenchController.instance.previewObj;

        previewObj.SetActive(false);

        if (MapManager.instance.IsCurrentMousePosTileEmpty() && MapManager.instance.IsBuildableAtTile(MapManager.instance.MousePositionGrid))
        {
            this.transform.position = MapManager.instance.MousePositionGrid + new Vector3(0.5f, 0.5f, 0);
            MapManager.instance.PlaceTower(transform.position, this);
        }
        else if (MapManager.instance.IsBuildableAtTile(MapManager.instance.MousePositionGrid) && 
            MapManager.instance.GetTowerAtMousePositionGrid().GetTowerType() == GetTowerType())
        {
            if (MapManager.instance.GetTowerAtMousePositionGrid().GainXP(GetXP()))
            {
                MapManager.instance.RemoveTower(lastLocation);
                Destroy(this.gameObject);
            }
            else
            {
                transform.position = lastLocation;
                MapManager.instance.PlaceTower(lastLocation, this);
            }
        }
        else
        {
            this.transform.position = lastLocation;
            MapManager.instance.PlaceTower(lastLocation, this);
        }
        UpdateXPBar();
        GameManager.instance.RequestStateChange(GameState.FreeHover, false);
    }

    
}
