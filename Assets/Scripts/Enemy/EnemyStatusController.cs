using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum StatusType
{
    Speed = 0, AttackSpeed = 1, DamageAmp = 2, Poison = 3, Darkness = 4, Bleed = 5
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
    [SerializeField]
    private GameObject darkHeraldPop;
    [SerializeField]
    private Relic darkHeraldRelic;

    private EnemyHealthController healthController;
    private EnemyMovementController movementController;
    private EnemyStatusVisualizer visualizer;

    private Dictionary<StatusType, StatusInfo> effects = new Dictionary<StatusType, StatusInfo>();
    Dictionary<StatusType, Action<float>> ApplyDict = new Dictionary<StatusType, Action<float>>();
    Dictionary<StatusType, Action> ResetDict = new Dictionary<StatusType, Action>();

    private List<StatusType> removeList = new List<StatusType>();

    private float lastDOTTick;

    private float poison;
    private float bleed;
    private float darkness;
    // Start is called before the first frame update
    void Awake()
    {
        visualizer = GetComponent<EnemyStatusVisualizer>();
        healthController = GetComponent<EnemyHealthController>();
        movementController = GetComponent<EnemyMovementController>();
        ApplyDict.Add(StatusType.Speed, ApplySpeed);
        ResetDict.Add(StatusType.Speed, ResetSpeed);

        ApplyDict.Add(StatusType.DamageAmp, ApplyDamageAmp);
        ResetDict.Add(StatusType.DamageAmp, ResetDamageAmp);

        ApplyDict.Add(StatusType.Poison, ApplyPoison);
        ResetDict.Add(StatusType.Poison, ResetPoison);

        ApplyDict.Add(StatusType.Bleed, ApplyBleed);
        ResetDict.Add(StatusType.Bleed, ResetBleed);

        ApplyDict.Add(StatusType.Darkness, ApplyDarkness);
        ResetDict.Add(StatusType.Darkness, ResetDarkness);
    }

    private void OnEnable()
    {
        effects.Clear();
        bleed = 0;
        poison = 0;
        darkness = 0;
    }

    private void Update()
    {
        if(lastDOTTick <= Time.time)
        {
            lastDOTTick = Time.time + GameManager.instance.dotTickRate;
            ApplyDOTS();
        }
        RemoveExpiredEffects();
        ApplyAllEffects();
    }

    private void ApplyDOTS()
    {
        if(poison > 0)
            healthController.TakeDamage((int)poison);
        if(bleed > 0)
            healthController.TakeDamage((int)bleed);
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
            else if(duration + Time.time > effects[t].endTime)
                effects[t] = new StatusInfo(effects[t].value, duration);
        }
        else
        {
            effects.Add(t, new StatusInfo(amount, duration));
            visualizer.ApplyVisual(t);
        }
        if (t == StatusType.Darkness)
            darkness += amount;
    }

    #region speed
    private void ApplySpeed(float value)
    {
        movementController.speed = movementController.baseSpeed * value;
    }
    private void ResetSpeed()
    {
        movementController.speed = movementController.baseSpeed;
    }
    #endregion
    #region damage amp
    private void ApplyDamageAmp(float value)
    {
        healthController.DamageAmp = Mathf.Max(value, healthController.DamageAmp);
    }
    private void ResetDamageAmp()
    {
        healthController.DamageAmp = 1f;
    }
    #endregion
    #region poison
    private void ApplyPoison(float value)
    {
        poison = value;
    }
    private void ResetPoison()
    {
        poison = 0;
    }
    #endregion
    #region bleed
    private void ApplyBleed(float value)
    {
        bleed = value;
    }
    private void ResetBleed()
    {
        bleed = 0;
    }
    #endregion
    #region darkness
    private void ApplyDarkness(float value)
    {
        if (darkness >= 50)
        {
            if (RelicManager.instance.ContainsRelic(darkHeraldRelic))
            {
                DarkHeraldPop();
            }
            else
            {
                healthController.TakeDamage((int)darkness);
            }
            darkness = 0;
        }
    }
    private void ResetDarkness()
    {
        if (RelicManager.instance.ContainsRelic(darkHeraldRelic))
        {
            DarkHeraldPop();
        }
        else
        {
            healthController.TakeDamage((int)darkness);
        }
        darkness = 0;
    }

    public void DarkHeraldPop()
    {
        GameObject aoeGO = Instantiate(darkHeraldPop, this.gameObject.transform.position, Quaternion.identity);

        AOEController aoe = aoeGO.GetComponent<AOEController>();
        aoe.animFinishTime = 0f;
        aoe.delay = 0f;
        aoe.radius = 0.5f;
        aoe.damage = (int)darkness;
        aoe.duration = 0.25f;
        aoe.Init();
    }
    #endregion

}
