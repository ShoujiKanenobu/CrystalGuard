using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EnemySpawnManager : MonoBehaviour
{
    public GameObject waveStartButton;
    public TextMeshProUGUI waveText;
    public GORuntimeSet enemySet;

    public Toggle AutoPlayToggle;

    public Vector3 SpawnTilePoint;
    public List<Vector3> Path;
    public GameObject enemyPrefab;
    public float disableDelay;

    public float bufferZone;
    public float loopModifierStepAmount;
   
    public EnemyWaveStats waveStats;

    private float loopMultiplier;

    private bool AccountedForWave;
    private bool waveSpawning;
    private float lastSpawn;
    private float timeBetweenSpawns;
    private int waveNumber;
    private int spawnCount;

    public void Start()
    {
        waveSpawning = false;
        waveStartButton.SetActive(true);
        waveNumber = 0;
        timeBetweenSpawns = 0;
        spawnCount = 0;
        AccountedForWave = true;
        loopMultiplier = 1;
    }

    void Update()
    {
        if(Time.time - lastSpawn > timeBetweenSpawns && spawnCount > 0 && waveSpawning)
        {
            lastSpawn = Time.time;
            SpawnEnemy();
        }
        if (enemySet.Items.Count == 0 && !AccountedForWave && spawnCount <= 0)
        {
            GoldSystem.instance.GainGold(waveStats.waves[waveNumber % waveStats.waves.Count].reward);
            waveSpawning = false;
            waveStartButton.SetActive(true);
            waveNumber++;
            if (waveNumber > waveStats.waves.Count - 1)
            {
                if(waveNumber == waveStats.waves.Count)
                    StartCoroutine(GameManager.instance.TextForSeconds(3, "You beat all 10 Waves! Lets go to endless mode...."));
                loopMultiplier += loopModifierStepAmount;
            }
            AccountedForWave = true;
        }

        if (waveStartButton.activeSelf == true && AutoPlayToggle.isOn)
            LoadNewWave();
    }

    public void ResetLevel()
    {
        waveSpawning = false;
        waveStartButton.SetActive(true);
        waveNumber = 0;
        timeBetweenSpawns = 0;
        spawnCount = 0;
        AccountedForWave = true;
        loopMultiplier = 1;
        while(enemySet.Items.Count > 0)
        {
            enemySet.Items[0].SetActive(false);
        }
    }

    public void LoadNewWave()
    {
        if (GameManager.instance.state == GameState.GameOver)
            return;

        int waveCount = waveNumber % waveStats.waves.Count;
        spawnCount = (int)(waveStats.waves[waveCount].spawnCount * loopMultiplier);
        timeBetweenSpawns = waveStats.waves[waveCount].timeBetweenSpawns;

        waveText.text = "Wave: " + (waveNumber + 1);

        waveSpawning = true;
        StartCoroutine(DisableButtonAfterDelay());
        AccountedForWave = false;
    }

    private void SpawnEnemy()
    {
        GameObject newEnemy = EnemyPool.instance.GetPooledObject();
        if(newEnemy != null)
        {
            SetUpEnemy(newEnemy);
            newEnemy.SetActive(true);
        }
        else
        {
            newEnemy = Instantiate(enemyPrefab, new Vector3(-99, -99, 0), Quaternion.identity);
            SetUpEnemy(newEnemy);
            EnemyPool.instance.ExpandPool(newEnemy);
        }
        spawnCount--;
    }

    private void SetUpEnemy(GameObject newEnemy)
    {
        int waveCount = waveNumber % waveStats.waves.Count;
        newEnemy.transform.position = SpawnTilePoint + new Vector3(Random.Range(0f + bufferZone, 1f - bufferZone), Random.Range(0f + bufferZone, 1f - bufferZone));
        newEnemy.transform.rotation = Quaternion.Euler(0, 0, 0);
        newEnemy.GetComponent<EnemyMovementController>().Init(new List<Vector3>(Path), waveStats.waves[waveCount].speed);
        newEnemy.GetComponent<EnemyHealthController>().Init((int)(waveStats.waves[waveCount].HP * loopMultiplier), waveStats.waves[waveCount].livesDamage);
    }

    private IEnumerator DisableButtonAfterDelay()
    {
        yield return new WaitForSeconds(disableDelay);
        waveStartButton.SetActive(false);
    }
}
