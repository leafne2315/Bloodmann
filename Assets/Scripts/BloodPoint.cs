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
    void Start()
    {
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
}
