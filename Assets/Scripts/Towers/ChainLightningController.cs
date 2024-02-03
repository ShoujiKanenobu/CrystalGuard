using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class ChainLightningController : MonoBehaviour
{
    private List<EnemyHealthController> alreadyHit = new List<EnemyHealthController>();

    [SerializeField]
    private LineRenderer chain;

    [SerializeField]
    private float range;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int maxBounces;

    private int damage;

    private int bounces;

    private EnemyHealthController currentlyAttached;
    private EnemyHealthController nextTarget;

    [Button]
    public void Init(EnemyHealthController firstTarget, int damage)
    {
        this.transform.position = Vector3.zero;
        this.damage = damage;
        currentlyAttached = firstTarget;
        alreadyHit.Add(currentlyAttached);
        AquireNextTarget();
        firstTarget.TakeDamage(damage);
        bounces = 1;
    }

    private void Update()
    {

        chain.SetPosition(1, nextTarget.transform.position);

        MoveTowardsNextTarget();

        if (chain.GetPosition(0) == chain.GetPosition(1))
        {
            if (bounces >= maxBounces)
            {
                Destroy(this.gameObject);
            }

            if (nextTarget != null)
            {
                if (nextTarget.gameObject.activeSelf)
                {
                    nextTarget.TakeDamage(damage);

                    currentlyAttached = nextTarget;
                    AquireNextTarget();
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }

        }

    }

    private void MoveTowardsNextTarget()
    {
        Vector3 nextPos = Vector3.MoveTowards(chain.GetPosition(0), chain.GetPosition(1), speed * Time.deltaTime);
        chain.SetPosition(0, nextPos);
    }

    private void AquireNextTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(currentlyAttached.transform.position, range);

        float closest = 999;
        EnemyHealthController closestEnemy = null;
        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<EnemyHealthController>(out EnemyHealthController enemy))
            {
                if(!alreadyHit.Contains(enemy))
                {
                    float dist = Vector3.Distance(currentlyAttached.transform.position, enemy.transform.position);
                    if (dist < closest)
                    {
                        closestEnemy = enemy;
                        closest = dist;
                    }
                }
            }
        }
        if (closestEnemy == null)
        {
            Destroy(this.gameObject);
            return;
        }
            

        alreadyHit.Add(closestEnemy);
        nextTarget = closestEnemy;

        bounces++;
        chain.SetPosition(0, currentlyAttached.transform.position);

        if (nextTarget == null || !nextTarget.gameObject.activeSelf)
            Destroy(this.gameObject);
        chain.SetPosition(1, nextTarget.transform.position);

    }
}
