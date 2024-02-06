using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFlipper : MonoBehaviour
{
    public List<GameObject> targets;
    private int currentImage;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject t in targets)
            t.SetActive(false);
        currentImage = 0;
        targets[currentImage].SetActive(true);
    }

    public void CycleUp()
    {
        targets[currentImage].SetActive(false);
        currentImage++;
        if (currentImage >= targets.Count)
            currentImage = 0;
        targets[currentImage].SetActive(true);
    }

    public void CycleDown()
    {
        targets[currentImage].SetActive(false);
        currentImage--;
        if (currentImage < 0)
            currentImage = targets.Count - 1;
        targets[currentImage].SetActive(true);
    }
}
