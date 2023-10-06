using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateActiveSync : MonoBehaviour
{
    public GameObject target;
    public GameState activeState;

    private void Update()
    {
        if (GameManager.instance.state == activeState)
            target.SetActive(true);
        else
            target.SetActive(false);
    }
}
