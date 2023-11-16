using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relics/Basic")]
public class Relic : ScriptableObject
{
    public Sprite sprite;
    public string description;

    public virtual void OnObtained() { }
    public virtual void OnRoundEnd() { }
    public virtual void OnLifeLost(Vector3 position) { }
    public virtual void OnEnemyKilled(Vector3 position) { }
    
}
