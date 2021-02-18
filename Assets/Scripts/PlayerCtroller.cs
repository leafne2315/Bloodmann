﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerCtroller : MonoBehaviour {
	public GameObject GM;
	private GameManager GameManager;
	private Vector3 StartPos;
	public GameObject Arrow;
	public Vector2 FlyDir;
	public float hp;
	public float hp_Max = 100;
	public float speed;
	public float moveInput_X;
	private float moveInput_Y;
	private float JumpInput;
	public Rigidbody rb;
	public Vector2 RealMovement;
	public bool facingRight = true;
	//偵測
	[Header("Detect Settings")]
	private bool isGrounded;
	private bool isAttachWall;
	private bool isAttachOnTop;
	private Collider AttachingObj;
	public float checkRadius;
	public Transform GroundCheck;
	public Transform FrontCheck;
	public Transform UpCheck;
	public LayerMask WhatIsGround;
	public LayerMask WhatIsWall;
	public LayerMask WhatIsJar;
	//
	//跳躍
	[Header("Jump Settings")]
	public float JumpForce;
	public float DoubleJumpForce;
	public int extraJumps;
	public int extraJumpValue;
	private float JumpTimer;
	public float JumpTime;
	[SerializeField]private bool isJumping;
	//
	//飛行
	[Header("Flying Settings")] 
	public float flyPw;
    public float acel;
	public float flySpeed;
    public float MaxFSpeed;
    [SerializeField]private float showflySpeed;
	//
	//空中衝刺
	[Header("AirDash Settings")]
	public float AirDashTime;
	public bool isAirDash;
	public float AirDashSpeed;
	public float AirDashCD;
	[SerializeField]private bool canAirDash = true;
	//
	//衝刺
	[Header("Dash Settings")]
	[SerializeField]private Vector3 DashDir;
	public float Charge_MaxTime;
	public float Dash_PreTime;
	public float dashTimer;
	public float dashTime;
	public float dashCD;
	public float holdingTime = 0.2f;
	public bool isDash;
	private bool canDash;
	[SerializeField]private float dashSpeed;
	public float Max_dashSpeed;

	[Header("ReBound Settings")]
	public float BoundSpeed;
	Vector3 BoundDir;
	public float BoundTime;
	//
	//被攻擊
	[Header("UnderAttack Settings")]
	public float KnockTimer;
	public bool isHit;
	public float KnockDuration;
	public float KnockPwr;
	private Vector3 KnockDir;
	private bool getHitByRight;
	//
	//狀態控制
	[Header("Statement Settings")]
	public PlayerState currentState;
	public PlayerState LastState;
	public enum PlayerState{Normal,Defend,GetHit,Dash,Rebound,Attach,BugFly,AirDash,PreAttack,Attack,AfterAttack,Reloading,Throw,Idle};
	private bool canAttach = true;
	public bool isFlying;
	public bool isStill;
	[Range(0.0f,0.5f)]public float Attach_IntervalTime;
	//
	//燃料
	[Header("Gas Settings")]
	public float GasUsingValue;
	public float currentGas;
	public float Gas_MaxValue;
	public bool Out_Of_Gas;
	private bool RestoreGas_isOver = true;
	[Header("PreAttack Settings")]
	public float PreAttackTime;
	private float Timer;
	//
	//攻擊
	[Header("Attack Settings")]
	public bool isIneffective;
	private bool isAttacking;
	public int AttackRemain;
	public int FullRemain;
	public bool canAttack = true;
	public float AttackTime = 0.5f;
	public float AttackCD = 0.8f;
	//public float AttackRange;
	private Vector3 AttackDir;
	public Transform hitPos;
	public Transform hitPos_Up;
	public Transform currentHitPos;
	private Quaternion AttackAngle;
	public LayerMask EnemyLayer;
	public float ReviveTime;
	public Vector3 HitBox_size;
	public GameObject AttackFTX;
	public GameObject AttackHitFTX;
	public bool hitConfirm = false;
	[Header("AfterAttack Settings")]
	public float AfterAttackTime;
	[Header("Reload Settings")]
	public float ReloadTime;
	//
	[Header("Throwing Settings")]
	public ThrowingCurve ThrowScript;
	private Vector3 ThrowDir;
	private Vector3 RotateAngleSpeed;
	private bool StartThrow;
	private float ThrowTimer;
	public float ThrowWaitingTime = 0.03f;
	public float AimSmoothTime = 0.05f;
	public GameObject StonePref;
	public Transform ThrowPos;
	public float ThrowEndTime;
	public float ThrowCD;
	private bool canThrow = true;
	private bool isThrowing = false;
	[Header("Animation Settings")]
	public Animator PlayerAni;
	//
	[Header("??? Settings")]
	//private float OriginGravity;
	public bool isInvincible = false;
	private float StableValue;
	private ExternalForce Ef;
	public Vector3 SavePointPos;

	void Awake()
	{
		GameManager = GM.GetComponent<GameManager>();
		rb = GetComponent<Rigidbody>();
		//OriginGravity = rb.gravityScale;
		Ef = GetComponent<ExternalForce>();
	}
	void Start()
	{
		StartPos = transform.position;

		KnockTimer = 0;
		extraJumps = extraJumpValue;
		JumpTimer = JumpTime;
		currentState = PlayerState.Normal;
		currentGas = Gas_MaxValue;
		Arrow.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
	}
	void FixedUpdate()
	{
		if(currentState!=PlayerState.BugFly&&currentState!=PlayerState.Attach&&currentState!=PlayerState.Dash)
		{
			rb.AddForce(Physics.gravity*5.0f,ForceMode.Acceleration);
		}
		
	}
	void Update()
	{
		//print(currentState);
		if(hp<=0)
		{
			Die();
		}

		switch(currentState)
		{
			case PlayerState.Idle:

			break;

			case PlayerState.Normal:
				
				CheckStability();
				RealMovement = new Vector2(Mathf.Lerp(rb.velocity.x,moveInput_X * speed,StableValue) , rb.velocity.y);
				RealMovementFix();

				
				rb.velocity = RealMovement + Ef.OtherForce;
				showflySpeed = rb.velocity.y;
				
				AccelControll();
				LimitGSpeed();
				/*
				if(Input.GetKey(KeyCode.W)||Input.GetButton("PS4-R2")&&!Out_Of_Gas)
				{
					rb.velocity += Vector2.up*acel;
					if(rb.velocity.y>MaxFSpeed)
					{
						rb.velocity = new Vector2(rb.velocity.x,MaxFSpeed);
					}
					GasUse(0);
				}
				*/
				if(Input.GetKeyDown(KeyCode.Space)||Input.GetButtonDown("PS4-x") && isGrounded == true)
				{
					rb.velocity =  new Vector2 (moveInput_X*speed,JumpForce);
				}

				// if(isGrounded)
				// {
				// 		RestoreGas();
				// }
				

				if(Input.GetMouseButtonDown(0)||Input.GetButtonDown("PS4-Triangle"))//->Dash
				{	
					
					dashTimer = 0; //重置dash 時間
					// dashSpeed = 0;
					currentState = PlayerState.Dash;
	
					// rb.gravityScale = 0;
					rb.velocity = Vector3.zero;
					//StartCoroutine(dashCD_Count());
					Arrow.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
					Arrow.GetComponent<ArrowShow>().LastDir = Vector3.up;
				}

				if(isAttachWall&&!isGrounded&&canAttach)
				{
					if(AttachingObj!=null)
					{
						if(AttachingObj.CompareTag("Jar")||AttachingObj.CompareTag("Lift"))
						{
							AttachingObj.GetComponent<OnAttached>().isAttached = true;
							FixedJoint j = gameObject.AddComponent<FixedJoint>();
							j.connectedBody = AttachingObj.attachedRigidbody;
						}
					}

					currentState = PlayerState.Attach;
					
					rb.velocity = Vector3.zero;
				}
				if(Input.GetKeyDown(KeyCode.C)||Input.GetButtonDown("PS4-L1"))
				{
					currentState = PlayerState.BugFly;
					if(isGrounded)
					{

						FlyDir = new Vector2(moveInput_X,moveInput_Y).normalized;
					}
					else
					{
						FlyDir = rb.velocity.normalized;
					}
				}

				if(Input.GetButtonDown("PS4-Square")||Input.GetKeyDown(KeyCode.Z))
				{
					rb.velocity = Vector3.zero;
					canAttack = false;
					//StartCoroutine(AttackCD_Count());
					currentState = PlayerState.PreAttack;
					PlayerAni.SetTrigger("Attack");
					
				}

				if(Input.GetButtonDown("PS4-L2"))
				{
					rb.velocity = Vector3.zero;

					if(AttackRemain!=FullRemain)
					{
						currentState = PlayerState.Reloading;
					}
				}

			break;

			case PlayerState.Attach:

				//isAttachWall = Physics2D.OverlapCircle(FrontCheck.position,0.05f,WhatIsWall);
				rb.velocity = Vector3.zero;
				RestoreGas();

				if(Input.GetMouseButtonDown(0)||Input.GetButtonDown("PS4-Triangle"))//->Dash
				{	
				
					
					currentState = PlayerState.Dash;
					//rb.gravityScale = 2;
					rb.velocity = Vector3.zero;
					//StartCoroutine(dashCD_Count());
					Arrow.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
					Arrow.GetComponent<ArrowShow>().LastDir = Vector3.up;	
				}

				if(Input.GetKeyDown(KeyCode.C)||Input.GetButtonDown("PS4-L1")&&!Out_Of_Gas)
				{
					if(AttachingObj!=null)
					{
						AttachingObj.GetComponent<OnAttached>().isAttached = false;
						Destroy(GetComponent<FixedJoint>());
					}

					currentState = PlayerState.BugFly;
					StartCoroutine(IntervalTime_Count());
					FlyDir = rb.velocity.normalized;
				}

				if(Input.GetButtonDown("PS4-x"))
				{
					if(!isAttachOnTop)
					{
						if(!facingRight)
						{
							rb.velocity = Vector2.one.normalized*JumpForce;
						}
						else
						{
							rb.velocity = new Vector2(-1,1).normalized*JumpForce;
						}
					}
					else
					{
						rb.velocity = Vector2.down*10;
					}
					
					if(AttachingObj!=null)
					{
						AttachingObj.GetComponent<OnAttached>().isAttached = false;
						Destroy(GetComponent<FixedJoint>());
					}


					currentState = PlayerState.Normal;
					StartCoroutine(IntervalTime_Count());
				}

			break;

			case PlayerState.BugFly:

				FlyMovement();
				/*
				if(Input.GetButtonDown("PS4-Triangle")&&canAirDash)
				{
					isAirDash = true;
					currentState = PlayerState.AirDash;
					StartCoroutine(AirDash_Count());
					FlyDir = new Vector2(moveInput_X,moveInput_Y);
					canAirDash = false;
				}
				*/
				if(Input.GetKeyDown(KeyCode.V)||Input.GetButtonDown("PS4-L1"))
				{
					currentState = PlayerState.Normal;
					//rb.gravityScale = OriginGravity;
					//FlyDir = Vector2.zero;
				}
				if(Input.GetMouseButtonDown(0)||Input.GetButtonDown("PS4-Triangle"))//->Dash
				{	
					
					dashTimer = 0; //重置dash 時間
					// dashSpeed = 0;
					currentState = PlayerState.Dash;
	
					// rb.gravityScale = 0;
					rb.velocity = Vector3.zero;
					//StartCoroutine(dashCD_Count());
					Arrow.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
					Arrow.GetComponent<ArrowShow>().LastDir = Vector3.up;
				}
				// if(Input.GetButtonDown("PS4-Square")||Input.GetKeyDown(KeyCode.Z))
				// {
					
				// 	AttackHitCheck();
					
				// }

				if(Out_Of_Gas)
				{
					currentState = PlayerState.Normal;
				}

			break;

			case PlayerState.AirDash:

				if(isAirDash)
				{
					rb.velocity = FlyDir.normalized*AirDashSpeed;
				}
				else
				{
					currentState = PlayerState.BugFly;
				}

			break;

			case PlayerState.Dash:

				if(!isDash)
				{

					
					//GasUse(40);
					/*
					if(dashSpeed<Max_dashSpeed)
					{
						dashSpeed+=75*Time.deltaTime;
					}
					else
					{
						dashSpeed = Max_dashSpeed;
					}
					*/

					//dashTimer+=Time.deltaTime;

					if(Input.GetMouseButtonUp(0)||Input.GetButtonUp("PS4-Triangle")||dashTimer>Charge_MaxTime||Out_Of_Gas)
					{
						
						isDash = true;
						
						//Mouse_DirCache();
						GetDashDir();

						dashTimer = 0;
						Arrow.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
					}
				}
				else
				{
					if(dashTimer<dashTime)
					{
						dashTimer+=Time.deltaTime;
						
						rb.velocity = DashDir*dashSpeed;

						if(DashDir.x>0 &&!facingRight)
						{
							Flip();
						}
						else if(DashDir.x<0 && facingRight)
						{
							Flip();
						}

						if((isAttachWall||isGrounded) && dashTimer>0.1f)//撞牆
						{
							rb.velocity = Vector3.zero;

							dashTimer = 0; //重置dash 時間
							currentState = PlayerState.Normal;
							isDash = false;
							
						}

						DashAttack();
						if(hitConfirm)
						{
							dashTimer = 0;
							hitConfirm = false;
							AttackRemain--;

							
							isDash = false;
							getBoundDir();
							rb.velocity = BoundDir*BoundSpeed;
							currentState = PlayerState.Rebound;
						}	
					}
					else
					{
						dashTimer = 0; //重置dash 時間
						currentState = PlayerState.Normal;
						isDash = false;			
					}
				}
			break;
			
			case PlayerState.Rebound:

				if(Timer<BoundTime)
				{
					Timer+=Time.deltaTime;

					
				}
				else
				{
					Timer = 0;
					currentState = PlayerState.Normal;
				}

			break;

			case PlayerState.Throw:
				
				getThrowDir();
				float ThrowAngle = Vector3.SignedAngle(ThrowDir,Vector3.right,Vector3.back);
				ThrowScript.ThwAngleChange(ThrowAngle);

				if(Input.GetButtonUp("PS4-O")&&!isThrowing)
				{
					isThrowing = true;
					ThrowObj();

					StartCoroutine(ThrowCD_Count());
	
					LineRenderer Lr = ThrowScript.GetComponent<LineRenderer>();
					Lr.enabled = false;
				}

				if(isThrowing)
				{
					if(ThrowTimer<ThrowEndTime)
					{
						ThrowTimer+=Time.deltaTime;
						//執行投擲動畫
					}
					else
					{
						ThrowTimer = 0;
						currentState = LastState;
						isThrowing = false;
						//ThrowDir = Default_ThrowDir();//重置回預設方向
					}
				}

			break;

			case PlayerState.GetHit:
				
				if(KnockTimer<KnockDuration)
				{
					KnockTimer+=Time.deltaTime;
					rb.velocity = KnockDir*KnockPwr;
				}
				else
				{
					KnockTimer = 0;
					if(LastState!=PlayerState.BugFly)
					{
						currentState = PlayerState.Normal;
					}
					else
					{
						currentState = PlayerState.BugFly;
					}
				}

			break;

			case PlayerState.PreAttack:

				if(Timer<PreAttackTime)
				{
					Timer+=Time.deltaTime;
					//animation
				}
				else
				{
					Timer = 0;
					currentState = PlayerState.Attack;
					AttackMove();
				}

			break;

			case PlayerState.Attack:
				
				if(Timer<AttackTime)
				{
					Timer+=Time.deltaTime;

					AttackHitCheck();
					//animation
				}
				else
				{
					Timer = 0;
					currentState = PlayerState.AfterAttack;
				}

			break;

			case PlayerState.AfterAttack:

				if(Timer<AfterAttackTime)
				{
					Timer+=Time.deltaTime;
				}
				else
				{
					Timer = 0;
					hitConfirm = false;
					currentState = PlayerState.Normal;

					print(AttackRemain);
				}

			break;
			case PlayerState.Reloading:

				if(Timer<ReloadTime)
				{
					Timer+=Time.deltaTime;
				}
				else
				{
					Timer = 0;
					AttackRemain = FullRemain;
					currentState = PlayerState.Normal;
				}
				
			break;

			default:
			break;
		}


		isGrounded = Physics.CheckSphere(GroundCheck.position,checkRadius,WhatIsGround);
		isAttachOnTop = Physics.CheckSphere(UpCheck.position,0.05f,WhatIsWall);
		isAttachWall = Physics.CheckSphere(FrontCheck.position,0.05f,WhatIsWall)||isAttachOnTop;
		
		if(isAttachWall)
		{
			if(isAttachOnTop)
			{
				Collider[] c = Physics.OverlapSphere(UpCheck.position,0.05f,WhatIsJar);
				if(c.Length!=0)
				{
					AttachingObj = c[0];
				}
			}
			else
			{
				Collider[] c = Physics.OverlapSphere(FrontCheck.position,0.05f,WhatIsJar);
				if(c.Length!=0)
					AttachingObj = c[0];
			}
		}
		else
		{
			AttachingObj = null;
		}
			
		isAttacking = (currentState ==PlayerState.PreAttack||currentState==PlayerState.Attack||currentState==PlayerState.AfterAttack);
		isFlying = (currentState == PlayerState.AirDash||currentState == PlayerState.BugFly);
		isStill = (currentState == PlayerState.Attach||(currentState==PlayerState.Normal && isGrounded));

		checkAttackRemain();

		CheckStability();

		if(!isAttacking)
		{
			getMoveInput();
		}

		
		//Debug.Log(rb.velocity.x);
		//Debug.Log(rb.velocity.y);
		if(isGrounded == true)
		{
			extraJumps = extraJumpValue;
			JumpTimer = JumpTime;

			if(currentState!=PlayerState.Dash&&currentState!=PlayerState.BugFly)
			{	
				RestoreGas();
			}
			// if(RestoreGas_isOver)
			// {
			// 	StartCoroutine(GasRestore());
			// 	RestoreGas_isOver = false;
			// }
		}

		if(!isAttachWall && currentState!=PlayerState.Idle)
		{
			if(facingRight==false&&moveInput_X>0)
			{
				Flip();
			}
			else if(facingRight == true&&moveInput_X<0)
			{
				Flip();
			}
		}

		if(Input.GetButtonDown("PS4-O"))//從任何階段進入Throw階段
		{
			if(canThrow)
			{
				LastState = currentState;
				ThrowDir = Default_ThrowDir();
				// print(ThrowDir);

				StartCoroutine(showLine_afterFrame());
				// LineRenderer Lr = ThrowScript.GetComponent<LineRenderer>();
				// Lr.enabled = true;

				currentState = PlayerState.Throw;
				canThrow = false;
			}
			
		}

	}
	void getBoundDir()
	{
		if(DashDir.x>0)
		{
			if(DashDir.y>0.5f)
			{
				BoundDir = Vector3.left;
			}
			else
			{
				BoundDir = new Vector3(-0.5f,0.5f,0);
			}
		}
		else
		{
			if(DashDir.y>0.5f)
			{
				BoundDir = Vector3.right;
			}
			else
			{
				BoundDir = new Vector3(0.5f,0.5f,0);
			}
		}
	}
	void DashAttack()
	{
		Vector3 attackPos = transform.position + DashDir*2.0f;
		Vector3 hitBox = new Vector3(0.25f,0.25f,0.5f); 
		float angle = Vector3.SignedAngle(Vector3.right,DashDir,Vector3.forward);
		Vector3 q = new Vector3(0,0,angle);
		
		if(!isIneffective)
		{
			if(!hitConfirm)
			{
				Collider[] hitObjs = Physics.OverlapBox(attackPos,hitBox,Quaternion.Euler(q),EnemyLayer);
				
				if(hitObjs.Length==0)
				{
					print("Miss");
				}
				else
				{
					hitConfirm = true;
					
					foreach(Collider c in hitObjs)
					{
						print("Hit"+c.name+"!!!!");
						StartCoroutine(c.GetComponent<tempGetHit>().HitTrigger(AttackDir));
					}
				}
			}

		}
		else
		{
			print("No Core!!");
		}
	}
	void checkAttackRemain()
	{
		if(AttackRemain<=0)
		{
			isIneffective = true;
		}
		else
		{
			isIneffective = false;
		}
	}
	void getMoveInput()
	{
		moveInput_X = Input.GetAxis("PS4-L-Horizontal");
		moveInput_Y = Input.GetAxis("PS4-L-Vertical");
	}
	void AttackMove()
	{
			if(moveInput_Y>0.65f)
			{
				AttackAngle = Quaternion.Euler(0,0,90);
				currentHitPos = hitPos_Up;
				AttackDir = Vector3.up;
			}
			else
			{
				currentHitPos = hitPos;
				if(facingRight)
				{
					AttackAngle = Quaternion.identity;
					AttackDir = Vector3.right;
				}
				else
				{
					AttackAngle = Quaternion.Euler(0,180,0);
					AttackDir = Vector3.left;
				}
			}
			AttackRemain--;

			GameObject Ftx = Instantiate(AttackFTX,currentHitPos.position,AttackAngle);
			Ftx.transform.SetParent(transform);
	}
	void AttackHitCheck()
	{	
		if(!isIneffective)
		{
			if(!hitConfirm)
			{
				Collider[] hitObjs = Physics.OverlapBox(currentHitPos.position,HitBox_size,AttackAngle,EnemyLayer);
				
				if(hitObjs.Length==0)
				{
					print("Miss");
				}
				else
				{
					hitConfirm = true;
					
					foreach(Collider c in hitObjs)
					{
						print("Hit"+c.name+"!!!!");
						StartCoroutine(c.GetComponent<tempGetHit>().HitTrigger(AttackDir));
					}
				}
			}

		}
		else
		{
			print("No Core!!");
		}

	}
	Vector3 Default_ThrowDir()
	{
		Vector3 dir;
		if(facingRight)
		{
			dir = new Vector3(1.0f,1.0f,0).normalized;
		}
		else
		{
			dir = new Vector3(-1.0f,1.0f,0).normalized;
		}

		return dir;
	}
	void Flip()
	{
		facingRight = !facingRight;
		Vector3 Scaler = transform.localScale;
		Scaler.x*=-1;
		transform.localScale = Scaler;
	}
	void RealMovementFix()
	{
		if(Mathf.Abs(RealMovement.x)<0.5f)
		{
			RealMovement.x = 0.0f;
		}
	}
	IEnumerator AirDash_Count()
	{
		for(float i =0 ; i<=AirDashTime ; i+=Time.deltaTime)
		{
			yield return 0;
		}
		isAirDash = false;

		for(float i =0 ; i<=(AirDashCD-AirDashTime) ; i+=Time.deltaTime)
		{
			yield return 0;
		}
		canAirDash = true;
	}
	IEnumerator AttackCD_Count()
	{
		for(float i =0 ; i<=AttackCD ; i+=Time.deltaTime)
		{
			yield return 0;
		}
		canAttack = true;
	}
	IEnumerator IntervalTime_Count()
	{
		canAttach = false;
		for(float i =0 ; i<=Attach_IntervalTime ; i+=Time.deltaTime)
		{
			yield return 0;
		}
		canAttach = true;
	}
	IEnumerator ReviveTime_Count()
	{
		for(float i =0 ; i<=ReviveTime ; i+=Time.deltaTime)
		{
			yield return 0;
		}
		isInvincible = false;
	}
	IEnumerator ThrowCD_Count()
	{
		canThrow = false;
		for(float i =0 ; i<=ThrowCD ; i+=Time.deltaTime)
		{
			yield return 0;
		}
		canThrow = true;
	}
	IEnumerator showLine_afterFrame()
	{
		for(int i = 0;i<2;i++)
		{
			yield return 0;
		}
		

		LineRenderer Lr = ThrowScript.GetComponent<LineRenderer>();
		Lr.enabled = true;
	}
	
	void ThrowObj()
	{
		GameObject stone = Instantiate(StonePref,ThrowPos.position,Quaternion.identity);
		stone.GetComponent<StoneController>().getDir(ThrowDir);
	}
	/*
	IEnumerator dashCD_Count()
	{
		for(float i =0 ; i<=dashCD ; i+=Time.deltaTime)
		{

			yield return 0;
		}
		canDash = true;
	}
	*/
	private void LimitGSpeed()
    {
        if(rb.velocity.y<-60.0f)
        {
            rb.velocity =new Vector2(rb.velocity.x,-60.0f);
        }
    }
    void AccelControll()
    {
        if(rb.velocity.y<0)
        {
            acel = 5.0f;
        }
        else
        {
            acel = 1.2f;
        }
    }
	private void CheckStability()
	{
		if(isGrounded)
		{
			StableValue = 0.9f;
		}
		else
		{
			StableValue = 0.1f;
		}
	}
	void GasUse(float comsumePS)
	{
		if(currentGas>0)
			currentGas-=comsumePS*Time.deltaTime;
		else
		{
			Out_Of_Gas = true;
		}
	}
	
	void RestoreGas()
	{
		if(currentGas<Gas_MaxValue)
		{
			currentGas+=60*Time.deltaTime;

			if(Out_Of_Gas)
				Out_Of_Gas = false;	
		}
		else
		{
			currentGas = Gas_MaxValue;
		}
	}
	Vector3 Joysticks_Dir()
	{
		Vector3 L_Joy = new Vector3(Input.GetAxis("PS4-L-Horizontal"),Input.GetAxis("PS4-L-Vertical"),0.0f);
		if(Vector3.Magnitude(L_Joy)<0.2f)
		{
			L_Joy = Vector3.zero;
		}

		L_Joy = L_Joy.normalized;

		return L_Joy;
	}
	void GetDashDir()
	{
		DashDir = Arrow.GetComponent<ArrowShow>().LastDir;
	}
	void Mouse_DirCache()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = -Camera.main.transform.position.z;
		Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
		Vector3 myPos = new Vector2(transform.position.x,transform.position.y);
		DashDir = (targetPos-myPos).normalized;
	}
	void FlyMovement()
	{
		
		FlyDir = Vector2.Lerp(FlyDir,new Vector2(moveInput_X,moveInput_Y),0.05f);
		if(Input.GetButton("PS4-R2"))
		{
			rb.velocity = FlyDir*40;
			GasUse(15);
		}
		else
		{
			rb.velocity = FlyDir*flySpeed;
			GasUse(GasUsingValue);
		}
	}
	IEnumerator HitTrigger()
    {
		Color OriginColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.red;
		yield return 0;
        GetComponent<Renderer>().material.color = OriginColor;
    }
	public void gettingHit()
	{
		if(!isInvincible)
		{
			isInvincible = true;
			hp -= 15.0f;
			//StartCoroutine(HitTrigger());
			StartCoroutine(ReviveTime_Count());
			LastState = currentState;
			currentState = PlayerState.GetHit;

			if(facingRight)
			{
				KnockDir = new Vector3(-Mathf.Cos(45*Mathf.Deg2Rad),Mathf.Sin(45*Mathf.Deg2Rad),0);
			}
			else
			{
				KnockDir = new Vector3(Mathf.Cos(45*Mathf.Deg2Rad),Mathf.Sin(45*Mathf.Deg2Rad),0);
			}

			if(GetComponent<FixedJoint>()!=null)
			{
				Destroy(GetComponent<FixedJoint>());
			}
		}
	} 
	void OnTriggerEnter(Collider other)
	{
		// if(other.CompareTag("Enemy"))
		// {
		// 	if(!isInvincible)
		// 	{
		// 		//print(other.name);

		// 		if(other.transform.position.x>transform.position.x)
		// 		{
		// 			getHitByRight = true;
		// 		}
		// 		else
		// 		{
		// 			getHitByRight = false;
		// 		}
		// 		getKnockDir();
		// 		hp -= 15.0f;

		// 		isInvincible = true;
		// 		StartCoroutine(ReviveTime_Count());
		// 		LastState = currentState;
		// 		currentState = PlayerState.GetHit;
		// 	}
			
		// }
	}
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(FrontCheck.position,checkRadius);
		Gizmos.DrawWireCube(hitPos.position,2*HitBox_size);
	}
	// private void OnTriggerEnter2D(Collider2D other)
	// {
	// 	if(other.CompareTag("VisionEnemy"))
	// 	{
	// 		if(isDash)
	// 		{
	// 			other.GetComponent<EnemyAI>().Die();
	// 		}
	// 		else
	// 		{
	// 			Die();
	// 		}	
	// 	}

	// 	if(other.CompareTag("SoundEnemy"))
	// 	{
	// 		if(isDash)
	// 		{
	// 			other.GetComponent<SoundEnemy>().Die();
	// 		}
	// 		else
	// 		{
	// 			Die();
	// 		}
	// 	}
	// }
	private void OnCollisionEnter(Collision other)
	{
		// if(other.collider.CompareTag("VisionEnemy"))
		// {
		// 	if(isDash)
		// 	{
		// 		other.collider.GetComponent<EnemyAI>().Die();
		// 	}
		// 	else
		// 	{
		// 		Die();
		// 	}	
		// }

		// if(other.collider.CompareTag("SoundEnemy"))
		// {
		// 	if(isDash)
		// 	{
		// 		other.collider.GetComponent<SoundEnemy>().Die();
		// 	}
		// 	else
		// 	{
		// 		Die();
		// 	}
		// }
		if(other.collider.CompareTag("Sea"))
		{
			transform.position = SavePointPos;
			hp = hp-15;
		}
		if(other.collider.CompareTag("Spike"))
		{
			gettingHit();
		}
	}
	void Resurrect()
	{
		gameObject.SetActive(true);
		currentState = PlayerState.Normal;
	}
	void getThrowDir()
	{
		Vector3 LJoyinput = Joysticks_Dir();

		if(LJoyinput!=Vector3.zero)
		{
			if(Vector3.Angle(ThrowDir,LJoyinput)<1.5f)
			{}
			else
			{
				ThrowDir = Vector3.SmoothDamp(ThrowDir,LJoyinput,ref RotateAngleSpeed,AimSmoothTime);
			}
			
		}	
	}
	void getKnockDir()
	{

		if(getHitByRight)
		{
			KnockDir = new Vector3(-Mathf.Cos(45*Mathf.Deg2Rad),Mathf.Sin(45*Mathf.Deg2Rad),0);
		}
		else
		{
			KnockDir = new Vector3(Mathf.Cos(45*Mathf.Deg2Rad),Mathf.Sin(45*Mathf.Deg2Rad),0);
		}
	}
	public void Die()
	{
		hp = hp_Max;
		transform.position = SavePointPos;
		//GameManager.ReloadScene();

		// gameObject.SetActive(false);
		// rb.velocity = Vector2.zero;
		// currentGas = 100;
		// Out_Of_Gas = false;
		// transform.position = StartPos;
		// currentState = PlayerState.Idle;

		// Invoke("Resurrect",1.0f);
	}
	
}
