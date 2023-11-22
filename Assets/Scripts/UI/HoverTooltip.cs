using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HoverTooltip : MonoBehaviour
{
    public static HoverTooltip instance;

    private bool IsActive = false;

    Camera cam;
    Vector3 min = new Vector3(0, 0, 0);
    Vector3 max;
    RectTransform rect;
    [SerializeField]
    private float offset = 2f;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    void Start()
    {
        cam = Camera.main;
        rect = GetComponent<RectTransform>();
        max = new Vector3(cam.pixelWidth, cam.pixelHeight, 0);
    }

    void Update()
    {
        if(IsActive)
        {
            Vector3 position = new Vector3(Input.mousePosition.x + rect.rect.width, Input.mousePosition.y - (rect.rect.height / 2 + offset), 0f);
            transform.position = new Vector3(Mathf.Clamp(position.x, min.x + rect.rect.width / 2, max.x - rect.rect.width / 2), 
                Mathf.Clamp(position.y, min.y + rect.rect.height / 2, max.y - rect.rect.height / 2), transform.position.z);
        }
        else
        {
            transform.position = new Vector3(-99, -99, -99);
        }
    }

    public void ActivateTooltip(string text)
    {
        descriptionText.text = text;
        IsActive = true;
    }
    public void DeactivateTooltip()
    {
        IsActive = false;
    }

}
