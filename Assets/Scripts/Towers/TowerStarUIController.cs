using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerStarUIController : MonoBehaviour
{
    private SpriteGridLayer starParent;

    public GameObject star;
    public void Awake()
    {
        starParent = transform.GetComponentInChildren<SpriteGridLayer>();
    }

    public void CheckStars(int level)
    {
        int count = starParent.transform.childCount;
        int iterations = level - count;
        if (count > level)
            for(int i = 0; i < iterations; i++)
                Destroy(starParent.transform.GetChild(0).gameObject);
            
        if (count < level)
            for (int i = 0; i < iterations; i++)
                Instantiate(star, starParent.transform);

        starParent.RespaceChildren();
    }

    public void ClearStars()
    {
        foreach (Transform child in starParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
