using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TowerSelectionController : MonoBehaviour
{
    public int cost;
    public int rerollCost;
    public Color normalColor;
    public Color freezeColor;

    public GameObject previewObj;
    public GameObject infoText;
    public GameObject resourceUI;

    public GameObject towerInfoPanel;
    public TextMeshProUGUI towerInfoName;
    public TextMeshProUGUI towerInfoStats;

    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;
    public TextMeshProUGUI option3Text;
    public Image option1Image;
    public Image option2Image;
    public Image option3Image;
    public Image selfPanel;

    public GameFlowStateController boc;

    private bool frozen;

    public GORandom random;

    private WeightedItem<GameObject>[] currentItems = new WeightedItem<GameObject>[3];
    private bool isPreview;
    private int lastSelection;

    void Start()
    {
        frozen = false;
        random.RecalculateWeights();
        selfPanel.color = normalColor;
        lastSelection = -1;
        isPreview = false;
    }

    public void DisplayTowerAtPosition()
    {
        //PositionUIFromMouse();
        TowerBase tower = MapManager.instance.GetTowerAtMousePositionGrid();
        DisplayTowerInfo(tower.data[tower.level - 1]);
        towerInfoName.text = tower.name.Replace("(Clone)", "");
    }

    public void ResetPreview()
    {
        isPreview = false;
        towerInfoName.text = "";
        towerInfoStats.text = "";

        RectTransform resourceRect = resourceUI.GetComponent<RectTransform>();
        resourceRect.anchorMax = new Vector2(0, 1);
        resourceRect.anchorMin = new Vector2(0, 1);
        resourceRect.anchoredPosition = new Vector3(250, -150, 0);
    }

    public void ReRoll()
    {
        if (!GoldSystem.instance.SpendGold(rerollCost))
        {
            GameManager.instance.InsufficientGoldMessage();
            return;
        }

        if (frozen)
        {
            frozen = false;
            FillOptions();
            frozen = true;
        }
        else
            FillOptions();
    }

    public void FillOptions()
    {
        if (frozen)
        {
            FreezeOptions();
            return;
        }

        currentItems[0] = random.RollForItem();
        currentItems[1] = random.RollForItem();
        currentItems[2] = random.RollForItem();
        UpdateChoiceUI();

        ResetPreview();
    }
    
    public void PreviewOrBuild(int i)
    {
        if (i != lastSelection)
            isPreview = false;

        if (isPreview)
            PurchaseTower(i);
        else
        {
            isPreview = true;
            TowerBase d = currentItems[i].item.GetComponent<TowerBase>();
            towerInfoName.text = currentItems[i].item.name;
            DisplayTowerInfo(d.data[0]);
        }
        lastSelection = i;
    }

    private void DisplayTowerInfo(TowerDataBase d)
    {
        towerInfoStats.text = d.description;
        
        /*if (d.debuff != null)
            towerInfoStats.text += d.debuff.description + "\n";*/
    }

    public void PurchaseTower(int i)
    {
        previewObj.SetActive(false);
        if (!GoldSystem.instance.SpendGold(cost))
        {
            GameManager.instance.InsufficientGoldMessage();
            return;
        }

        TowerBenchController.instance.AddTowerToBench(currentItems[i]);

        currentItems[i] = random.RollForItem();
        UpdateChoiceUI();

        GameManager.instance.RequestStateChange(0, false);

        //State change handled by button
        isPreview = false;
        lastSelection = -1;

        ResetPreview();
    }

    public void FreezeOnCancel()
    {
        frozen = true;
    }

    public void FreezeOptions()
    {
        frozen = !frozen;
        if (frozen)
            selfPanel.color = freezeColor;
        else
            selfPanel.color = normalColor;

    }

    private void UpdateChoiceUI()
    {
        option1Image.sprite = currentItems[0].item.GetComponent<TowerBase>().data[0].shopIcon;
        option2Image.sprite = currentItems[1].item.GetComponent<TowerBase>().data[0].shopIcon;
        option3Image.sprite = currentItems[2].item.GetComponent<TowerBase>().data[0].shopIcon;
        option1Text.text = currentItems[0].item.name;
        option2Text.text = currentItems[1].item.name; 
        option3Text.text = currentItems[2].item.name;
    }


    //LOTS of hardcoding in this funciton. Yikes!
    public void PositionUIFromMouse()
    {
        RectTransform RT = selfPanel.GetComponent<RectTransform>();
        RectTransform resourceRect = resourceUI.GetComponent<RectTransform>();
        
        if (MapManager.instance.MousePositionGrid.y < 0)
        {
            RT.anchorMax = new Vector2(1, 1);
            RT.anchorMin = new Vector2(0, 1);
            RT.anchoredPosition = new Vector3(0, -225, 0);
            infoText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -415, 0);

            resourceRect.anchorMax = new Vector2(0, 0);
            resourceRect.anchorMin = new Vector2(0, 0);
            resourceRect.anchoredPosition = new Vector3(250, 150, 0);
            
        }
        else
        {
            RT.anchorMax = new Vector2(1,0);
            RT.anchorMin = new Vector2(0,0);
            RT.anchoredPosition = new Vector3(0, 225, 0);
            infoText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 415, 0);
            
            resourceRect.anchorMax = new Vector2(0, 1);
            resourceRect.anchorMin = new Vector2(0, 1);
            resourceRect.anchoredPosition = new Vector3(250, -150, 0);
        }
        PositionTowerInfoFromMouse();
    }

    private void PositionTowerInfoFromMouse()
    {
        RectTransform towerInfoRect = towerInfoPanel.GetComponent<RectTransform>();

        if (MapManager.instance.MousePositionGrid.y < 0)
        {
            towerInfoRect.anchoredPosition = new Vector3(330, 315, 0);
        }
        else
        {
            towerInfoRect.anchoredPosition = new Vector3(330, -315, 0);
        }
    }
}
