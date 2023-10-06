using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaTowerController : TowerBase
{
    private List<EnemyHealthController> targets = new List<EnemyHealthController>();

    // Update is called once per frame
    void FixedUpdate()
    {
        if(FindTargetsInRange())
        {
            HandleAttackTime();
        }

        void HandleAttackTime()
        {
            if (Time.time > nextAttack && targets.Count != 0)
            {
                Shoot();
                nextAttack = Time.time + data[level - 1].attackspeed;
            }
        }
    }

    public void Shoot()
    {
        GameObject temp = NovaPool.instance.GetPooledObject();
        if (temp != null)
        {
            temp.transform.position = transform.position;
            temp.transform.rotation = Quaternion.identity;
            temp.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            temp.SetActive(true);
        }
        else
        {
            temp = Instantiate(data[level - 1].projectile, transform.position, Quaternion.identity);
            NovaPool.instance.ExpandPool(temp);
        }
        temp.GetComponent<SpriteRenderer>().color = data[level - 1].projColor;
        NovaController tempNC = temp.GetComponent<NovaController>();
        tempNC.damage = data[level - 1].damage;
        tempNC.expandRate = data[level - 1].projSpeed;
        tempNC.range = data[level - 1].range;
        if (data[level - 1].debuff != null)
            tempNC.debuff = data[level - 1].debuff;
    }

    public bool FindTargetsInRange()
    {
        targets.Clear();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, data[level - 1].range);
        if (hits.Length == 0)
            return false;

        foreach(Collider2D hit in hits)
        {
            if(hit.TryGetComponent<EnemyHealthController>(out EnemyHealthController enemy))
            {
                targets.Add(enemy);
            }
        }
        return true;
    }
}
