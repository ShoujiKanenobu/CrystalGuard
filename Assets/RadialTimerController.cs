using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(SpriteRenderer))]
public class RadialTimerController : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private float currentCooldown;
    private float maxCooldown;
    private float percent;
    public void Update()
    {
        if (currentCooldown >= maxCooldown)
        {
            currentCooldown = maxCooldown;
            percent = 0;
            sr.material.SetFloat("_Arc1", 360f);
        }
        else
        {
            currentCooldown += Time.deltaTime;
            percent = currentCooldown / maxCooldown * 360f;
            sr.material.SetFloat("_Arc1", percent);
        }
    }

    public float GetCooldownPercent()
    {
        return percent;
    }

    public void SetCooldown(float cooldown)
    {
        currentCooldown = 0;
        maxCooldown = cooldown;
    }
}
