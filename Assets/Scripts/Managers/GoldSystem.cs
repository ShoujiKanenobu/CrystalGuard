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

    [SerializeField]
    private GameObject floatText;
    [SerializeField]
    private Vector3 offset;


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
        createFloatingText("+" + i.ToString(), Color.green);
        changeUI();
    }

    public bool SpendGold(int i)
    {
        if (currentGold < i)
            return false;

        currentGold -= i;
        createFloatingText("-" + i.ToString(), Color.red);
        changeUI();
        return true;
    }

    public void ForceLoseGold(int i)
    {
        currentGold -= i;
        if (currentGold < 0)
            currentGold = 0;
        changeUI();
    }
    private void changeUI()
    {
        goldElement.text = currentGold.ToString();
    }

    private void createFloatingText(string newText, Color color)
    {
        GameObject temp = Instantiate(floatText, goldElement.transform);
        temp.GetComponent<FloatingTextController>().Init(goldElement.transform.position + offset, newText, color, 3f, 80);
    }

}
