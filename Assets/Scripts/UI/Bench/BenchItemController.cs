using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(TowerStarUIController))]
public class BenchItemController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public GameObject item = null;
    private Image img;
    [SerializeField]
    private Color defaultColor;

    private GameObject previewObj;
    private TowerStarUIController starControl;
    // Start is called before the first frame update
    void Start()
    {
        starControl = GetComponent<TowerStarUIController>();
        img = GetComponent<Image>();
        img.color = defaultColor;
        previewObj = TowerBenchController.instance.previewObj;
    }

    public void SwapItem(BenchItemController x)
    {
        GameObject piTemp = x.item;
        Sprite spTemp = x.img.sprite;
        Color cTemp = x.img.color;

        x.item = item;
        x.img.sprite = img.sprite;
        x.img.color = img.color;

        item = piTemp;
        img.sprite = spTemp;
        img.color = cTemp;

        HandleStars();
        x.HandleStars();
    }

    public void AddItem(WeightedItem i)
    {
        img.sprite = i.item.GetComponent<TowerBase>().data[0].shopIcon;
        img.color = Color.white;
        item = Instantiate(i.item, new Vector3(999, 999, 0), Quaternion.identity);
        item.GetComponent<TowerBase>().Init();
        HandleStars();
        item.SetActive(false);
    }

    private void HandleStars()
    {
        if (item != null)
            starControl.CheckStars(item.GetComponent<TowerBase>().level);
        else
            starControl.ClearStars();
    }

    public void ClearSlot()
    {
        img.sprite = null;
        img.color = defaultColor;
        item = null;
        HandleStars();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null)
            return;
        
        TowerBase d = item.GetComponent<TowerBase>();
        previewObj.GetComponent<TowerRangeIndicator>().ShowRadius(d.data[0].range);
        previewObj.GetComponent<SpriteRenderer>().sprite = d.data[0].shopIcon;
        previewObj.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item == null)
            return;
        Vector3 nextPos = MapManager.instance.MousePositionWorld;
        nextPos.z = 0;
        previewObj.transform.position = nextPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        previewObj.SetActive(false);
        if (item == null)
            return;
        

        if(MapManager.instance.IsBuildableAtTile(MapManager.instance.MousePositionGrid))
        {
            if(MapManager.instance.IsCurrentMousePosTileEmpty())
            {
                if(MapManager.instance.isAtTowerLimit())
                {
                    GameManager.instance.FullTowersMessage();
                    return;
                }
                item.transform.position = MapManager.instance.MousePositionGrid + new Vector3(0.5f, 0.5f, 0);
                MapManager.instance.PlaceTower((Vector3)MapManager.instance.MousePositionGrid, item.GetComponent<TowerBase>());
                item.SetActive(true);
                ClearSlot();
            }
            else if(MapManager.instance.GetTowerAtMousePositionGrid().GetTowerType() == item.GetComponent<TowerBase>().GetTowerType())
            {
                MapManager.instance.GetTowerAtMousePositionGrid().GainXP(item.GetComponent<TowerBase>().GetXP());
                ClearSlot();
            }
        }
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == this.gameObject)
            return;

        previewObj.SetActive(false);

        eventData.pointerDrag.TryGetComponent<BenchItemController>(out BenchItemController o);
        if (o == null)
        {
            if(item == null)
            {
                TowerBenchController.instance.previewObj.SetActive(false);
                item = eventData.pointerDrag;
                img.sprite = item.GetComponent<TowerBase>().data[0].shopIcon;
                img.color = Color.white;
                item.SetActive(false);
            }
            else if(item.GetComponent<TowerBase>().GetTowerType() == eventData.pointerDrag.GetComponent<TowerBase>().GetTowerType())
            {
                item.GetComponent<TowerBase>().GainXP(eventData.pointerDrag.GetComponent<TowerBase>().GetXP());
                Destroy(eventData.pointerDrag);

            }
            
        }
        else if(item != null)
        {
            if (o.item.GetComponent<TowerBase>().GetTowerType() == item.GetComponent<TowerBase>().GetTowerType())
                MergeTowers(item.GetComponent<TowerBase>(), o);
        }
        else
        {
            SwapItem(o);
        }
        HandleStars();
    }

    private void MergeTowers(TowerBase x, BenchItemController o)
    {
        x.GainXP(o.item.GetComponent<TowerBase>().GetXP());
        o.ClearSlot();
        Destroy(o.item);
    }
}