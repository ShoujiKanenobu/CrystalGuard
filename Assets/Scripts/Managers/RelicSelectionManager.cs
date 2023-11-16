using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RelicSelectionManager : MonoBehaviour
{
    [SerializeField]
    private RelicRandom random;

    public GameObject panel;

    public TextMeshProUGUI previewName;
    public TextMeshProUGUI previewDescription;

    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;
    public TextMeshProUGUI option3Text;
    public Image option1Image;
    public Image option2Image;
    public Image option3Image;

    private int lastSelection;

    private WeightedItem<Relic>[] currentItems = new WeightedItem<Relic>[3];

    public void Start()
    {
        random.RecalculateWeights();
        Init();
    }

    public void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        FillSlots();
        lastSelection = -1;
        previewName.text = "";
        previewDescription.text = "";
    }

    public void FillSlots()
    {
        currentItems[0] = random.RollForItem();
        currentItems[1] = random.RollForItem();
        currentItems[2] = random.RollForItem();

        if(currentItems[0].item == currentItems[1].item || 
            currentItems[1].item == currentItems[2].item || 
            currentItems[0].item == currentItems[2].item)
        {
            FillSlots();
        }

        UpdateChoiceUI();
    }

    private void UpdateChoiceUI()
    {
        option1Image.sprite = currentItems[0].item.sprite;
        option2Image.sprite = currentItems[1].item.sprite;
        option3Image.sprite = currentItems[2].item.sprite;
        option1Text.text = currentItems[0].item.name;
        option2Text.text = currentItems[1].item.name;
        option3Text.text = currentItems[2].item.name;
    }

    public void HandlePreview(int i)
    {
        if (lastSelection == i)
            PurchaseItem(i);
        else
        {
            lastSelection = i;

            previewDescription.text = currentItems[i].item.description;
            previewName.text = currentItems[i].item.name;
        }
    }

    private void PurchaseItem(int i)
    {
        RelicManager.instance.AddRelic(currentItems[i].item);
        panel.SetActive(false);
    }
}
