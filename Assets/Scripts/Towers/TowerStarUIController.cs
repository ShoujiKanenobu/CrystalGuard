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
        if (count > level)
            Destroy(starParent.transform.GetChild(0).gameObject);
        if (count < level)
            Instantiate(star, starParent.transform);

        starParent.RespaceChildren();
    }
}
