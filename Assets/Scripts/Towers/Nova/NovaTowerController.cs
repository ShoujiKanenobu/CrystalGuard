using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NovaTowerController : TowerBase
{
    [SerializeField]
    private List<NovaTowerData> novaData = new List<NovaTowerData>();
    public Transform attackOrigin;
    public override List<TowerDataBase> data
    {
        get { return novaData.Cast<TowerDataBase>().ToList(); }
    }
    private List<EnemyHealthController> targets = new List<EnemyHealthController>();

    public void Start()
    {
        Init();
        if (attackOrigin == null)
            attackOrigin = this.transform;
    }

    void FixedUpdate()
    {
        RadialActiveCheck();

        if (FindTargetsInRange())
        {
            HandleAttackTime();
        }

        void HandleAttackTime()
        {
            if (Time.time > nextAttack && targets.Count != 0)
            {
                Shoot();
                float finalattackspeed = novaData[level - 1].attackspeed - bonusAttackSpeed;
                finalattackspeed = Mathf.Max(finalattackspeed, 0.01f);
                nextAttack = Time.time + finalattackspeed;
            }
        }
    }

    public void Shoot()
    {
        NovaTowerData nt = novaData[level - 1];
        GameObject temp = Instantiate(nt.nova, attackOrigin.position, Quaternion.identity);

        NovaController tempNC = temp.GetComponent<NovaController>();
        tempNC.rotationSpeed = nt.rotationSpeed;
        tempNC.damage = nt.damage + (int)bonusDamage;
        tempNC.expandRate = nt.expandSpeed;
        tempNC.range = nt.range;
        if (nt.debuff != null)
            tempNC.debuff = nt.debuff;
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
