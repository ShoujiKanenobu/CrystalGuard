using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class CanvasBugFix : MonoBehaviour
{
    public Vector3 pos;
    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        SetPos();
    }

    public void SetPos()
    {
        rect.anchoredPosition = pos;
    }
}
