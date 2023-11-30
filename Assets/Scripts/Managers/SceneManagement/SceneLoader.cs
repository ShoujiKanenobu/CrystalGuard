using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//https://levelup.gitconnected.com/tip-of-the-day-loading-screen-in-unity-d25d474e064f

public class LoadingData
{
    public static string sceneToLoad;
}

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private Image progressBar;

    [SerializeField]
    private Animator anim;

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(LoadingData.sceneToLoad);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            progressBar.fillAmount = operation.progress;
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
                anim.SetTrigger("FadeOut");
            }
            yield return null;
        }
    }
}
