using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpriteGridLayer : MonoBehaviour
{
    public float spacing;
    public float YPos = 0;
    public bool unforcedPos = false;
    public void Awake()
    {
        if(!unforcedPos)
            transform.localPosition = new Vector3(0, -0.4f, 0);        
    }
    [Button]
    public void RespaceChildren()
    {
        int cc = transform.childCount;
        int k = 0;
        for (int i = 0; i < cc; i++)
        {
            
            transform.GetChild(i).localPosition = new Vector3(spacing * k - (cc - 1) * spacing / 2, YPos, 0);
            if (transform.GetChild(i).gameObject.activeSelf)
                k++;
        }
    }
}
