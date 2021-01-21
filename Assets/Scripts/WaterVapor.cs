using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class WaterVapor : MonoBehaviour
{
    [Header("Detect Settings")]
    public Vector3 VaporField;
    public float VaporLength;
    public LayerMask WhatIsPlayer;
    public bool isVaporOn;
    public RaycastHit hit;
    public float onTime;
    public float offTime;
    private  float onTimer;
    private  float offTimer;
    public GameObject Player;
    //public float Angle;
    private GameObject tempDamageBlock;
    public GameObject tempPf;
    [Header("Statement")]
    public vaporState currentState;
    public enum vaporState{On, Off }
    void Awake()
    {
        
    }
    void Start()
    {
        VaporField.x = VaporLength;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = Quaternion.Euler(0,0,Angle);
        // transform.localScale = VaporField*2;
        
        switch(currentState)
        {
            case vaporState.On:
            
                if(onTimer<onTime)
                {
                    onTimer += Time.deltaTime;
                }
                else
                {
                    onTimer = 0;
                    VTX_Off();
                    currentState = vaporState.Off;
                    isVaporOn = false;
                    //
                    if(tempDamageBlock!=null)
                        Destroy(tempDamageBlock);
                }
            
                if(Physics.CheckBox(transform.position+VaporField.x*transform.right,VaporField,Quaternion.Euler(0,0,transform.rotation.eulerAngles.z),WhatIsPlayer))
                {
                    Player.GetComponent<PlayerCtroller>().gettingHit();
                }
            
            break;
            
            case vaporState.Off:
            
                if(offTimer<offTime)
                {
                    offTimer += Time.deltaTime;
                }
                else
                {
                    offTimer = 0;
                    VTX_On();
                    currentState = vaporState.On;
                    isVaporOn = true;
                    //
                    /*
                    tempDamageBlock = Instantiate(tempPf,transform.position+VaporField.x*transform.right,Quaternion.identity);
                    tempDamageBlock.transform.localScale = 2*VaporField;
                    tempDamageBlock.transform.rotation = Quaternion.Euler(0,0,transform.rotation.eulerAngles.z);
                    */
                }
            
            break;      
        }
    }
    void VTX_On()
    {
        transform.GetChild(0).GetComponent<VisualEffect>().SendEvent("OnPlay");
    }
    void VTX_Off()
    {
        transform.GetChild(0).GetComponent<VisualEffect>().SendEvent("OnStop");
    }
    void OnDrawGizmos() 
    {
        if(!isVaporOn)
            Gizmos.color = Color.gray;
        else
        {
            Gizmos.color = Color.red;
        }
        
        Gizmos.DrawWireCube(transform.position+VaporField.x*transform.right ,2*VaporField);
    }
}
