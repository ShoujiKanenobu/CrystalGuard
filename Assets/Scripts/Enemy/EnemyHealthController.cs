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

    private SpriteRenderer sr;
    [SerializeField]
    private Material flashMaterial;
    [SerializeField]
    private Material originalMaterial;
    private Coroutine flashRoutine;

    [SerializeField]
    private GameEvent darkheraldEvent;
    [SerializeField]
    private Relic darkheraldRelic;
    [SerializeField]
    private GameEvent EnemyDeathEvent;

    [SerializeField]
    private Relic cullingStrikeRelic;
    [SerializeField]
    private GameObject cullEffect;
    public void Init(int HP, int livesDamage)
    {
        if(sr == null)
            sr = GetComponent<SpriteRenderer>();
        this.HP = HP;
        this.maxHP = HP;
        this.livesDamage = livesDamage;
    }

    private void OnEnable()
    {
        barUI.gameObject.SetActive(false);
        DamageAmp = 1;
    }

    private void OnDisable()
    {
        if(flashRoutine != null)
            StopCoroutine(flashRoutine);
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
        sr.material = originalMaterial;
    }

    public void TakeDamage(int damage)
    {
        if (barUI.gameObject.activeSelf == false)
            barUI.gameObject.SetActive(true);

        HP -= (int)(damage * DamageAmp);

        if (flashRoutine != null)
            StopCoroutine(flashRoutine);
        if(this.gameObject.activeSelf)
            flashRoutine = StartCoroutine(FlashDamage(0.1f));

        if (HP <= 0)
            Die();

        if (RelicManager.instance.ContainsRelic(cullingStrikeRelic) && (float)HP / (float)maxHP <= 0.1f)
        {
            Instantiate(cullEffect, this.transform.position, Quaternion.identity);
            Die();
        }
        

        barUI.SetBarHP(HP, maxHP);
    }

    public void Die()
    {
        gameObject.SetActive(false);
        if (RelicManager.instance.ContainsRelic(darkheraldRelic))
        {
            this.GetComponent<EnemyStatusController>().DarkHeraldPop();
        }
        EnemyDeathEvent.Raise(this.transform.position);
        EnemyDeathEvent.Raise();
    }
    public void TakeLives()
    {
        GameManager.instance.LoseLife(livesDamage);
        gameObject.SetActive(false);
    }

    public IEnumerator FlashDamage(float duration)
    {
        sr.material = flashMaterial;

        yield return new WaitForSeconds(duration);

        sr.material = originalMaterial;
        flashRoutine = null;
    }
}
