using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator FadingAnimator;
    public float transitionTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        StartCoroutine(LoadLevel(scene.buildIndex)) ;
    }
    public void NextScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        StartCoroutine(LoadLevel(scene.buildIndex+1));
    }
    public void PreScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        StartCoroutine(LoadLevel(scene.buildIndex-1));
    }
    public void BackToStartScene()
    {
        StartCoroutine(LoadLevel(0));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        FadingAnimator.Play("Level_FadeOut");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
    public IEnumerator LoadLevelWithDelay(int levelIndex,float DelayTime)
    {
        FadingAnimator.Play("Level_FadeOut");
        yield return new WaitForSeconds(DelayTime);
        SceneManager.LoadScene(levelIndex);
    }
}
