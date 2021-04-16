using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TranSceneCtrller : MonoBehaviour
{
    // Start is called before the first frame update
    public bool showUI;
    private InputManager Im;
    public GameObject ActivateUI;
    public GameObject ActiveUI_Pf;
    public Transform RealWorldCanvas;
    public bool canActivate = true; 
    private LevelLoader LvLoader;
    private int SceneIndex;
    public int NextScene;
    void Awake()
    {
        Im = GameObject.Find("InputManager").GetComponent<InputManager>();
        LvLoader = GameObject.Find("LevelLoder").GetComponent<LevelLoader>();
    } 
    void Start()
    {
        ActivateUI = Instantiate(ActiveUI_Pf,transform.position + Vector3.forward*-2 + Vector3.up*1.0f,Quaternion.identity,RealWorldCanvas);
        ActivateUI.transform.GetComponent<Image>().CrossFadeAlpha(0,0,false);
        SceneIndex = SceneManager.GetActiveScene().buildIndex;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(canActivate)
            {
                showUI = true;
                OpenActivateUI();
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            showUI = false;
            closeActivateUI();
        }
    }
    public void closeActivateUI()
    {
        ActivateUI.transform.GetComponent<Image>().CrossFadeAlpha(0.0f,0.5f,false);
    }
    public void OpenActivateUI()
    {
        ActivateUI.transform.GetComponent<Image>().CrossFadeAlpha(1.0f,0.5f,false);
    }
    public void ChangeScene()
    {
        switch(SceneIndex)
        {
            case 1:

                StartCoroutine(LvLoader.LoadLevelWithDelay(NextScene,5.0f));
                GameObject sfx = Instantiate(Resources.Load("SoundPrefab/Elevator") as GameObject, transform.position, Quaternion.identity);
                //play Elevator Sound

            break;
        
            case 2:

            break;

            case 3:

            break;
        }
    }
}
