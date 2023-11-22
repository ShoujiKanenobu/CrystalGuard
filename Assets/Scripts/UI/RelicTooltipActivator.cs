using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class RelicTooltipActivator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Relic relic;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (relic == null)
            Debug.Log("Relic not set on Relic Icon");
        else
            HoverTooltip.instance.ActivateTooltip(relic.name + ": " + relic.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverTooltip.instance.DeactivateTooltip();
    }

}
