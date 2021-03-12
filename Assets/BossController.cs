using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // Start is called before the first frame update
    public BossState currentState;
	public enum BossState{Idle,Move,QuickBack,CloseTwice,AirDown,DashAttack,ShootAir,Razer,KnockDown};
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case BossState.Move:

            break;

            case BossState.CloseTwice:
            
            break;
        }
    }
}
