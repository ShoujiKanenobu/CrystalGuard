using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    //Map Data
    [SerializeField]
    private Tilemap map;
    private Collider2D mapCollider;
    [SerializeField]
    private Tilemap interactableMap;
    [SerializeField]
    private Tile hoveredTile;
    [SerializeField]
    private List<TileData> datas;

    private Dictionary<Vector2, TowerBase> towerPlacements = new Dictionary<Vector2, TowerBase>();
    private List<Vector3Int> lastingHighlights = new List<Vector3Int>();
    private List<Vector3Int> nonLastingHighlight = new List<Vector3Int>();

    private Dictionary<TileBase, TileData> dataFromTiles;

    [SerializeField]
    public Vector3 MousePositionWorld; //{ get; private set; }
    [SerializeField]
    public Vector3Int MousePositionGrid; // { get; private set; }

    public Tilemap GetMap() { return map; }
    public Collider2D GetMapCollider() { return mapCollider; }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;

        mapCollider = map.GetComponent<Collider2D>();
        if (mapCollider == null)
            Debug.LogError("Terrain Tilemap has no collider.");

        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (TileData data in datas)
        {
            foreach (TileBase tile in data.tiles)
            {
                dataFromTiles.Add(tile, data);
            }
        }
    }

    private void Update()
    {
        Vector3 mPos = new Vector3(-99,-99,-99);
        if (Input.mousePresent || Input.touchCount > 0)
            mPos = Input.mousePosition;

        MousePositionWorld = Camera.main.ScreenToWorldPoint(mPos);
        MousePositionGrid = map.WorldToCell(MousePositionWorld);

        ClearNonLastingTiles();
        HighlightMouseHover();
    }

    public TowerBase GetTowerAtMousePositionGrid()
    {
        return towerPlacements[(Vector2Int)MousePositionGrid];
    }

    public void ResetLevel()
    {
        foreach(var x in towerPlacements)
        {
            Destroy(x.Value.gameObject);
        }
        towerPlacements.Clear();
    }

    private void HighlightMouseHover()
    {
        if (GameManager.instance.state == GameState.FreeHover)
            HighlightTile(MousePositionGrid, false);
    }

    public void ClearLastingTiles()
    {
        if (lastingHighlights.Count == 0)
            return;

        foreach (Vector3Int x in lastingHighlights)
        {
            interactableMap.SetTile(x, null);
        }

        lastingHighlights.Clear();
    }

    public void ClearNonLastingTiles()
    {
        if(nonLastingHighlight.Count == 0)
            return;

        foreach (Vector3Int x in nonLastingHighlight)
        {
            if (!lastingHighlights.Contains(x))
                interactableMap.SetTile(x, null);
        }

        nonLastingHighlight.Clear();
    }

    internal void SellTargetTower(Vector2 selectedTile)
    {
        int level = towerPlacements[selectedTile].level;
        Destroy(towerPlacements[selectedTile].gameObject);
        towerPlacements.Remove(selectedTile);
        GoldSystem.instance.GainGold(level);
    }

    public void HighlightTile(Vector3Int position, bool lasting)
    {
        if (!IsBuildableAtTile(position) || position == null)
            return;

        if (lasting)
            lastingHighlights.Add(position);
        else
            nonLastingHighlight.Add(position);

        interactableMap.SetTile(position, hoveredTile);
    }

    public bool IsCurrentMousePosTileEmpty()
    {
        return !towerPlacements.ContainsKey(new Vector2(MousePositionGrid.x, MousePositionGrid.y));
    }

    public bool IsBuildableAtTile(Vector3Int pos)
    {
        if (map.GetTile(pos) == null)
            return false;
        return dataFromTiles[map.GetTile(pos)].isBuildable;
    }

    public void PlaceTower(Vector2 pos, TowerBase tower)
    {
        towerPlacements.Add(pos, tower);
    }

    public bool isMaxLevel(Vector2 pos)
    {
        return !(towerPlacements[pos].level < 3);
    }

    public void MergeTower(Vector2 pos, Vector2 consumedPos, Vector2 consumedPos2)
    {
        if (!towerPlacements.ContainsKey(consumedPos))
            return;

        if (!towerPlacements.ContainsKey(consumedPos2))
            return;

        if (!towerPlacements.ContainsKey(pos))
            return;

        if (towerPlacements[pos].GetTowerType() == towerPlacements[consumedPos].GetTowerType() &&
            towerPlacements[pos].GetTowerType() == towerPlacements[consumedPos2].GetTowerType())
        {
            
            towerPlacements[pos].increaseLevel();
            Destroy(towerPlacements[consumedPos].gameObject);
            Destroy(towerPlacements[consumedPos2].gameObject);
            towerPlacements.Remove(consumedPos);
            towerPlacements.Remove(consumedPos2);
        }
        else
        {
            StartCoroutine(GameManager.instance.TextForSeconds(2f, "Tower Types or levels don't match"));
        }
    }

}
