using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SavePoint : MonoBehaviour
{
    public bool showUI;
    public bool isSave;
    private InputManager Im;
    public GameObject SavepointMenu_UI;
    public GameObject ActivateUI;
    public bool canActivate;
    public GameObject ActiveUI_Pf;
    public Transform RealWorldCanvas;
    public Transform RestPos;
    public float fadeInTime;
    public float fadeOutTime;
    public GameObject savePointFirstButton;
    
    void Awake()
    {
        Im = GameObject.Find("InputManager").GetComponent<InputManager>();
    }
    void Start()
    {
        ActivateUI = Instantiate(ActiveUI_Pf,transform.position + Vector3.forward*-2 + Vector3.up*1,Quaternion.identity,RealWorldCanvas);
        ActivateUI.transform.GetComponent<Image>().CrossFadeAlpha(0,0,false);
        RestPos = transform.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(SavepointMenu_UI.GetComponent<CanvasGroup>().alpha == 0)
        {
            SavepointMenu_UI.SetActive(false);
        }
        else
        {
            SavepointMenu_UI.SetActive(true);
        }

        // if (EventSystem.current.currentSelectedGameObject != null)
        // {
        //     Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        // }

    }
    void OnTriggerStay(Collider other)
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
    public void SavePointMenu_Open()
    {
        StartCoroutine(FadeInAlphaTime());
        
        
    }

    public void SavePointMenu_Close()
    {
        StartCoroutine(FadeOutAlphaTime());
    }

    public void closeActivateUI()
    {
        ActivateUI.transform.GetComponent<Image>().CrossFadeAlpha(0.0f,0.5f,false);
    }
    public void OpenActivateUI()
    {
        ActivateUI.transform.GetComponent<Image>().CrossFadeAlpha(1.0f,0.5f,false);
    }

    IEnumerator FadeInAlphaTime()
    {
        for(float i =0 ; i<=fadeInTime ; i+=Time.deltaTime)
        {
            SavepointMenu_UI.GetComponent<CanvasGroup>().alpha += Time.deltaTime/fadeInTime;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(savePointFirstButton);
            yield return 0;
        }
    }

    IEnumerator FadeOutAlphaTime()
    {
        for(float i =0 ; i<=fadeOutTime ; i+=Time.deltaTime)
        {
            SavepointMenu_UI.GetComponent<CanvasGroup>().alpha -= Time.deltaTime/fadeOutTime;
            yield return 0;
        }
    }

    public void RestoreHP()
    {
       
    }

    public void AddHP()
    {
        
    }
}
