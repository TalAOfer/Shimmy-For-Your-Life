using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private bool isFadeIn;
    [SerializeField] private bool isFadeOut;
    [SerializeField] private AllLevels allLevels;
    
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (isFadeIn) anim.Play("Fade_In");
    }
    private void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    public void RestartScene()
    {
        if (isFadeOut) anim.Play("Fade_Out");
        ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToStartMenu()
    {
        ChangeScene(0);
    }

    public void GoToLevel(Component sender, object data)
    {
        Level level = (Level)data;
        SceneManager.LoadSceneAsync(level.sceneName);
    }
    public void GoToNextLevel()
    {
        if (isFadeOut) anim.Play("Fade_Out");
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        ChangeScene(currentLevelIndex + 1);
    }
    

    public void ExitApp()
    {
        Application.Quit();
    }
}
