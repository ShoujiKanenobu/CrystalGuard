using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GoldSystem : MonoBehaviour
{
    public static GoldSystem instance;

    [SerializeField]
    private TextMeshProUGUI goldElement;
    public int currentGold;
    public int startingGold;

    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;

        currentGold = startingGold;
        changeUI();
    }

    public void ResetLevel()
    {
        currentGold = startingGold;
        changeUI();
    }

    public void GainGold(int i)
    {
        currentGold += i;
        changeUI();
    }

    public bool SpendGold(int i)
    {
        if (currentGold < i)
            return false;

        currentGold -= i;
        changeUI();
        return true;
    }

    private void changeUI()
    {
        goldElement.text = currentGold.ToString();
    }

}
