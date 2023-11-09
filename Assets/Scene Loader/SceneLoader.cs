using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private bool isFadeIn;
    [SerializeField] private bool isFadeOut;

    private Animator anim;
    private int sceneIndex;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (isFadeIn) anim.Play("Fade_In");
    }

    public void RestartScene()
    {
        if (isFadeOut) anim.Play("Fade_Out");
        ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    public void NextScene()
    {
        if (isFadeOut) anim.Play("Fade_Out");
        ChangeScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
