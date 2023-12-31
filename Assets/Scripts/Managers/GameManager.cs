using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
public enum GameState
{
    FreeHover = 0, BuildOptionHover = 1, Buying = 2, SelectedTower = 3, Merging = 4, GameOver = 5, Reset = 6, RelicBuying = 7, Paused = 8
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string prefsName;

    public float dotTickRate;
    public int startingLives;
    public static GameManager instance;
    [SerializeField]
    public GameState state { get; private set; }

    public TextMeshProUGUI infoText;
    public TextMeshProUGUI livesText;

    public TowerSelectionController tsc;

    public GameEvent LifeLostEvent;

    [SerializeField]
    private GORuntimeSet enemySet;

    [SerializeField]
    private float flashDuration;
    [SerializeField]
    private SpriteRenderer shrineSR;
    [SerializeField]
    private Material flashMaterial;
    private Material originalMaterial;
    private Coroutine flashRoutine;

    private int Lives;

    [SerializeField]
    private AudioPoolInfo loseLifeSound;

    [SerializeField]
    private Relic glancingBlow;

    private bool firstLoss;

    private bool isPaused;
    private GameState lastState;

    [SerializeField]
    private GameObject VictoryScreen;
    [SerializeField]
    private GameObject LazyPauseButtonFix;

    public bool leftMousePressed;
    public bool rightMousePressed;

    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
        Lives = startingLives;
        livesText.text = "Lives: " + Lives;
        originalMaterial = shrineSR.material;
        state = GameState.RelicBuying;
        resetFirstLoss();
        isPaused = false;
    }

    public void RequestStateChange(GameState g, bool retainHighlights)
    {
        //Lockout if game over
        if (state == GameState.GameOver && g != GameState.Reset)
            return;

        if (isPaused == true)
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
            state = GameState.RelicBuying;
        }
            
    }
    
    public void WavesComplete()
    {
        TogglePause(false);
        VictoryScreen.SetActive(true);
        LazyPauseButtonFix.SetActive(false);
        PlayerPrefs.SetInt(prefsName, 1);
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

    public bool isEnemiesAlive()
    {
        if (enemySet.Items.Count == 0)
            return false;
        return true;
    }

    public bool hasLives()
    {
        return Lives > 0;
    }

    public int GetLives()
    {
        return Lives;
    }

    public bool isGamePaused()
    {
        return isPaused;
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

    public void resetFirstLoss()
    {
        firstLoss = false;
    }

    public void LoseLife(int i)
    {
        if(RelicManager.instance.ContainsRelic(glancingBlow) && !firstLoss)
        {
            firstLoss = true;
        }
        else
        {
            Lives -= i;
            livesText.text = "Lives: " + Lives;
        }

        LifeLostEvent.Raise();
        LifeLostEvent.Raise(shrineSR.transform.position);

        if (flashRoutine != null)
            StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashDamage(flashDuration));

        AudioSourceProvider.instance.PlayClipOnSource(loseLifeSound);

        if(Lives <= 0)
        {
            LazyPauseButtonFix.SetActive(false);
            RequestStateChange(GameState.GameOver, false);
        }
    }

    public void GainLife(int i)
    {
        if (state == GameState.GameOver)
            return;

        Lives += i;
        livesText.text = "Lives: " + Lives;
    }

    public void TogglePause(bool pause)
    {
        if(!pause)
        {
            if(isPaused == false)
                lastState = state;
            RequestStateChange(GameState.Paused, true);
            isPaused = true;
            Time.timeScale = 0;
        }
        else
        {
            isPaused = false;
            RequestStateChange(lastState, true);
            Time.timeScale = 1f;
        }
    }
}
