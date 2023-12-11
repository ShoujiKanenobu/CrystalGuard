using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShardsBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject shardPrefab;
    [SerializeField]
    private int angles;

    [SerializeField]
    private Relic iceShardRelic;

    private BulletController bt;
    private void Start()
    {
        bt = this.GetComponent<BulletController>();
    }

    private void OnDestroy()
    {
        if (!RelicManager.instance.ContainsRelic(iceShardRelic))
            return;

        float randOffset = Random.value * 360f;
        float step = 360 / angles;
        float currentAngle = randOffset;
        for (int i = 0; i < angles; i++)
        {
            GameObject temp = Instantiate(shardPrefab, this.transform.position, Quaternion.identity);
            PierceBulletController pbc = temp.GetComponent<PierceBulletController>();
            if(pbc != null)
            {
                pbc.duration = 0.5f;
                pbc.radius = 0.1f;
                pbc.speed = bt.speed / 3f;
                pbc.damage = bt.damage / 2;
                pbc.direction = Quaternion.AngleAxis(currentAngle, Vector3.forward) * this.transform.position + Vector3.forward;
                if (bt.debuff != null)
                    pbc.debuff = bt.debuff;
            }
            else
                Debug.LogError("Couldn't find Pierce Bullet Controller on Ice shard.");
            currentAngle += step;

            //Clamping, essentially
            if (temp.transform.position.x > 50f || temp.transform.position.y > 50f || temp.transform.position.z > 50f)
                Destroy(temp);
            if (temp.transform.position.x < -50f || temp.transform.position.y < -50f || temp.transform.position.z < -50f)
                Destroy(temp);
        }
    }
}
