using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEController : MonoBehaviour
{
    public float animFinishTime;
    public float delay;
    public float radius;
    public int damage;
    public float duration;
    public DebuffInfo debuff;

    private List<Collider2D> alreadyHit = new List<Collider2D>();
    private float hitboxEndTime;
    private float hitboxStartTime;
    private float deleteTime;
    public void Init()
    {
        hitboxEndTime = Time.time + duration + delay;
        hitboxStartTime = Time.time + delay;
        deleteTime = Time.time + duration + delay + animFinishTime;

        transform.localScale = new Vector3(radius * 2f, radius * 2f, 1f);

        alreadyHit.Clear();
    }

    public void Update()
    {
        if (deleteTime < Time.time)
            Destroy(this.gameObject);

        if (hitboxStartTime > Time.time || hitboxEndTime < Time.time)
            return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach(Collider2D hit in hits)
        {
            if (alreadyHit.Contains(hit))
                continue;

            hit.gameObject.TryGetComponent<EnemyHealthController>(out EnemyHealthController temp);
            if(temp != null)
            {
                if (debuff != null)
                    temp.GetComponent<EnemyStatusController>().ApplyStatusEffect(debuff);
                temp.TakeDamage(damage);
                alreadyHit.Add(hit);
            }
        }
    }
}
