using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
public class BuffTowerController : TowerBase
{
    [SerializeField]
    private List<BuffTowerData> buffData = new List<BuffTowerData>();
    public override List<TowerDataBase> data
    {
        get { return buffData.Cast<TowerDataBase>().ToList(); }
    }

    private void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        RadialActiveCheck();

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, buffData[level - 1].range - 0.5f);

        foreach(Collider2D hit in hits)
        {
            if(hit.TryGetComponent<TowerBase>(out TowerBase tower))
            {
                tower.ApplyBuff(buffData[level - 1].buff, buffData[level - 1].buffAmount, GetTowerType());     
            }
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, buffData[level - 1].range - 0.5f);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<TowerBase>(out TowerBase tower))
            {
                tower.buffs.Clear();
            }
        }

        base.OnBeginDrag(eventData);
    }
}
