
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "WeightedRandomTable", menuName = "Weighted Random/Relic")]
public class RelicRandom : WeightedRandom<Relic>
{
    public WeightedItem<Relic> RarityRoll(List<WeightedItem<Relic>> items, RelicRarity rarity)
    {
        List<WeightedItem<Relic>> availableForRarity = new List<WeightedItem<Relic>>();
        foreach (WeightedItem<Relic> r in items)
        {
            if (r.item.rarity == rarity)
                availableForRarity.Add(r);
        }
        //This fix ignores rarity. Possible fix to make it only take +1 or -1 rarity
        if (availableForRarity.Count < 1)
        {
            foreach(WeightedItem<Relic> r in items)
            {
                if(rarity == RelicRarity.common)
                {
                    if (r.item.rarity == rarity + 1)
                        availableForRarity.Add(r);
                }
                else
                {
                    if (r.item.rarity == rarity - 1)
                        availableForRarity.Add(r);
                }
            }
        }

        if (availableForRarity.Count == 0)
            Debug.LogError("Couldn't find an available" + rarity + "relic to give");

        return RollforUniqueItem(availableForRarity);
    }
}
