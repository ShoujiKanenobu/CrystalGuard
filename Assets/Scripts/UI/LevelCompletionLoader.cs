using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelCompletionLoader : MonoBehaviour
{
    [SerializeField]
    private Color completeColor;

    [SerializeField]
    private Color incompleteColor;

    [SerializeField]
    private string pref;

    [SerializeField]
    private Image panelcolor;

    public void OnEnable()
    {
        if (panelcolor == null)
            return;

        if(PlayerPrefs.HasKey(pref))
        {
            panelcolor.color = completeColor;
        }
        else
        {
            panelcolor.color = incompleteColor;
        }
    }

    public void ResetCompletion()
    {
        PlayerPrefs.DeleteAll();
    }
}
