using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isActivated = false;
    public float speed;
    private Rigidbody rb;
    private Vector3 LiftDir;
    public float StayButtomTime;
    private float Timer;    
    public enum LiftState{GoDown,Wait,GoUp,Inactivated}
    public LiftState currentState;
    
    [Header("Detect Setting")]
    [SerializeField]private bool isOnTop;
    [SerializeField]private bool isButtom;
    public Transform GroundCheck;
    public Transform TopCheck;
    public float DetectRadius;
    public LayerMask WhatIsGround;
    void Start()
    {
        LiftDir = Vector3.down;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectingPosition();

        switch (currentState)
        {
            case LiftState.Inactivated:

                rb.velocity = Vector3.zero;

                if(isActivated)
                {
                    rb.velocity = LiftDir*speed;
                    currentState = LiftState.GoDown;
                }

            break;

            case LiftState.GoDown:

                if(isButtom)
                {
                    LiftDir = Vector3.zero;
                    currentState = LiftState.Wait;
                }

            break;

            case LiftState.Wait:

                if(Timer<StayButtomTime)
                {
                    Timer+=Time.deltaTime;
                }
                else
                {
                    Timer = 0;
                    currentState = LiftState.GoUp;
                    LiftDir = Vector3.up;
                }
            break;

            case LiftState.GoUp:
                
                rb.velocity = LiftDir*speed;
                if(isOnTop)
                {
                    isActivated = false;
                    print("reset");
                    currentState = LiftState.Inactivated;
                }

            break;

            default:
            break;
        }
    }

    void DetectingPosition()
    {
        isOnTop = Physics.CheckSphere(TopCheck.position,DetectRadius,WhatIsGround);
        isButtom = Physics.CheckSphere(GroundCheck.position,DetectRadius,WhatIsGround);
    }
}
