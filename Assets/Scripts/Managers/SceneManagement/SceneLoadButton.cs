using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoadButton : MonoBehaviour
{
    [SerializeField]
    private string targetScene;

    [SerializeField]
    private Animator anim;
    public void LoadScene()
    {
        //LoadingData.sceneToLoad = targetScene;
        SceneManager.LoadScene(targetScene);
    }

    public void LoadAfterFade()
    {
        Time.timeScale = 1f;
        anim.SetTrigger("FadeOut");
    }
}
