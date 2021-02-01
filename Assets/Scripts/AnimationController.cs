using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerCtroller playerCtroller;
    Animator PlayerAni;

    void Start()
    {
        playerCtroller =transform.parent.GetComponent<PlayerCtroller>();
        PlayerAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAni.SetFloat("MoveX",Mathf.Abs(playerCtroller.RealMovement.x));
        PlayerAni.SetFloat("MoveY",playerCtroller.RealMovement.y);
        PlayerAni.SetBool("isFly",playerCtroller.isFlying);
        PlayerAni.SetBool("isDash",playerCtroller.isAirDash);
        if(playerCtroller.isFlying||Mathf.Abs(playerCtroller.rb.velocity.x)>0.5f)
            PlayerAni.SetBool("isFlymove",true);
        else
            PlayerAni.SetBool("isFlymove",false);

        if(Input.GetButtonDown("PS4-x"))
        {
            PlayerAni.SetTrigger("Jump");
        }

        
		
            
        
        
        
    }
}
