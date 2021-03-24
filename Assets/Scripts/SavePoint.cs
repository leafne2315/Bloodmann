using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePoint : MonoBehaviour
{
    // Start is called before the first frame update
    public bool showUI;
    public bool isSave;
    private InputManager Im;
    public Sprite SavepointMenu_UI;
    public GameObject ActivateUI;
    public bool canActivate;


    void Awake()
    {
        Im = GameObject.Find("InputManager").GetComponent<InputManager>();
    }
    void Start()
    {
        ActivateUI.transform.position = transform.position + Vector3.forward*-2; 
        ActivateUI.transform.GetComponent<Image>().CrossFadeAlpha(0,0,false);
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
                ActivateUI.transform.GetComponent<Image>().CrossFadeAlpha(1.0f,0.5f,false);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            showUI = false;
            ActivateUI.transform.GetComponent<Image>().CrossFadeAlpha(0.0f,0.5f,false);
        }
    }
    public void SavePointMenu_Open()
    {
        //open menu
    }
}
