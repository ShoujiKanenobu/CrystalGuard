using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebuffInfo", menuName = "Debuff Info")]
public class DebuffInfo : ScriptableObject
{
    public StatusType type;
    public float debuffValue;
    public float duration;
    public string description;
    
}
