using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RelicSelectionManager : MonoBehaviour
{
    [SerializeField]
    private RelicRandom random;

    [SerializeField]
    public WeightedItemListRelic itemsClean;

    private List<WeightedItem<Relic>> availableItems;

    public GameObject panel;

    public TextMeshProUGUI rarityTitle;

    public TextMeshProUGUI previewName;
    public TextMeshProUGUI previewDescription;

    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;
    public TextMeshProUGUI option3Text;
    public Image option1Image;
    public Image option2Image;
    public Image option3Image;

    public Color commonColor;
    public Color uncommonColor;
    public Color rareColor;
    public Color legendaryColor;
    public Image option1Glow;
    public Image option2Glow;
    public Image option3Glow;

    private int lastSelection;
    private bool firstRoll;
    private WeightedItem<Relic>[] currentItems = new WeightedItem<Relic>[3];

    public AudioPoolInfo sound;

    public void Start()
    {
        availableItems = new List<WeightedItem<Relic>>(itemsClean.items);
        random.RecalculateWeights(availableItems);
        Init();
        firstRoll = true;
    }

    public void ResetAvailableItems()
    {
        availableItems = new List<WeightedItem<Relic>>(itemsClean.items);
        firstRoll = true;
    }

    private void Init()
    {
        
        FillSlots();
        lastSelection = -1;
        previewName.text = "";
        previewDescription.text = "";
    }

    private void ClearCurrentItems()
    {
        currentItems[0] = new WeightedItem<Relic>();
        currentItems[1] = new WeightedItem<Relic>();
        currentItems[2] = new WeightedItem<Relic>();
    }

    public void FillSlots()
    {
        ClearCurrentItems();

        RelicRarity rarity = RollRandomRarity();

        SetRarityText(rarity);
        
        WeightedItem<Relic> tempHold;
        List<WeightedItem<Relic>> blacklist = new List<WeightedItem<Relic>>(availableItems);
        tempHold = random.RarityRoll(availableItems, rarity);
        currentItems[0] = tempHold;
        blacklist.Remove(tempHold);

        tempHold = random.RarityRoll(blacklist, rarity);
        currentItems[1] = tempHold;
        blacklist.Remove(tempHold);

        tempHold = random.RarityRoll(blacklist, rarity);
        currentItems[2] = tempHold;

        SetRarityGlow(currentItems[0].item.rarity, option1Glow);
        SetRarityGlow(currentItems[1].item.rarity, option2Glow);
        SetRarityGlow(currentItems[2].item.rarity, option3Glow);

        UpdateChoiceUI();
    }

    private void SetRarityText(RelicRarity r)
    {
        switch(r)
        {
            case RelicRarity.common:
                rarityTitle.text = "Select a Common Power";
                break;
            case RelicRarity.uncommon:
                rarityTitle.text = "Select a Uncommon Power";
                break;
            case RelicRarity.rare:
                rarityTitle.text = "Select a Rare Power";
                break;
            case RelicRarity.legendary:
                rarityTitle.text = "Select a LEGENDARY Power";
                break;
        }
    }

    private void SetRarityGlow (RelicRarity r, Image option)
    {
        switch (r)
        {
            case RelicRarity.common:
                option.color = commonColor;
                break;
            case RelicRarity.uncommon:
                option.color = uncommonColor;
                break;
            case RelicRarity.rare:
                option.color = rareColor;
                break;
            case RelicRarity.legendary:
                option.color = legendaryColor;
                break;
        }
    }

    private RelicRarity RollRandomRarity()
    {
        int topRoll = 100;
        
        if(firstRoll)
        {
            if (PlayerPrefs.GetInt("WonLastGame") != 1)
            {
                topRoll = 75;
            }
            else
            {
                PlayerPrefs.SetInt("WonLastGame", 0);
            }
            firstRoll = false;
        }

        float x = Random.Range(0, topRoll);
        if (x <= 50)
            return RelicRarity.common;
        if (x <= 75)
            return RelicRarity.uncommon;
        if (x <= 90)
            return RelicRarity.rare;
        return RelicRarity.legendary;
    }

    private bool InCurrentItems(Relic r)
    {
        if (currentItems[0].item == r || currentItems[1].item == r || currentItems[2].item == r)
            return true;
        return false;
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
        AudioSourceProvider.instance.PlayClipOnSource(sound);
        RelicManager.instance.AddRelic(currentItems[i].item);
        availableItems.Remove(currentItems[i]);
        panel.SetActive(false);
        GameManager.instance.RequestStateChange(GameState.FreeHover, false);
    }

    public void ActivateRelicPanel()
    {
        GameManager.instance.RequestStateChange(GameState.RelicBuying, false);
        Init();
    }
}
