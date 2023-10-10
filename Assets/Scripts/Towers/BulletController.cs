using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public int damage;
    public GameObject target;
    public float minHitDistance;
    private Vector3 lastKnownPos;
    
    public DebuffInfo debuff;
    public void Update()
    {
        if (target != null)
        {
            lastKnownPos = target.transform.position;
            if (target.activeSelf == false)
                target = null;
        }

        Vector3 targetPos;
        targetPos = lastKnownPos;

        if (Vector3.Distance(transform.position, targetPos) < minHitDistance)
        {
            if (target != null)
            {
                if (debuff != null)
                    target.GetComponent<EnemyStatusController>().ApplyStatusEffect(debuff);
                target.GetComponent<EnemyHealthController>().TakeDamage(damage);
            }
                
            gameObject.SetActive(false);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

}
