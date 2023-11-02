using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public enum GameState
{
    FreeHover = 0, BuildOptionHover = 1, SelectedTileNoTower = 2, SelectedTower = 3, Merging = 4, GameOver = 5, Reset = 6
}

public class GameManager : MonoBehaviour
{
    public float dotTickRate;
    public int startingLives;
    public static GameManager instance;
    [SerializeField]
    public GameState state { get; private set; }

    public TextMeshProUGUI infoText;
    public TextMeshProUGUI livesText;

    public TowerSelectionController tsc;

    [SerializeField]
    private float flashDuration;
    [SerializeField]
    private SpriteRenderer shrineSR;
    [SerializeField]
    private Material flashMaterial;
    private Material originalMaterial;
    private Coroutine flashRoutine;

    private int Lives;

    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
        Lives = startingLives;
        livesText.text = "Lives: " + Lives;
        originalMaterial = shrineSR.material;
    }

    public void RequestStateChange(GameState g, bool retainHighlights)
    {
        //Lockout if game over
        if (state == GameState.GameOver && g != GameState.Reset)
            return;

        state = g;
        if(!retainHighlights)
            MapManager.instance.ClearLastingTiles();

        if (g == GameState.SelectedTower)
            tsc.DisplayTowerAtPosition();
        if (g == GameState.FreeHover)
            tsc.ResetPreview();

        if (state == GameState.Reset)
        {
            Lives = startingLives;
            livesText.text = "Lives: " + Lives;
            state = GameState.FreeHover;
        }
            
    }
    
    public void RequestStateChangeUpCast(int i)
    {
        RequestStateChange((GameState)i, false);
    }

    public IEnumerator TextForSeconds(float duration, string text)
    {
        infoText.text = text;
        infoText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        infoText.gameObject.SetActive(false);
    }

    public IEnumerator FlashDamage(float duration)
    {
        shrineSR.material = flashMaterial;

        yield return new WaitForSeconds(duration);

        shrineSR.material = originalMaterial;
        flashRoutine = null;
    }

    public bool hasLives()
    {
        return Lives > 0;
    }

    public void InsufficientGoldMessage()
    {
        StartCoroutine(GameManager.instance.TextForSeconds(1f, "Not Enough Gold!"));
    }

    public void FullBenchMessage()
    {
        StartCoroutine(GameManager.instance.TextForSeconds(1f, "Bench is full!"));
    }

    public void FullTowersMessage()
    {
        StartCoroutine(GameManager.instance.TextForSeconds(1f, "Tower Cap is full!"));
    }

    public void LoseLife(int i)
    {
        Lives -= i;
        livesText.text = "Lives: " + Lives;

        if (flashRoutine != null)
            StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashDamage(flashDuration));

        if(Lives <= 0)
        {
            RequestStateChange(GameState.GameOver, false);
            Debug.Log("Game over!");
        }
    }
}
