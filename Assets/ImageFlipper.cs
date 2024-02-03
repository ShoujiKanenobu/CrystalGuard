using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ImageFlipper : MonoBehaviour
{
    public List<Sprite> images;
    public Image targetImage;
    private int currentImage;
    
    // Start is called before the first frame update
    void Start()
    {
        currentImage = 0;
        targetImage.sprite = images[currentImage];
    }

    public void CycleUp()
    {
        currentImage++;
        if (currentImage >= images.Count)
            currentImage = 0;
        targetImage.sprite = images[currentImage];
    }

    public void CycleDown()
    {
        currentImage--;
        if (currentImage < 0)
            currentImage = images.Count - 1;
        targetImage.sprite = images[currentImage];
    }
}
