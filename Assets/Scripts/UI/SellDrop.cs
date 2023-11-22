using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellDrop : MonoBehaviour, IDropHandler
{
    public AudioPoolInfo sound;
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.TryGetComponent<BenchItemController>(out BenchItemController o);
        eventData.pointerDrag.TryGetComponent<TowerBase>(out TowerBase tb);

        TowerBase toSell = null;
        if (o != null && tb == null)
        {
            toSell = o.item.GetComponent<TowerBase>();
            GoldSystem.instance.GainGold(toSell.level);
            AudioSourceProvider.instance.PlayClipOnSource(sound);
            o.ClearSlot();
            TowerBenchController.instance.previewObj.SetActive(false);
        }
        else if (tb != null && o == null)
        {
            toSell = tb;
            GoldSystem.instance.GainGold(toSell.level);
            AudioSourceProvider.instance.PlayClipOnSource(sound);
            Destroy(toSell.gameObject);
            GameManager.instance.RequestStateChange(GameState.FreeHover, false);
            TowerBenchController.instance.previewObj.SetActive(false);
        }
        else
            Debug.LogError("Sell Drop data is both a BenchItemController and Towerbase!");
    }
}
