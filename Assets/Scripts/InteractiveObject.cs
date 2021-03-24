using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractiveObject : MonoBehaviour
{
    // Start is called before the first frame update
    public bool showUI;
    public bool isInteracting;
    private InputManager Im;
    void Awake()
    {
        Im = GameObject.Find("ImputManager").GetComponent<InputManager>();
    }
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if(showUI)
        {
            
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            showUI = true;
        }
        else
        {
            showUI = false;
        }
    }
    
    
}
