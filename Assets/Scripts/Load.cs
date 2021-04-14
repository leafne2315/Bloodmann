using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour
{
    public Animator animator;
    private int levelToLoad;
    public bool isScene1 = true;
    public MenuController menuController;
    void Awake() 
    {
        //gameObject.SetActive(true);
        //menuController.GetComponent<MenuController>(); 
    }
    void Update()
    {
        if(Input.anyKeyDown)
        {
            //FadeToNextLevel();
            FadeToLevel(1);
        }

        // if(menuController.isHomePage)
        // {
        //     FadeToLevel(0);
        //     animator.SetTrigger("FadeIn");
        // }
    }

    // public void FadeToNextLevel()
    // {
    //     FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    // }
    
    public void FadeToLevel (int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.Play("BlackFadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
        //SceneManager.LoadScene(1);
        //Destroy(gameObject);
    }
}
