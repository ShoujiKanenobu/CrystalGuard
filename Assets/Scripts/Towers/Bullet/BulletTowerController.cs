using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletTowerController : TowerBase
{    
    [SerializeField]
    private List<BulletTowerData> bulletData = new List<BulletTowerData>();
    public Transform attackOrigin;
    public override List<TowerDataBase> data
    {
        get { return bulletData.Cast<TowerDataBase>().ToList(); }
    }

    private EnemyMovementController target;

    public void Start()
    {
        Init();
        if (attackOrigin == null)
            attackOrigin = this.transform;
    }

    void FixedUpdate()
    {
        HandleAttackTime();

        FindTargetInRange();

        void HandleAttackTime()
        {
            if (Time.time > nextAttack && target != null)
            {
                Shoot();
                float finalattackspeed = bulletData[level - 1].attackspeed - bonusAttackSpeed;
                finalattackspeed = Mathf.Max(finalattackspeed, 0.01f);
                nextAttack = Time.time + finalattackspeed;
            }
        }
    }
    public void FindTargetInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, bulletData[level - 1].range + bonusRange);

        if (target != null)
        {
            if (!hits.Contains(target.GetComponent<Collider2D>()))
            {
                target = null;
            }
        }
        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<EnemyMovementController>(out EnemyMovementController enemy))
            {
                if (target == null)
                    target = enemy;
                else
                {
                    if (target.distanceTraveled < enemy.distanceTraveled)
                    {
                        target = enemy;
                    }
                }
            }
        }
    }

    private void Shoot()
    {
        BulletTowerData bt = bulletData[level - 1];
        GameObject temp = Instantiate(bt.projectile, attackOrigin.position, Quaternion.identity);

        BulletController tempBC = temp.GetComponent<BulletController>();
        if (tempBC != null)
        {
            tempBC.speed = bt.projSpeed;
            tempBC.damage = bt.damage + (int)bonusDamage;
            tempBC.target = target.gameObject;
            if (bt.debuff != null)
                tempBC.debuff = bt.debuff;
        }
        else
            Debug.LogError("Couldn't find Bullet Controller on prefab, maybe its the wrong projectile type?");
        
    }

}
