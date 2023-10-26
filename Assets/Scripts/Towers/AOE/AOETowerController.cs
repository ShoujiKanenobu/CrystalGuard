using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AOETowerController : TowerBase
{
    [SerializeField]
    private List<AOETowerData> aoeData = new List<AOETowerData>();
    public Transform attackOrigin;

    public override List<TowerDataBase> data
    {
        get { return aoeData.Cast<TowerDataBase>().ToList(); }
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
                float finalattackspeed = aoeData[level - 1].attackspeed - bonusAttackSpeed;
                finalattackspeed = Mathf.Max(finalattackspeed, 0.01f);
                nextAttack = Time.time + finalattackspeed;
            }
        }
    }
    public void FindTargetInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, aoeData[level - 1].range + bonusRange);

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
        AOETowerData ad = aoeData[level - 1];
        GameObject temp = Instantiate(ad.projectile, attackOrigin.position, Quaternion.identity);

        AOEBulletController tempABC = temp.GetComponent<AOEBulletController>();
        tempABC.AOE = ad.explosion;
        tempABC.radius = ad.radius;
        tempABC.duration = ad.duration;
        tempABC.speed = ad.projSpeed;
        tempABC.damage = ad.damage + (int)bonusDamage;
        tempABC.target = target.gameObject;
        tempABC.delay = ad.delay;
        tempABC.animFinishTime = ad.animFinishTime;
        if (ad.debuff != null)
            tempABC.debuff = ad.debuff;

        temp.SetActive(true);

    }
}
