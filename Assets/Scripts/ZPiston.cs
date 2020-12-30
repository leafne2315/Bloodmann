using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZPiston : MonoBehaviour
{
    [Header("Basic Settings")]
    public float speed;
    public float onTime;
    public float offTime;
    private  float onTimer;
    private  float offTimer;
    public float ZmoveRange;
    // [Header("IEnumerator Settings")]
    // private IEnumerator onTimeCoroutine;
    // private IEnumerator offTimeCoroutine;
    [Header("Statement")]
    public pistonState currentState;
    public enum pistonState{On, Off, waitOn, waitOff}
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case pistonState.On:
            
            transform.Translate(0,0, -speed * Time.deltaTime);
            offTimer = offTime;
            break;
            
            case pistonState.Off:
            
            transform.Translate(0,0, speed * Time.deltaTime);
            onTimer = onTime;
            break;
            
            case pistonState.waitOn:
            
            transform.Translate(0,0,0);
            onTimer -= Time.deltaTime;
            break;
            
            case pistonState.waitOff:
            
            transform.Translate(0,0,0);
            offTimer -= Time.deltaTime;
            break;
            
            
        }
        if(onTimer <=0)
        {
            currentState = pistonState.On;   
        }
        
        if(offTimer <=0)
        {
            currentState = pistonState.Off;
        }
        
        if(transform.position.z >= ZmoveRange && onTimer == onTime)
        {
            offTimer = offTime;
            currentState = pistonState.waitOn;
        }
        
        if(transform.position.z <= 0 && offTimer == offTime)
        {
            onTimer = onTime;
            currentState = pistonState.waitOff;            
        }

    }

}
