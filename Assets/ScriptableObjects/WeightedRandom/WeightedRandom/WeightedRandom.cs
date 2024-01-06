using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeightedItem<T>
{
    public T item;
    public int weight;
    public WeightedItem(int w)
    {
        item = default(T);
        weight = w;
    }

}

[System.Serializable]
public class WeightedRandom<T> : ScriptableObject
{

    private int totalWeight;

    public void RecalculateWeights(List<WeightedItem<T>> items)
    {
        totalWeight = 0;
        foreach (WeightedItem<T> x in items)
        {
            totalWeight += x.weight;
        }
    }

    public WeightedItem<T> RollForItem(List<WeightedItem<T>> items)
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

    public WeightedItem<T> RollforUniqueItem(List<WeightedItem<T>> items)
    {
        int uniqueWeight = 0;

        foreach (WeightedItem<T> x in items)
        {
            uniqueWeight += x.weight;
        }

        float roll = uniqueWeight * Random.value;
        foreach (WeightedItem<T> x in items)
        {
            if (roll < x.weight)
            {
                return x;
            }
            roll -= x.weight;
        }
        return new WeightedItem<T>(-1);
    }
}