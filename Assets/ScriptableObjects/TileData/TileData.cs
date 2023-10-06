using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileData", menuName = "NewTileData")]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;

    public bool isBuildable;
}
