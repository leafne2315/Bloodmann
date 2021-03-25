using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodPoint : MonoBehaviour
{
    // Start is called before the first frame update
    public bool showUI;
    private InputManager Im;
    public GameObject ActivateUI;
    public bool canActivate;
    public int bloodStock;
    public GameObject ActiveUI_Pf;
    public Transform RealWorldCanvas;
    void Start()
    {
        ActivateUI = Instantiate(ActiveUI_Pf,transform.position + Vector3.forward*-2,Quaternion.identity,RealWorldCanvas);
        ActivateUI.transform.GetComponent<Image>().CrossFadeAlpha(0,0,false);
        
        Im = GameObject.Find("InputManager").GetComponent<InputManager>();
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
}
