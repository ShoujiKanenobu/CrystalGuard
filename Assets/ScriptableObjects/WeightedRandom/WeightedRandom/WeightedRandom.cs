using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeightedItem
{
    public GameObject item;
    public Sprite itemImage;
    public int weight;
}

[System.Serializable][CreateAssetMenu(fileName = "WeightedRandomTable", menuName = "Weighted Random Table")]
public class WeightedRandom : ScriptableObject
{
    [SerializeField]
    public List<WeightedItem> items;

    private int totalWeight;

    public void RecalculateWeights()
    {
        totalWeight = 0;
        foreach (WeightedItem x in items)
        {
            totalWeight += x.weight;
        }
    }

    public WeightedItem RollForItem()
    {
        float roll = totalWeight * Random.value;
        foreach (WeightedItem x in items)
        {
            if (roll < x.weight)
            {
                return x;
            }
            roll -= x.weight;
        }
        return default(WeightedItem);
    }
}