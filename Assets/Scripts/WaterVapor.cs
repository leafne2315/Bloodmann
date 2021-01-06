using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    currentState = vaporState.On;
                    isVaporOn = true;
                    //
                    tempDamageBlock = Instantiate(tempPf,transform.position+VaporField.x*transform.right,Quaternion.identity);
                    tempDamageBlock.transform.localScale = 2*VaporField;
                    tempDamageBlock.transform.rotation = Quaternion.Euler(0,0,transform.rotation.eulerAngles.z);
                }
            
            break;      
        }
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
