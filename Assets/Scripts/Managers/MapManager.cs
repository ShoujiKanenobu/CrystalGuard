using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [SerializeField]
    private Tilemap map;
    private Collider2D mapCollider;
    [SerializeField]
    private Tilemap interactableMap;
    [SerializeField]
    private Tile hoveredTile;
    [SerializeField]
    private List<TileData> datas;
    [SerializeField]
    private int CurrentUpgradeCost = 5;
    [SerializeField]
    private int UpgradeCostIncrease;
    [SerializeField]
    private int TowerLimit;
    [SerializeField]
    private int TrueTowerLimit;
    [SerializeField]
    private TextMeshProUGUI towersText;
    [SerializeField]
    private TextMeshProUGUI UpgradeText;

    private Dictionary<Vector2, TowerBase> towerPlacements = new Dictionary<Vector2, TowerBase>();
    private List<Vector3Int> lastingHighlights = new List<Vector3Int>();
    private List<Vector3Int> nonLastingHighlight = new List<Vector3Int>();

    private Dictionary<TileBase, TileData> dataFromTiles;

    [SerializeField]
    public Vector3 MousePositionWorld; 
    [SerializeField]
    public Vector3Int MousePositionGrid;

    #region utility
    public Tilemap GetMap() { return map; }
    public Collider2D GetMapCollider() { return mapCollider; }

    public bool isAtTowerLimit()
    {
        return towerPlacements.Count >= TowerLimit;
    }
    public TowerBase GetTowerAtMousePositionGrid()
    {
        return towerPlacements[(Vector2Int)MousePositionGrid];
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
    #endregion

    #region unityFunctions
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
        CurrentUpgradeCost = 5;
        TowerLimit = 5;
        UpdateUpgradeText();
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
    #endregion

    #region gameplay
    public void UpgradeTowerLimit()
    {

        if (GameManager.instance.state == GameState.RelicBuying || GameManager.instance.state == GameState.Paused) 
            return;

        if(GoldSystem.instance.SpendGold(CurrentUpgradeCost))
        {
            TowerLimit++;
            CurrentUpgradeCost += UpgradeCostIncrease;
            UpdateUpgradeText();
            UpdateTowerText();
        }

        if (TowerLimit == TrueTowerLimit)
            UpgradeText.transform.parent.gameObject.SetActive(false);
        else
            UpgradeText.transform.parent.gameObject.SetActive(true);
    }

    public void LoseTowerLimit()
    {
        TowerLimit--;
        UpdateTowerText();
    }
    public void ResetLevel()
    {

        foreach (var x in towerPlacements)
        {
            Destroy(x.Value.gameObject);
        }
        towerPlacements.Clear();

        CurrentUpgradeCost = 5;
        TowerLimit = 5;
        UpdateTowerText();
        UpdateUpgradeText();
    }
    public void PlaceTower(Vector2 pos, TowerBase tower)
    {
        pos.x = Mathf.Floor(pos.x);
        pos.y = Mathf.Floor(pos.y);
        towerPlacements.Add(pos, tower);
        UpdateTowerText();
    }

    public void RemoveTower(Vector2 pos)
    {
        pos.x = Mathf.Floor(pos.x);
        pos.y = Mathf.Floor(pos.y);
        towerPlacements.Remove(pos);
        UpdateTowerText();
    }
    #endregion

    #region highlights
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
        if (nonLastingHighlight.Count == 0)
            return;

        foreach (Vector3Int x in nonLastingHighlight)
        {
            if (!lastingHighlights.Contains(x))
                interactableMap.SetTile(x, null);
        }

        nonLastingHighlight.Clear();
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
    #endregion

    #region text
    private void UpdateTowerText()
    {
        towersText.text = "Towers: " + towerPlacements.Count + "/" + TowerLimit;
    }

    private void UpdateUpgradeText()
    {
        UpgradeText.text = CurrentUpgradeCost.ToString();
    }
    #endregion





    

    
}
