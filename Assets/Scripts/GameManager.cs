using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private SavingAndLoad SLmanager;
    public GameObject SaveLoad;
    [SerializeField]private bool isGamePause;
    public bool isMenuPause;
    public bool isTeachPause;
    void Awake()
    {
        //SLmanager = SaveLoad.GetComponent<SavingAndLoad>();
        SLmanager = GameObject.Find("Save&Load").GetComponent<SavingAndLoad>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.Delete))
        {
            SLmanager.ResetFile();
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            GameData.OpenAllAbility();
        }


        isGamePause = isMenuPause||isTeachPause;

        if(isGamePause)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
        // if(Input.GetKeyDown(KeyCode.S))
        // {
        //     SLmanager.SaveFile();
        // }

        // if(Input.GetKeyDown(KeyCode.L))
        // {
        //     SLmanager.LoadFile();
        // }
    }
    void OnApplicationQuit()
    {
        SLmanager.ResetFile();
        PlayerPrefs.DeleteAll();
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }



    
}
