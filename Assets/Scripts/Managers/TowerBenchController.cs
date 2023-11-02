using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TowerBenchController : MonoBehaviour
{
    public static TowerBenchController instance;
    [SerializeField]
    private GameObject towerBench;
    private int benchSize;

    public GameObject previewObj;

    private List<BenchItemController> inv = new List<BenchItemController>();
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;

        if (towerBench != null)
            benchSize = towerBench.transform.childCount;
        for(int i = 0; i < benchSize; i++)
        {
            inv.Add(transform.GetChild(i).GetComponent<BenchItemController>());
        }
    }

    public void ResetAllSlots()
    {
        foreach(BenchItemController b in inv)
        {
            b.ClearSlot();
        }
    }

    public void AddTowerToBench(WeightedItem item)
    {
        int emptyIndex = -1;
        int currentIndex = 0;
        foreach(BenchItemController i in inv)
        {
            if(i.item == null)
            {
                emptyIndex = currentIndex;
                break;
            }
            currentIndex++;
        }

        if (emptyIndex == -1)
            GameManager.instance.FullBenchMessage();
        else
            transform.GetChild(emptyIndex).GetComponent<BenchItemController>().AddItem(item);
    }
}
