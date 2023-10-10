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
    public Color AOEColor;
    public float minHitDistance;
    private Vector3 targetPos;

    public DebuffInfo debuff;

    private void OnEnable()
    {
        if (target != null)
        {
            targetPos = target.transform.position;
            if (target.activeSelf == false)
                target = null;
        }
    }

    private void OnDisable()
    {
        target = null;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, targetPos) < minHitDistance)
        {
            GameObject temp = AOEPool.instance.GetPooledObject();
            if(temp != null)
            {
                temp.transform.position = this.transform.position;
                temp.transform.rotation = Quaternion.identity;
            }
            else
            {
                temp = Instantiate(AOE, transform.position, Quaternion.identity);
                AOEPool.instance.ExpandPool(temp);
                temp.SetActive(false);
            }
            temp.GetComponent<SpriteRenderer>().color = AOEColor;
            AOEController aoe = temp.GetComponent<AOEController>();
            aoe.damage = damage;
            aoe.duration = duration;
            aoe.radius = radius;
            if(debuff != null)
                aoe.debuff = debuff;

            temp.SetActive(true);

            this.gameObject.SetActive(false);
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
}
