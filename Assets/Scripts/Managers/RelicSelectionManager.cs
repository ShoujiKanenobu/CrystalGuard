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

    private Relic[] currentItems = new Relic[3];

    public AudioPoolInfo sound;

    public void Start()
    {
        availableItems = new List<WeightedItem<Relic>>(itemsClean.items);
        random.RecalculateWeights(availableItems);
        Init();
    }

    public void ResetAvailableItems()
    {
        availableItems = new List<WeightedItem<Relic>>(itemsClean.items);
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
        currentItems[0] = null;
        currentItems[1] = null;
        currentItems[2] = null;
    }

    public void FillSlots()
    {
        ClearCurrentItems();

        RelicRarity rarity = RollRandomRarity();

        SetRarityText(rarity);
        SetRarityGlow(rarity);
        WeightedItem<Relic> tempHold;

        tempHold = random.RarityRoll(availableItems, rarity);
        currentItems[0] = tempHold.item;
        availableItems.Remove(tempHold);

        tempHold = random.RarityRoll(availableItems, rarity);
        currentItems[1] = tempHold.item;
        availableItems.Remove(tempHold);

        tempHold = random.RarityRoll(availableItems, rarity);
        currentItems[2] = tempHold.item;
        availableItems.Remove(tempHold);

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

    private void SetRarityGlow (RelicRarity r)
    {
        switch (r)
        {
            case RelicRarity.common:
                option1Glow.color = commonColor;
                option2Glow.color = commonColor;
                option3Glow.color = commonColor;
                break;
            case RelicRarity.uncommon:
                option1Glow.color = uncommonColor;
                option2Glow.color = uncommonColor;
                option3Glow.color = uncommonColor;
                break;
            case RelicRarity.rare:
                option1Glow.color = rareColor;
                option2Glow.color = rareColor;
                option3Glow.color = rareColor;
                break;
            case RelicRarity.legendary:
                option1Glow.color = legendaryColor;
                option2Glow.color = legendaryColor;
                option3Glow.color = legendaryColor;
                break;
        }
    }

    private RelicRarity RollRandomRarity()
    {
        float x = Random.Range(0, 100);
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
        if (currentItems[0] == r || currentItems[1] == r || currentItems[2] == r)
            return true;
        return false;
    }

    private void UpdateChoiceUI()
    {
        option1Image.sprite = currentItems[0].sprite;
        option2Image.sprite = currentItems[1].sprite;
        option3Image.sprite = currentItems[2].sprite;
        option1Text.text = currentItems[0].name;
        option2Text.text = currentItems[1].name;
        option3Text.text = currentItems[2].name;
    }

    public void HandlePreview(int i)
    {
        if (lastSelection == i)
            PurchaseItem(i);
        else
        {
            lastSelection = i;

            previewDescription.text = currentItems[i].description;
            previewName.text = currentItems[i].name;
        }
    }

    private void PurchaseItem(int i)
    {
        AudioSourceProvider.instance.PlayClipOnSource(sound);
        RelicManager.instance.AddRelic(currentItems[i]);
        panel.SetActive(false);
        GameManager.instance.RequestStateChange(GameState.FreeHover, false);
    }

    public void ActivateRelicPanel()
    {
        GameManager.instance.RequestStateChange(GameState.RelicBuying, false);
        Init();
    }
}
