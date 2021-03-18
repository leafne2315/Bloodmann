using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour
{
    public Animator animator;
    private int levelToLoad;
    public bool isScene1 = true;
    void Awake() 
    {
        DontDestroyOnLoad(transform.gameObject);    
    }
    void Update()
    {
        if(Input.anyKeyDown && isScene1)
        {
            //FadeToNextLevel();
            FadeToLevel(1);
            
        }
    }

    // public void FadeToNextLevel()
    // {
    //     FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    // }
    
    public void FadeToLevel (int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
        //SceneManager.LoadScene(1);
        isScene1 = false;
        animator.SetTrigger("FadeIn");
    }
}
