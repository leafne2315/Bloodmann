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
        PlayerAni.SetBool("isGround",playerCtroller.isGrounded);
        if(playerCtroller.isAttachWall&&!playerCtroller.isGrounded&&!playerCtroller.isFlying)
        {
            PlayerAni.SetBool("isOnWall",true);
        }
        else
        {
            PlayerAni.SetBool("isOnWall",false);
        }

        if(playerCtroller.isFlying&&Mathf.Abs(playerCtroller.rb.velocity.x)>0.2f)
            PlayerAni.SetBool("isFlymove",true);
        else
            PlayerAni.SetBool("isFlymove",false);

        if(Input.GetButtonDown("PS4-x")&&(playerCtroller.isGrounded||playerCtroller.isAttachWall))
        {
            PlayerAni.SetTrigger("Jump");
        }

        if(Input.GetAxis("PS4-L-Horizontal")!=0.0f)
        {
            PlayerAni.SetBool("MovingInputting",true);
        }
        else
        {
            PlayerAni.SetBool("MovingInputting",false);
        }
    }
    public void ResetTrigger()
    {
        PlayerAni.ResetTrigger("Jump");
        PlayerAni.ResetTrigger("Reload");
        PlayerAni.ResetTrigger("Roll");
        PlayerAni.ResetTrigger("Attach");
    }
}
