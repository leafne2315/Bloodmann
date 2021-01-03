using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVapor : MonoBehaviour
{
    [Header("Detect Settings")]
    public Vector3 VaporField;
    public LayerMask WhatIsPlayer;
    public bool vaporDamage;
    public bool vapor;
    public RaycastHit hit;
    public float onTime;
    public float offTime;
    private  float onTimer;
    private  float offTimer;
    public float Angle;

    [Header("Statement")]
    public vaporState currentState;
    public enum vaporState{On, Off }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = Quaternion.Euler(0,0,Angle);
        // transform.localScale = VaporField*2;
        
        switch(currentState)
        {
            case vaporState.On:
            
            onTimer -= Time.deltaTime;
            transform.rotation = Quaternion.Euler(0,0,Angle);
            if(Physics.CheckBox(transform.position+VaporField.x*transform.right,VaporField,Quaternion.Euler(0,0,Angle),WhatIsPlayer))
            {
                vaporDamage = true;
            }
            else
            {
                vaporDamage = false;
            }
            
            offTimer = offTime;
            
            break;
            
            case vaporState.Off:
            
            if(vaporDamage)
            {
                vaporDamage = false;
            }
            offTimer -= Time.deltaTime;
            onTimer = onTime;
            
            break;      
        }
        
        if(onTimer <=0)
        {
            currentState = vaporState.Off;   
            vapor = false;
        }
        
        if(offTimer <=0)
        {
            currentState = vaporState.On;
            vapor = true;
        }
    }

    void OnDrawGizmos() 
    {
        if(!vapor)
            Gizmos.color = Color.gray;
        if(vapor)
            Gizmos.color = Color.green;
        if(vaporDamage && vapor)
            Gizmos.color = Color.red;
        
        Gizmos.DrawWireCube(transform.position+VaporField.x*transform.right ,2*VaporField);
    }
}
