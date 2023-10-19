using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEBulletController : MonoBehaviour
{
    public float speed;
    public int damage;
    public float radius;
    public float duration;
    public GameObject target;
    public GameObject AOE;
    public float minHitDistance = 0.35f;
    private Vector3 targetPos;

    public DebuffInfo debuff;


    void Update()
    {
        if (target != null)
        {
            targetPos = target.transform.position;
            if (target.activeSelf == false)
                target = null;
        }
        if (Vector3.Distance(transform.position, targetPos) < minHitDistance)
        {
            GameObject temp = Instantiate(AOE, transform.position, Quaternion.identity);
            
            AOEController aoe = temp.GetComponent<AOEController>();
            aoe.damage = damage;
            aoe.duration = duration;
            aoe.radius = radius;
            if(debuff != null)
                aoe.debuff = debuff;
            aoe.Init();

            Destroy(this.gameObject);
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
}
