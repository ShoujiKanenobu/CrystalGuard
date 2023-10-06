using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRangeIndicator : MonoBehaviour
{

    public LineRenderer circleLR;

    public void ShowRadius(float r)
    {
        DrawRangeIndicator(100, r);
    }


    void DrawRangeIndicator(int steps, float radius)
    {
        //Code from: https://www.youtube.com/watch?v=DdAfwHYNFOE

        circleLR.positionCount = steps;

        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float progress = (float)currentStep / steps;
            float currentRadian = progress * 2 * Mathf.PI;
            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 pos = new Vector3(x, y, 0);
            circleLR.SetPosition(currentStep, pos);
        }
    }
}
