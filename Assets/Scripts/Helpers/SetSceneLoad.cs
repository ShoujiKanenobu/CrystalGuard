using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSceneLoad : MonoBehaviour
{
    [SerializeField]
    private SceneLoadButton load;
    [SerializeField]
    private string sceneToLoad;

    public void SetNextScene()
    {
        load.targetScene = sceneToLoad;
    }

}
