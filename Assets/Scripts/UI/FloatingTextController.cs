using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FloatingTextController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    private float startTime;
    private float duration;

    private Vector3 startPos;
    private Vector3 endPos;

    private Color startColor;
    private Color endColor;


    public void Start()
    {
        startTime = Time.time;
        //duration = 3f;
    }

    public void Init(Vector3 position, string newText, Color color, float duration, float distance)
    {
        this.duration = duration;
        text.text = newText;
        text.color = color;
        startColor = color;
        endColor = color;
        endColor.a = 0;
        startPos = position;
        endPos = startPos + new Vector3(0, distance, 0);
    }

    public void Update()
    {
        float percent = (Time.time - startTime) / duration;

        if (percent > 1)
            Destroy(this.gameObject);

        this.transform.position = Vector3.Lerp(startPos, endPos, percent);
        text.color = Color.Lerp(startColor, endColor, percent);
    }
}
