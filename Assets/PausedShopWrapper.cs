using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedShopWrapper : MonoBehaviour
{
    [SerializeField]
    private GameObject towerInfoPanel;

    [SerializeField]
    private TowerSelectionController tsc;
    public void ShowShop()
    {
        if (GameManager.instance.state == GameState.Paused)
            return;
        GameManager.instance.RequestStateChange(GameState.Buying, true);
        towerInfoPanel.SetActive(true);
        tsc.FillOptions();
    }

    public void HideShop()
    {
        if (GameManager.instance.state == GameState.Paused)
            return;
        GameManager.instance.RequestStateChange(GameState.FreeHover, false);
        towerInfoPanel.SetActive(false);
        tsc.FreezeOnCancel();
    }
}
