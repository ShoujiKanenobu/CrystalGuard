using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TowerSelectionController : MonoBehaviour
{
    public int cost;
    public int rerollCost;
    public Color normalColor;
    public Color freezeColor;

    public GameObject previewObj;
    public GameObject InfoText;

    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;
    public TextMeshProUGUI option3Text;
    public Image option1Image;
    public Image option2Image;
    public Image option3Image;
    public Image selfPanel;

    public GameFlowStateController boc;

    private bool frozen;

    public WeightedRandom random;

    private WeightedItem[] currentItems = new WeightedItem[3];
    private bool isPreview;
    private int lastSelection;
    // Start is called before the first frame update
    void Start()
    {
        frozen = false;
        random.RecalculateWeights();
        selfPanel.color = normalColor;
        lastSelection = -1;
        isPreview = false;
    }

    public void ReRoll()
    {
        if (!GoldSystem.instance.SpendGold(rerollCost))
        {
            GameManager.instance.InsufficientGoldMessage();
            return;
        }
            
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
    }

    public void PreviewOrBuild(int i)
    {
        if (i != lastSelection)
            isPreview = false;

        if (isPreview)
            BuildOption(i);
        else
        {
            isPreview = true;
            previewObj.SetActive(true);
            previewObj.transform.position = boc.selectedTile + new Vector3(0.5f, 0.5f, 0);
            //Yikes
            previewObj.GetComponent<TowerRangeIndicator>().ShowRadius(currentItems[i].item.GetComponent<TowerBase>().data[0].range);
            Color tempColor = currentItems[i].item.GetComponent<TowerBase>().data[0].towerColor;
            tempColor.a = 0.6f;
            previewObj.GetComponent<SpriteRenderer>().color = tempColor;
        }
        lastSelection = i;
    }

    public void BuildOption(int i)
    {
        previewObj.SetActive(false);
        if (!GoldSystem.instance.SpendGold(cost))
        {
            GameManager.instance.InsufficientGoldMessage();
            return;
        }
            
        Vector3 towerGridPos = boc.selectedTile;

        TowerBase temp = Instantiate(currentItems[i].item, 
            towerGridPos + new Vector3(0.5f, 0.5f, 0), 
            Quaternion.identity).GetComponent<TowerBase>();

        MapManager.instance.PlaceTower(towerGridPos, temp);

        currentItems[i] = random.RollForItem();
        UpdateChoiceUI();

        GameManager.instance.RequestStateChange(0, false);

        //State change handled by button
        this.gameObject.SetActive(false);
        isPreview = false;
        lastSelection = -1;
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
        option1Image.sprite = currentItems[0].itemImage;
        option2Image.sprite = currentItems[1].itemImage;
        option3Image.sprite = currentItems[2].itemImage;
        option1Text.text = currentItems[0].item.name;
        option2Text.text = currentItems[1].item.name;
        option3Text.text = currentItems[2].item.name;
    }


    //LOTS of hardcoding in this funciton. Yikes!
    public void PositionFromMouse()
    {
        RectTransform RT = this.GetComponent<RectTransform>();
        if (MapManager.instance.MousePositionGrid.y < 0)
        {
            RT.anchorMax = new Vector2(1, 1);
            RT.anchorMin = new Vector2(0, 1);
            RT.anchoredPosition = new Vector3(0, -225, 0);
            InfoText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -415, 0);
        }
        else
        {
            RT.anchorMax = new Vector2(1,0);
            RT.anchorMin = new Vector2(0,0);
            RT.anchoredPosition = new Vector3(0, 225, 0);
            InfoText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 415, 0);
        }
    }
}
