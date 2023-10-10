using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NovaTowerController : TowerBase
{
    [SerializeField]
    private List<NovaTowerData> novaData = new List<NovaTowerData>();
    public override List<TowerDataBase> data
    {
        get { return novaData.Cast<TowerDataBase>().ToList(); }
    }
    private List<EnemyHealthController> targets = new List<EnemyHealthController>();

    public void Start()
    {
        Init();
        this.GetComponent<SpriteRenderer>().color = novaData[0].towerColor;
    }

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
                nextAttack = Time.time + novaData[level - 1].attackspeed;
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
            temp = Instantiate(novaData[level - 1].nova, transform.position, Quaternion.identity);
            NovaPool.instance.ExpandPool(temp);
        }
        temp.GetComponent<SpriteRenderer>().color = novaData[level - 1].novaColor;
        NovaController tempNC = temp.GetComponent<NovaController>();
        tempNC.damage = novaData[level - 1].damage;
        tempNC.expandRate = novaData[level - 1].expandSpeed;
        tempNC.range = novaData[level - 1].range;
        if (novaData[level - 1].debuff != null)
            tempNC.debuff = novaData[level - 1].debuff;
    }

    public bool FindTargetsInRange()
    {
        targets.Clear();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, novaData[level - 1].range);
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
