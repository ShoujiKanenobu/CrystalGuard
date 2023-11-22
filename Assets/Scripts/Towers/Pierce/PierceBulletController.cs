using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierceBulletController : MonoBehaviour
{
    public float radius;
    public float speed;
    public float duration;
    public int damage;
    public GameObject target;
    public DebuffInfo debuff;

    public float minHitDistance = 0.35f;
    private Vector3 travelMagnitude;
    private List<Collider2D> alreadyHit = new List<Collider2D>();
    private float endTime;

    public AudioPoolInfo sound;
    public AudioPoolInfo hitSound;

    void Start()
    {
        endTime = Time.time + duration;
        alreadyHit.Clear();
        transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        if (target != null)
        {
            AudioSourceProvider.instance.PlayClipOnSource(sound);
            travelMagnitude = target.transform.position - this.transform.position;
            travelMagnitude = travelMagnitude.normalized;
            this.transform.rotation = Quaternion.LookRotation(travelMagnitude);
        }
        else
            Destroy(this.gameObject);
    }

    void Update()
    {
        if (endTime <= Time.time)
            Destroy(this.gameObject);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D hit in hits)
        {
            if (alreadyHit.Contains(hit))
                continue;

            hit.gameObject.TryGetComponent<EnemyHealthController>(out EnemyHealthController temp);
            if (temp != null)
            {
                if (debuff != null)
                    hit.gameObject.GetComponent<EnemyStatusController>().ApplyStatusEffect(debuff);
                temp.TakeDamage(damage);
                alreadyHit.Add(hit);
                AudioSourceProvider.instance.PlayClipOnSource(hitSound);
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, transform.position + travelMagnitude, Time.deltaTime * speed);
    }
}
