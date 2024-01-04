using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemyMovementController : MonoBehaviour
{
    public GORuntimeSet enemySet;
    public EnemyHealthController healthController;

    public List<Vector3> targetPositions;
    private int currentTargetIndex = 0;
    [SerializeField] private float minDistFromTarget;
    public float speed = 1.0f;
    public float baseSpeed;

    public float distanceTraveled;

    private Vector3 offset;
    private Vector3 lastPos;

    [SerializeField]
    private Relic distanceRelic;
    private float relicTravelAmount;
    [SerializeField]
    private float relicDistanceTillDamage;
    public void Init(List<Vector3> Pathing, float speed)
    {
        speed = speed - RelicBonusStatTracker.instance.EnemySlow;
        this.speed = speed;
        baseSpeed = speed;
        targetPositions = Pathing;
        relicTravelAmount = 0;
    }

    private void OnEnable()
    {
        enemySet.Add(this.gameObject);
        currentTargetIndex = 0;
        distanceTraveled = 0;

        offset.x = this.transform.position.x % 1;
        offset.y = -this.transform.position.y % 1;
        offset.z = 0;

        for (int i = 0; i < targetPositions.Count; i++)
        {
            targetPositions[i] -= offset;
        }

        lastPos = this.transform.position;
    }

    private void OnDisable()
    {
        enemySet.Remove(this.gameObject);
    }

    void Update()
    {
        Vector3 travelDist = transform.position - lastPos;
        distanceTraveled += travelDist.magnitude;
        lastPos = this.transform.position;

        //Ground Thorns Relic Behaviour
        if(RelicManager.instance.ContainsRelic(distanceRelic))
        {
            relicTravelAmount += travelDist.magnitude;
            if (relicTravelAmount >= relicDistanceTillDamage)
            {
                healthController.TakeDamage(1 + Mathf.CeilToInt(speed));
                relicTravelAmount = 0;
            }
        }

        if(Vector3.Distance(this.transform.position, targetPositions[currentTargetIndex]) < minDistFromTarget)
        {
            currentTargetIndex++;
            if (currentTargetIndex > targetPositions.Count - 1) {
                healthController.TakeLives();
            }
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, targetPositions[currentTargetIndex], speed * Time.deltaTime);
    }

    
}
