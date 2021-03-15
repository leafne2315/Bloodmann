using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour{
    // Start is called before the first frame update
    public BossState currentState;
	public enum BossState{Idle,Move,QuickBack,TwiceAttack,AirDown,DashAttack,ShootAir,Razer,KnockDown};
    private PlayerCtroller PlayerCtroller;
    public GameObject Player;
    public LayerMask PlayerLayer;
    private Rigidbody rb;
    public bool facingRight;
    private float timer;

    [Header("Move Settings")]
    public float MoveSpeed;
    public Vector3 MoveDir;
    public float MoveTime;

    [Header("TwiceAttack Settings")]
    public Vector3 HitBox_size;
    public float AttackStart_1;
    public float AttackEnd_1;
    public float AttackStart_2;
    public float AttackEnd_2;
    public float TA_StateTime;
    public bool hitConfirm;
    private Transform HitPos;
    [Header("AirDown Settings")]
    public float JumpUpTime;
    private Transform targetAirPos;
    public float DownSpeed;
    public float AD_StateTime;
    [Header("DashAttack Settings")]
    public float DA_Speed;
    public float DA_Time;
    private Vector3 DA_Dir;
    public float DA_StateTime;
    [Header("QuickBack")]
    public float BackTime;
    private Vector3 BackDir;
    public float BackSpeed;
    public float QB_StateTime;
    void Start()
    {
        PlayerCtroller = Player.GetComponent<PlayerCtroller>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case BossState.Move:

                getMoveDir();
                
                if(timer<MoveTime)
                {
                    timer+=Time.deltaTime;

                    rb.velocity = MoveDir*MoveSpeed;
                }
                else
                {
                    //next state
                }

            break;

            case BossState.TwiceAttack:

                timer+=Time.deltaTime;

                if(timer<AttackStart_1)
                {
                    //StateIn~~attack_1 start
                }
                else if(timer<AttackEnd_1)
                {
                    //attack_1 start~end
                    AttackHitCheck();
                }
                else if(timer<AttackStart_2)
                {
                    //attack_end1~attack_start2
                    hitConfirm = false;
                }
                else if(timer<AttackEnd_2)
                {
                    //Attack_2 start~end
                    AttackHitCheck();
                }
                else if(timer<TA_StateTime)
                {
                    //attack_2 End ~ StateTime
                    hitConfirm = false;
                }
                else
                {
                    timer = 0;
                    //next state
                }
                
            break;

            case BossState.AirDown:

                timer+= Time.deltaTime;

                if(timer<JumpUpTime)
                {
                    transform.position = Vector3.Lerp(transform.position,targetAirPos.position,timer/JumpUpTime);
                }
                else if(timer<AD_StateTime)
                {
                    rb.velocity = Vector3.down*DownSpeed;
                }
                else
                {
                    timer = 0;
                    //next state
                }

            break;

            case BossState.DashAttack:

                if(timer<DA_Time)
                {
                    rb.velocity = DA_Dir*DA_Speed;
                    //DA
                }
                else if(timer<DA_StateTime)
                {
                    //DA_over
                    rb.velocity = Vector3.zero;
                }
                else
                {
                    timer = 0;
                    //next state
                }

            break;

            case BossState.QuickBack:

                timer+=Time.deltaTime;

                if(timer<BackTime)
                {
                    rb.velocity = BackDir*BackSpeed;
                }
                else if(timer<QB_StateTime)
                {
                    rb.velocity = Vector3.zero;
                }
                else
                {
                    //next state
                }

            break;


        }

        if(facingRight==false&&MoveDir.x>0)
        {
            Flip();
        }
        else if(facingRight == true&&MoveDir.x<0)
        {
            Flip();
        }
    }
    void Flip()
	{
		facingRight = !facingRight;
		Vector3 Scaler = transform.localScale;
		Scaler.x*=-1;
		transform.localScale = Scaler;
	}
    void getMoveDir()
    {
        if(Player.transform.position.x-transform.position.x>0)
        {
            MoveDir = Vector3.right;
        }
        else
        {
            MoveDir = Vector3.left;
        }
    }
    void AttackHitCheck()
    {
        if(!hitConfirm)
		{
			Collider[] hitObjs = Physics.OverlapBox(HitPos.position,HitBox_size,Quaternion.identity,PlayerLayer);
			
			if(hitObjs.Length==0)
			{
				//print("Miss");
			}
			else
			{
				hitConfirm = true;
				
				foreach(Collider c in hitObjs)
				{
					print("Hit"+c.name+"!!!!");
					Player.GetComponent<PlayerCtroller>().gettingHit();
				}
			}
		}
    }
}