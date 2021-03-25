using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePoint : MonoBehaviour
{
   public bool showUI;
    public bool isSave;
    private InputManager Im;
    public Sprite SavepointMenu_UI;
    public GameObject ActivateUI;
    public bool canActivate;
    public GameObject ActiveUI_Pf;
    public Transform RealWorldCanvas;

    void Awake()
    {
        Im = GameObject.Find("InputManager").GetComponent<InputManager>();
    }
    void Start()
    {
        ActivateUI = Instantiate(ActiveUI_Pf,transform.position + Vector3.forward*-2,Quaternion.identity,RealWorldCanvas);
        ActivateUI.transform.GetComponent<Image>().CrossFadeAlpha(0,0,false);
    }

    // Update is called once per frame
    void Update()
    {    
        
    }
    void GenerateUI()
    {

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
        //open menu
    }

    public void closeActivateUI()
    {
        ActivateUI.transform.GetComponent<Image>().CrossFadeAlpha(0.0f,0.5f,false);
    }
    public void OpenActivateUI()
    {
        ActivateUI.transform.GetComponent<Image>().CrossFadeAlpha(1.0f,0.5f,false);
    }
}
