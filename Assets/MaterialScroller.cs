using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialScroller : MonoBehaviour
{
    public float scrollSpeed = 0.5f;

    private Renderer r;

    public void Start()
    {
        r = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        r.material.mainTextureOffset = new Vector2(Time.time * scrollSpeed, 0);
    }
}
