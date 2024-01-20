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
    public AudioPoolInfo castSound;
    public AudioPoolInfo hitSound;
    public GameObject chainLightning;
    public Relic chainLightningRelic;

    private List<Collider2D> alreadyHit = new List<Collider2D>();
    private float hitboxEndTime;
    private float hitboxStartTime;
    private float deleteTime;

    private bool isEcho;
    
    public void Init()
    {
        isEcho = false;
        hitboxEndTime = Time.time + duration + delay;
        hitboxStartTime = Time.time + delay;
        deleteTime = Time.time + duration + delay + animFinishTime;

        transform.localScale = new Vector3(radius * 2f, radius * 2f, 1f);

        alreadyHit.Clear();
        if(castSound.clip != null)
            AudioSourceProvider.instance.PlayClipOnSource(castSound);
    }

    public void Update()
    {
        if (deleteTime < Time.time)
        {
            
            if(RelicManager.instance.ContainsEcho() && !isEcho)
            {
                GameObject temp = Instantiate(this.gameObject, this.transform.position, Quaternion.identity);

                AOEController aoe = temp.GetComponent<AOEController>();
                aoe.delay = delay;
                aoe.damage = damage / 2;
                aoe.duration = duration;
                aoe.radius = radius;
                aoe.animFinishTime = animFinishTime;
                if (debuff != null)
                    aoe.debuff = debuff;
                aoe.Init();
                aoe.isEcho = true;
            }
            Destroy(this.gameObject);
        }
            

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
                if(hitSound.clip != null)
                    AudioSourceProvider.instance.PlayClipOnSource(hitSound);
                alreadyHit.Add(hit);

                if(chainLightningRelic != null)
                {
                    if (RelicManager.instance.ContainsRelic(chainLightningRelic))
                    {
                        ChainLightningController chain = Instantiate(chainLightning, transform.position, Quaternion.identity).GetComponent<ChainLightningController>();
                        chain.Init(temp, damage);
                    }
                }
            }
        }
    }
}
