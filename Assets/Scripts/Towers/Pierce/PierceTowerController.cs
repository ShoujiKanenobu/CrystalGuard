using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PierceTowerController : TowerBase
{
    [SerializeField]
    private List<PierceTowerData> bulletData = new List<PierceTowerData>();
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
        RadialActiveCheck();

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
        PierceTowerData bt = bulletData[level - 1];
        GameObject temp = Instantiate(bt.projectile, attackOrigin.position, Quaternion.identity);

        PierceBulletController pbc = temp.GetComponent<PierceBulletController>();
        if (pbc != null)
        {
            pbc.duration = bt.duration;
            pbc.radius = bt.radius;
            pbc.speed = bt.projSpeed;
            pbc.damage = bt.damage + (int)bonusDamage;
            pbc.target = target.gameObject;
            if (bt.debuff != null)
                pbc.debuff = bt.debuff;
        }
        else
            Debug.LogError("Couldn't find Pierce Bullet Controller on prefab, maybe its the wrong projectile type?");

    }
}
