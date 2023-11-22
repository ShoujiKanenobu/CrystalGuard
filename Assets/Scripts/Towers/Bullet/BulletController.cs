using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public int damage;
    public GameObject target;
    public float minHitDistance = 0.35f;
    private Vector3 lastKnownPos;
    
    public DebuffInfo debuff;

    public AudioPoolInfo sound;
    public AudioPoolInfo hitSound;

    public void Awake()
    {
        if(sound.clip != null)
            AudioSourceProvider.instance.PlayClipOnSource(sound);
    }

    public void Update()
    {
        if (target != null)
        {
            lastKnownPos = target.transform.position;
            transform.LookAt(lastKnownPos);
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
            if(hitSound.clip != null)
                AudioSourceProvider.instance.PlayClipOnSource(hitSound);
            Destroy(this.gameObject);
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

}
