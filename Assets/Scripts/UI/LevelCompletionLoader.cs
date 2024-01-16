using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelCompletionLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject stamp;
    [SerializeField]
    private string pref;

    public void OnEnable()
    {
        if(PlayerPrefs.HasKey(pref))
        {
            stamp.SetActive(true);
        }
        else
        {
            stamp.SetActive(false);
        }
    }
}
