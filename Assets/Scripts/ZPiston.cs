using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZPiston : MonoBehaviour
{
    [SerializeField]private float MovingTime;

    [Header("Basic Settings")]
    public float speed;
    public float FrontTime;
    public float BackTime;
    private  float Timer;
    private Vector3 MovingDir;
    public float ZmoveRange;
    // [Header("IEnumerator Settings")]
    // private IEnumerator onTimeCoroutine;
    // private IEnumerator offTimeCoroutine;
    [Header("Statement")]
    public pistonState currentState;
    private pistonState LastState;
    public enum pistonState{Back,Front,Moving}
    void Awake()
    {
        MovingTime = ZmoveRange/speed;
    }
    
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case pistonState.Back:

                if(Timer<BackTime)
                {
                    Timer+=Time.deltaTime;
                }
                else
                {
                    MovingDir = Vector3.forward;
                    Timer = 0;
                    LastState = currentState;
                    currentState = pistonState.Moving;
                }

            break;

            case pistonState.Front:
                
                if(Timer<FrontTime)
                {
                    Timer+=Time.deltaTime;
                }
                else
                {
                    MovingDir = Vector3.back;
                    Timer = 0;
                    LastState = currentState;
                    currentState = pistonState.Moving;
                }

            break;
            case pistonState.Moving:

                if(Timer<MovingTime)
                {
                    Timer+=Time.deltaTime;
                    transform.position += MovingDir*speed*Time.deltaTime;
                }
                else
                {
                    Timer = 0;

                    if(LastState==pistonState.Back)
                    {   
                        currentState = pistonState.Front;
                    }
                    else if(LastState==pistonState.Front)
                    {
                        currentState = pistonState.Back;
                    }
                    else
                    {
                        print("State Error!!!");
                    }
                }

            break;
            
            
            
            
            
        }

        

    }

}
