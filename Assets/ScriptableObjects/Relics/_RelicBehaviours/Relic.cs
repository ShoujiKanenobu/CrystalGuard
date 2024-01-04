using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RelicRarity { common, uncommon, rare, legendary }

[CreateAssetMenu(fileName = "New Relic", menuName = "Relics/Basic/NoBehaviour")]
public class Relic : ScriptableObject
{
    public Sprite sprite;
    public string description;
    public RelicRarity rarity;

    public virtual void OnObtained() { }
    public virtual void OnRoundEnd() { }
    public virtual void OnLifeLost(Vector3 position) { }
    public virtual void OnEnemyKilled(Vector3 position) { }
    
}
