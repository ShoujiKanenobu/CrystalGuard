using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class BulletTowerController : TowerBase
{    
    private EnemyMovementController target;

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleAttackTime();

        FindTargetInRange();

        void HandleAttackTime()
        {
            if (Time.time > nextAttack && target != null)
            {
                Shoot();
                nextAttack = Time.time + data[level - 1].attackspeed;
            }
        }
    }
    public void FindTargetInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, data[level - 1].range);

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
        GameObject temp = BulletPool.instance.GetPooledObject();
        if(temp != null)
        {
            temp.transform.position = transform.position;
            temp.transform.rotation = Quaternion.identity;
            temp.SetActive(true);
        }
        else
        {
            temp = Instantiate(data[level - 1].projectile, transform.position, Quaternion.identity);
            BulletPool.instance.ExpandPool(temp);
        }

        temp.GetComponent<SpriteRenderer>().color = data[level - 1].projColor;
        BulletController tempBC = temp.GetComponent<BulletController>();
        tempBC.speed = data[level - 1].projSpeed;
        tempBC.damage = data[level - 1].damage;
        tempBC.target = target.gameObject;
        if (data[level - 1].debuff != null)
            tempBC.debuff = data[level - 1].debuff;

    }

}
