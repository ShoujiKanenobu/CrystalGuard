using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEController : MonoBehaviour
{
    public float radius;
    public int damage;
    public float duration;
    public DebuffInfo debuff;

    private List<Collider2D> alreadyHit = new List<Collider2D>();
    private float endTime;
    public void Init()
    {
        endTime = Time.time + duration;
        alreadyHit.Clear();
    }

    public void Update()
    {
        if (endTime < Time.time)
            Destroy(this.gameObject);

        transform.localScale = new Vector3(radius * 2f, radius * 2f, 1f);
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
