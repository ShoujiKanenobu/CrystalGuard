using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarController : MonoBehaviour
{
    public Transform hpRect;

    public void SetBarHP(float currentHP, float maxHP)
    {
        float xScale = currentHP / maxHP;
        hpRect.localScale = new Vector3(xScale, hpRect.localScale.y, hpRect.localScale.z);
    }
}
