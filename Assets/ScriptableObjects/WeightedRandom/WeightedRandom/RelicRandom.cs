
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "WeightedRandomTable", menuName = "Weighted Random/Relic")]
public class RelicRandom : WeightedRandom<Relic> {
    public WeightedItem<Relic> RarityRoll(List<Relic> blacklist, RelicRarity rarity)
    {
        List<Relic> save = new List<Relic>(blacklist);
        foreach(WeightedItem<Relic> r in items)
        {
            if(r.item.rarity != rarity && !blacklist.Contains(r.item))
            {
                blacklist.Add(r.item);
            }
        }
        WeightedItem<Relic> toReturn = RollforUniqueItem(blacklist);
        if (toReturn.weight == -1 && rarity != RelicRarity.common)
            return RarityRoll(save, rarity - 1);
        return toReturn;
    }

   /* private bool CheckEnoughRelics(List<Relic> blacklist)
    {
        List<WeightedItem<Relic>> filtered = new List<WeightedItem<Relic>>(items);
        for (int i = filtered.Count - 1; i >= 0; i--)
        {
            if (blacklist.Contains(filtered[i].item))
                filtered.RemoveAt(i);
        }
        if (filtered.Count <= 4)
            return false;
        return true;
    }*/
}