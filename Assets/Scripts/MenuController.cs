using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public float menuInputY;
    public GameObject pauseMenu;
    public GameObject pauseFirstButton;
    public GameObject pauseSecondButton;
    public GameObject pauseControllerImage;
    public Load loadScript;
    public bool isOption;
    public bool isHomePage;
    public bool isController;
    private InputManager Im;
    public GameObject InputManager;
    // Start is called before the first frame update
    void Awake()
    {
        
    }
    void Start()
    {
        Im = InputManager.GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Im.PS4_Option)
        {
            isOption=!isOption;
            Im.SwitchState();
            pauseMenu.SetActive(isOption);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        }
        
        if(isOption && Input.GetButtonDown("PS4-O"))
        {
            isOption=!isOption;
            //isOption = true;
            pauseMenu.SetActive(isOption);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        }

        
        if(isOption||isController)
        {
           Time.timeScale = 0f;
        }
        else
        {
           Time.timeScale = 1f;
        }

        if(isController && Input.GetButtonDown("PS4-O"))
        {
            isController = false;
            pauseControllerImage.SetActive(false);
            isOption=!isOption;
            pauseMenu.SetActive(isOption);
            Time.timeScale = 0f;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseSecondButton);
        }

        if(isController && Im.PS4_Option)
        {
            isController = false;
            pauseControllerImage.SetActive(false);
            isOption=!isOption;
            pauseMenu.SetActive(isOption);
            Time.timeScale = 0f;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseSecondButton);
        }
        
        //menuInputY = Input.GetAxis("PS4-UpDown");
    }

    public void Resume()
    {
        isOption=!isOption;
        Im.SwitchState();
        pauseMenu.SetActive(isOption);
    }

    public void ControllerImage()
    {
        isController = true;
    
        pauseControllerImage.SetActive(true);
        isOption=!isOption;
        pauseMenu.SetActive(isOption);
    }
    public void HomePage()
    {
        SceneManager.LoadScene(0);
        //loadScript.GetComponent<Load>();
        //loadScript.GetComponent<Load>().FadeToLevel(0);
        //isHomePage = true;
        Time.timeScale = 1f;
    }
}
