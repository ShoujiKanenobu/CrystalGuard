using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NovaTowerData", menuName = "TowerData/NovaTowerData")]
public class NovaTowerData : TowerDataBase
{
    public float rotationSpeed;
    public float expandSpeed;
    public Color novaColor;
    public GameObject nova;
    public Sprite novaSprite;
}
