using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeightedItem<T>
{
    public T item;
    public int weight;
}

[System.Serializable]
public class WeightedRandom<T> : ScriptableObject
{
    [SerializeField]
    public List<WeightedItem<T>> items;

    private int totalWeight;

    public void RecalculateWeights()
    {
        totalWeight = 0;
        foreach (WeightedItem<T> x in items)
        {
            totalWeight += x.weight;
        }
    }

    public WeightedItem<T> RollForItem()
    {
        float roll = totalWeight * Random.value;
        foreach (WeightedItem<T> x in items)
        {
            if (roll < x.weight)
            {
                return x;
            }
            roll -= x.weight;
        }
        return default;
    }

    public WeightedItem<T> RollforUniqueItem(List<T> blacklist)
    {
        List<WeightedItem<T>> filtered = new List<WeightedItem<T>>(items);
        for(int i = filtered.Count - 1; i >= 0; i--)
        {
            if (blacklist.Contains(filtered[i].item))
                filtered.RemoveAt(i);
        }

        int uniqueWeight = 0;

        foreach (WeightedItem<T> x in filtered)
        {
            uniqueWeight += x.weight;
        }

        float roll = uniqueWeight * Random.value;
        foreach (WeightedItem<T> x in filtered)
        {
            if (roll < x.weight)
            {
                return x;
            }
            roll -= x.weight;
        }
        return default;
    }
}