using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GlowFlicker : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Image img;
    private Color savedColor = new Color();
    private void Start()
    {
        img = this.GetComponent<Image>();
    }

    private void Update()
    {
        if (img.color == new Color(0, 0, 0, 0))
            return;

        savedColor = img.color;
        savedColor.a = ((Mathf.Sin(Time.time * speed) + 1) / 2) * 0.4f;
        img.color = savedColor;
    }
}
