using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System;

public class GameFlowStateController : MonoBehaviour
{

    public List<GameObject> whitelistedUI;
    public Vector3Int selectedTile { get; private set; }
    private Vector3Int mergeTarget1;
    private Vector3Int mergeTarget2;

    private MapManager mapManager;
    private GameManager gameManager;

    private Vector3Int nullVector3Int = new Vector3Int(-999, -999, -999);

    public void Start()
    {
        mapManager = MapManager.instance;
        gameManager = GameManager.instance;
    }

    public void Update()
    {
        if (gameManager.state == GameState.Buying || gameManager.state == GameState.RelicBuying)
            return;

        if (Input.GetMouseButtonDown(0) && !isMouseOverWaveStart())
        {
            if(!mapManager.IsCurrentMousePosTileEmpty() && mapManager.IsBuildableAtTile(mapManager.MousePositionGrid))
            {
                selectedTile = mapManager.MousePositionGrid;
                mapManager.HighlightTile(selectedTile, false);
                gameManager.RequestStateChange(GameState.SelectedTower, false);
            }
            else if(gameManager.state != GameState.Buying)
            {
                gameManager.RequestStateChange(GameState.FreeHover, false);
            }
        }
    }

    //This is a hacky way to get around wave start build option bug.
    private bool isMouseOverWaveStart()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach(RaycastResult x in results)
        {
            if (whitelistedUI.Contains(x.gameObject))
                return true;
        }
        return false;
    }

    public void ReturnToFreeHover()
    {
        gameManager.RequestStateChange(GameState.FreeHover, false);
    }

    public void StartTowerSelection()
    {
        gameManager.RequestStateChange(GameState.Buying, true);
    }

    public void StartMerge()
    {
        mergeTarget1 = nullVector3Int;
        mergeTarget2 = nullVector3Int;
        gameManager.RequestStateChange(GameState.Merging, true);
    }

}
