using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public HPBarController barUI;

    private int HP;
    private int maxHP;
    private int livesDamage;
    public float DamageAmp;

    public void Init(int HP, int livesDamage)
    {
        this.HP = HP;
        this.maxHP = HP;
        this.livesDamage = livesDamage;
    }

    private void OnEnable()
    {
        barUI.gameObject.SetActive(false);
        DamageAmp = 1;
    }

    public void TakeDamage(int damage)
    {
        if (barUI.gameObject.activeSelf == false)
            barUI.gameObject.SetActive(true);

        HP -= (int)(damage * DamageAmp);
        if (HP <= 0)
            Die();

        barUI.SetBarHP(HP, maxHP);
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
    public void TakeLives()
    {
        GameManager.instance.LoseLife(livesDamage);
        gameObject.SetActive(false);
    }
}
