using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEnemyHighlighter : MonoBehaviour
{
    public GORuntimeSet enemySet;

    private float furthestDist = 0;
    private GameObject furthestEnemy;

    public Color highlightColor;
    public Color defaultColor;

    private void Update()
    {
        furthestDist = 0;
        if (enemySet.Items != null)
            return;

        foreach (GameObject t in enemySet.Items)
        {
            float dist = t.GetComponent<EnemyMovementController>().distanceTraveled;
            t.GetComponent<SpriteRenderer>().color = defaultColor;
            if (furthestDist < dist)
            {
                furthestDist = dist;
                furthestEnemy = t;
                
            }
            
        }
        furthestEnemy.GetComponent<SpriteRenderer>().color = highlightColor;

    }
}
