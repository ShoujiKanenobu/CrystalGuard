using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class CanvasBugFix : MonoBehaviour
{
    public Vector3 pos;

    void Start()
    {
        SetPos();
    }

    public void SetPos()
    {
        GetComponent<RectTransform>().anchoredPosition = pos;
    }

    [Button]
    public void GrabPosition()
    {
        pos = GetComponent<RectTransform>().anchoredPosition;
    }
    
    [Button]
    public void SetPosition()
    {
        GetComponent<RectTransform>().anchoredPosition = pos;
    }
}
