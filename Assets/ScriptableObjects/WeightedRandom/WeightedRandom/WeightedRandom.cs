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
        return default(WeightedItem<T>);
    }
}