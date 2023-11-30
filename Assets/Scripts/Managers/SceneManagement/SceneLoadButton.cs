using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoadButton : MonoBehaviour
{
    [SerializeField]
    private string targetScene;

    public void LoadScene()
    {
        //LoadingData.sceneToLoad = targetScene;
        SceneManager.LoadScene(targetScene);
    }
}
