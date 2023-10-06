using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaController : MonoBehaviour
{
    public float expandRate;
    public int damage;
    public DebuffInfo debuff;
    public float range;

    private List<Collider2D> alreadyHit = new List<Collider2D>();



    private void OnEnable()
    {
        transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        alreadyHit.Clear();
    }

    void FixedUpdate()
    {
        transform.localScale += new Vector3(expandRate * Time.deltaTime, expandRate * Time.deltaTime, expandRate * Time.deltaTime);

        if (transform.localScale.x > range * 2)
            this.gameObject.SetActive(false);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x / 2f);
        foreach (Collider2D hit in hits)
        {
            if (alreadyHit.Contains(hit))
                continue;

            hit.gameObject.TryGetComponent<EnemyHealthController>(out EnemyHealthController temp);
            if (temp != null)
            {
                temp.TakeDamage(damage);
                alreadyHit.Add(hit);
                if (debuff != null)
                    hit.gameObject.GetComponent<EnemyStatusController>().ApplyStatusEffect(debuff);
            }
        }
    }
}
