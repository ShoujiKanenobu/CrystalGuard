using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateActiveSync : MonoBehaviour
{
    public GameObject target;
    public List<GameState> activeState = new List<GameState>();

    private void Update()
    {
        if (activeState.Contains(GameManager.instance.state))
            target.SetActive(true);
        else
            target.SetActive(false);
    }
}
