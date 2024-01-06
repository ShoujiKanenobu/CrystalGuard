using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedItemListSO<T> : ScriptableObject
{
    public List<WeightedItem<T>> items;
}
