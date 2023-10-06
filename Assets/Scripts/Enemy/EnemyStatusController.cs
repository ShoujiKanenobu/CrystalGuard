using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum StatusType
{
    Speed = 0, AttackSpeed = 1, DamageAmp = 2
}

public struct StatusInfo
{
    public float value;
    public float endTime;

    public StatusInfo(float v, float duration)
    {
        value = v;
        endTime = Time.time + duration;
    }
}
[RequireComponent(typeof(EnemyStatusVisualizer))]
[RequireComponent(typeof(EnemyHealthController))]
[RequireComponent(typeof(EnemyMovementController))]
public class EnemyStatusController : MonoBehaviour
{
    private EnemyHealthController healthController;
    private EnemyMovementController movementController;
    private EnemyStatusVisualizer visualizer;

    private Dictionary<StatusType, StatusInfo> effects = new Dictionary<StatusType, StatusInfo>();
    Dictionary<StatusType, Action<float>> ApplyDict = new Dictionary<StatusType, Action<float>>();
    Dictionary<StatusType, Action> ResetDict = new Dictionary<StatusType, Action>();

    private List<StatusType> removeList = new List<StatusType>();
    // Start is called before the first frame update
    void Start()
    {
        visualizer = GetComponent<EnemyStatusVisualizer>();
        healthController = GetComponent<EnemyHealthController>();
        movementController = GetComponent<EnemyMovementController>();
        ApplyDict.Add(StatusType.Speed, ApplySpeed);
        ApplyDict.Add(StatusType.DamageAmp, ApplyDamageAmp);
        ResetDict.Add(StatusType.Speed, ResetSpeed);
        ResetDict.Add(StatusType.DamageAmp, ResetDamageAmp);
    }

    private void OnEnable()
    {
        effects.Clear();
    }

    private void Update()
    {
        RemoveExpiredEffects();
        ApplyAllEffects();
    }

    private void ApplyAllEffects()
    {
        foreach(var t in effects)
        {
            if (!ApplyDict.ContainsKey(t.Key))
                continue;
            var apply = ApplyDict[t.Key];
            apply(t.Value.value);
        }
    }

    private void RemoveExpiredEffects()
    {
        removeList.Clear();
        foreach (var t in effects)
        {
            if (Time.time >= t.Value.endTime)
            {
                removeList.Add(t.Key);
            }
        }

        foreach (var x in removeList)
        {
            var reset = ResetDict[x];
            reset();
            visualizer.RemoveVisual(x);
            effects.Remove(x);
        }
    }
    public void ApplyStatusEffect(DebuffInfo d)    {
        ApplyStatusEffect(d.type, d.debuffValue, d.duration);
    }

    public void ApplyStatusEffect(StatusType t, float amount, float duration)
    {
        if(effects.ContainsKey(t))
        {
            if (effects[t].value < amount)
                effects[t] = new StatusInfo(amount, duration);
            else
                effects[t] = new StatusInfo(effects[t].value, duration);
        }
        else
        {
            effects.Add(t, new StatusInfo(amount, duration));
            visualizer.ApplyVisual(t);
        }
    }

    private void ApplySpeed(float value)
    {
        movementController.speed = movementController.baseSpeed * value;
    }
    private void ResetSpeed()
    {
        movementController.speed = movementController.baseSpeed;
    }
    private void ApplyDamageAmp(float value)
    {
        healthController.DamageAmp = 1f + value;
    }
    private void ResetDamageAmp()
    {
        healthController.DamageAmp = 1f;
    }
}
