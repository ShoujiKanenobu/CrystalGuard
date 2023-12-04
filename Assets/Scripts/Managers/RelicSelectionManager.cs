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

    private Relic[] currentItems = new Relic[3];

    public AudioPoolInfo sound;

    public void Start()
    {
        random.RecalculateWeights();
        Init();
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

        List<Relic> blacklist = new List<Relic>(RelicManager.instance.obtainedRelics);
        currentItems[0] = random.RollforUniqueItem(blacklist).item;
        blacklist.Add(currentItems[0]);
        currentItems[1] = random.RollforUniqueItem(blacklist).item;
        blacklist.Add(currentItems[1]);
        currentItems[2] = random.RollforUniqueItem(blacklist).item;

        UpdateChoiceUI();
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
