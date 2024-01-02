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

    private RadialTimerController radialTimer;
    private float moveCooldown;

    public AudioPoolInfo pickupSound;
    public AudioPoolInfo dropSound;

    [SerializeField]
    private GameObject rangeBuffIndicator;
    [SerializeField]
    private GameObject damageBuffIndicator;
    [SerializeField]
    private GameObject attackSpeedBuffIndicator;

    public void Init()
    {
        
        if (!firstRun)
            return;
        levelHandler = GetComponent<TowerStarUIController>();
        xpBar = GetComponentInChildren<HPBarController>();
        radialTimer = GetComponentInChildren<RadialTimerController>();
        nextAttack = 0;
        typing = gameObject.name;
        level = 1;
        xp = 0;

        moveCooldown = 8f;

        bonusAttackSpeed = 0;
        bonusDamage = 0;
        bonusRange = 0;

        levelHandler.CheckStars(level);
        firstRun = false;

        UpdateXPBar();


    }

    public void RadialActiveCheck()
    {
        if (GameManager.instance.isEnemiesAlive() || EnemySpawnManager.instance.isWaveSpawning())
            radialTimer.gameObject.SetActive(true);
        else
        {
            radialTimer.SetCooldown(0f);
            radialTimer.gameObject.SetActive(false);
        }
    }

    public string GetTowerType()
    {
        return typing + level;
    }

    public void increaseLevel()
    {
        if(RelicManager.instance.ContainsTuneUp())
        {
            GoldSystem.instance.GainGold(3);
        }

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

        if(attackSpeedBuffIndicator != null)
        {
            attackSpeedBuffIndicator.SetActive(false);
            damageBuffIndicator.SetActive(false);
            rangeBuffIndicator.SetActive(false);
        }
        

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
        if (attackSpeedBuffIndicator != null)
        {
            if (bonusAttackSpeed != 0)
                attackSpeedBuffIndicator.SetActive(true);
            if (bonusDamage != 0)
                damageBuffIndicator.SetActive(true);
            if (bonusRange != 0)
                rangeBuffIndicator.SetActive(true);
        }
    }

    public Vector3 GetLastLocation()
    {
        return lastLocation;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            return;

        if (!isMoveable() || GameManager.instance.isGamePaused())
            return;

        AudioSourceProvider.instance.PlayClipOnSource(pickupSound);

        MapManager.instance.RemoveTower(transform.position);
        lastLocation = this.transform.position;
        this.transform.position = new Vector3(999, 999, 0);

        buffs.Clear();
        RecalculateBuffs();

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
        if (eventData.button == PointerEventData.InputButton.Right)
            return;

        if (!isMoveable() || GameManager.instance.isGamePaused())
            return;
        GameObject previewObj = TowerBenchController.instance.previewObj;
        Vector3 nextPos = MapManager.instance.MousePositionWorld;
        nextPos.z = 0;
        previewObj.transform.position = nextPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            return;

        if (!isMoveable() || GameManager.instance.isGamePaused() || this.transform.position != new Vector3(999, 999, 0))
            return;

        GameObject previewObj = TowerBenchController.instance.previewObj;

        previewObj.SetActive(false);

        AudioSourceProvider.instance.PlayClipOnSource(dropSound);

        if (MapManager.instance.IsCurrentMousePosTileEmpty() && MapManager.instance.IsBuildableAtTile(MapManager.instance.MousePositionGrid))
        {
            this.transform.position = MapManager.instance.MousePositionGrid + new Vector3(0.5f, 0.5f, 0);
            MapManager.instance.PlaceTower(transform.position, this);
            SetOnCooldown();
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
        buffs.Clear();
        RecalculateBuffs();

        GameManager.instance.RequestStateChange(GameState.FreeHover, false);
    }

    public bool isMoveable()
    {
        if (!GameManager.instance.isEnemiesAlive() && !EnemySpawnManager.instance.isWaveSpawning())
        {
            return true;
        }
            
        if (radialTimer.GetCooldownPercent() == 0)
        {
            return true;
        }

        return false;
    }

    public void SetOnCooldown()
    {
        radialTimer.SetCooldown(moveCooldown);
    }
}
