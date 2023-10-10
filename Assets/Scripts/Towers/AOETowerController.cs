using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AOETowerController : TowerBase
{
    [SerializeField]
    private List<AOETowerData> aoeData = new List<AOETowerData>();

    public override List<TowerDataBase> data
    {
        get { return aoeData.Cast<TowerDataBase>().ToList(); }
    }

    private EnemyMovementController target;

    public void Start()
    {
        Init();
        this.GetComponent<SpriteRenderer>().color = aoeData[0].towerColor;
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
                nextAttack = Time.time + aoeData[level - 1].attackspeed;
            }
        }
    }
    public void FindTargetInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, aoeData[level - 1].range);

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
        GameObject temp = AOEBulletPool.instance.GetPooledObject();
        if (temp != null)
        {
            temp.transform.position = transform.position;
            temp.transform.rotation = Quaternion.identity;
        }
        else
        {
            temp = Instantiate((aoeData[level - 1]).projectile, transform.position, Quaternion.identity);
            AOEBulletPool.instance.ExpandPool(temp);
            temp.SetActive(false);
        }
        temp.GetComponent<SpriteRenderer>().color = aoeData[level - 1].projColor;
        AOEBulletController tempABC = temp.GetComponent<AOEBulletController>();
        tempABC.AOEColor = aoeData[level - 1].aoeColor;
        tempABC.radius = aoeData[level - 1].radius;
        tempABC.duration = aoeData[level - 1].duration;
        tempABC.speed = aoeData[level - 1].projSpeed;
        tempABC.damage = aoeData[level - 1].damage;
        tempABC.target = target.gameObject;
        if (aoeData[level - 1].debuff != null)
            tempABC.debuff = aoeData[level - 1].debuff;

        temp.SetActive(true);

    }
}
