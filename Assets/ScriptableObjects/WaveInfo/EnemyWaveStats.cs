using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaveInfo
{
    public float timeBetweenSpawns;
    public int HP;
    public float speed;
    public int spawnCount;
    public int livesDamage;
    public int reward;
}

[CreateAssetMenu(fileName = "EnemyWaveStats" , menuName = "Enemy Wave Stat")]
public class EnemyWaveStats : ScriptableObject
{
    public List<WaveInfo> waves; 
}
