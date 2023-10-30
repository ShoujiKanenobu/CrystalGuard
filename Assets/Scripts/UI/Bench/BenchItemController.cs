using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class BenchItemController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public GameObject HeldItem = null;
    private GameObject pooledItem = null;
    private Image img;
    [SerializeField]
    private Color defaultColor;

    private GameObject previewObj;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        img.color = defaultColor;
    }

    public void AddItem(WeightedItem i)
    {
        img.sprite = i.item.GetComponent<TowerBase>().data[0].shopIcon;
        img.color = Color.white;
        HeldItem = i.item;
    }

    public void ClearSlot()
    {
        img.sprite = null;
        img.color = defaultColor;
        HeldItem = null;
        pooledItem = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (HeldItem == null)
            return;

        TowerBase d = HeldItem.GetComponent<TowerBase>();
        previewObj = TowerBenchController.instance.previewObj;
        previewObj.GetComponent<TowerRangeIndicator>().ShowRadius(d.data[0].range);
        previewObj.GetComponent<SpriteRenderer>().sprite = d.data[0].shopIcon;
        previewObj.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (HeldItem == null)
            return;
        Vector3 nextPos = MapManager.instance.MousePositionWorld;
        nextPos.z = 0;
        previewObj.transform.position = nextPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (HeldItem == null)
            return;
        previewObj.SetActive(false);

        if(pooledItem == null && MapManager.instance.IsCurrentMousePosTileEmpty() && MapManager.instance.IsBuildableAtTile(MapManager.instance.MousePositionGrid))
        {
            TowerBase tb = Instantiate(HeldItem, MapManager.instance.MousePositionGrid + new Vector3(0.5f, 0.5f, 0), Quaternion.identity).GetComponent<TowerBase>();
            MapManager.instance.PlaceTower((Vector3)MapManager.instance.MousePositionGrid, tb);
            ClearSlot();
        }
        else if(MapManager.instance.IsCurrentMousePosTileEmpty() && MapManager.instance.IsBuildableAtTile(MapManager.instance.MousePositionGrid))
        {
            HeldItem.transform.position = MapManager.instance.MousePositionGrid + new Vector3(0.5f, 0.5f, 0);
            MapManager.instance.PlaceTower((Vector3)MapManager.instance.MousePositionGrid, HeldItem.GetComponent<TowerBase>());
            HeldItem.SetActive(true);
            ClearSlot();
        }
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        TowerBenchController.instance.previewObj.SetActive(false);
        HeldItem = eventData.pointerDrag;
        pooledItem = eventData.pointerDrag;
        img.sprite = HeldItem.GetComponent<TowerBase>().data[0].shopIcon;
        img.color = Color.white;
        HeldItem.SetActive(false);
    }
}
