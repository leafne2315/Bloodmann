using System.Collections;
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
	public float speed;
	public float moveInput_X;
	private float moveInput_Y;
	private float JumpInput;
	public Rigidbody2D rb;
	public Vector2 RealMovement;
	public bool facingRight = true;
	//偵測
	[Header("Detect Settings")]
	private bool isGrounded;
	private bool isAttachWall;
	private bool isAttachOnTop;
	public float checkRadius;
	public Transform GroundCheck;
	public Transform FrontCheck;
	public Transform UpCheck;
	public LayerMask WhatIsGround;
	public LayerMask WhatIsWall;
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
	public bool isFlying; 
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
	[SerializeField]private Vector2 DashDir;
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
	//
	//被攻擊
	[Header("UnderAttack Settings")]
	public float KnockTimer;
	public bool isHit;
	public bool isGhost = false;
	//
	//狀態控制
	[Header("Statement Settings")]
	public PlayerState currentState;
	private PlayerState LastState;
	public enum PlayerState{Normal,Defend,GetHit,Dash,Attach,BugFly,AirDash,Attack,Idle};
	private bool canAttach = true;
	[Range(0.0f,0.5f)]public float Attach_IntervalTime;
	//
	[Header("??? Settings")]
	private float OriginGravity;
	private float StableValue;
	//
	//燃料
	[Header("Gas Settings")]
	public float currentGas;
	public float Gas_MaxValue;
	public bool Out_Of_Gas;
	private bool RestoreGas_isOver = true;
	//
	//攻擊
	[Header("Attack Settings")]
	public bool canAttack = true;
	public float AttackAniTime = 0.5f;
	float AttackTimeCount = 0;
	public float AttackCD = 0.8f;
	public Transform hitPos;
	public LayerMask EnemyLayer;
	Vector3 HitBox_size = new Vector3(0.8f,1.0f,0f);
	
	private ExternalForce Ef;

	void Awake()
	{
		GameManager = GM.GetComponent<GameManager>();
		rb = GetComponent<Rigidbody2D>();
		OriginGravity = rb.gravityScale;
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
		
		
	}
	void Update()
	{
		switch(currentState)
		{
			case PlayerState.Idle:

			break;

			case PlayerState.Normal:
				
				CheckStability();
				RealMovement = new Vector2(Mathf.Lerp(rb.velocity.x,moveInput_X * speed,StableValue) , rb.velocity.y);
				rb.velocity = RealMovement + Ef.OtherForce;
				showflySpeed = rb.velocity.y;
				checkGravity();
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
				// 	RestoreGas();
				// }
				
				if(isAttachWall&&!isGrounded&&canAttach)
				{
					currentState = PlayerState.Attach;
					rb.gravityScale = 0;
					rb.velocity = Vector2.zero;
				}

				if(Input.GetMouseButtonDown(0)||Input.GetButtonDown("PS4-Triangle")&&!Out_Of_Gas&&(isAttachWall||isGrounded))//->Dash
				{	
					
					dashTimer = 0; //重置dash 時間
					dashSpeed = 0;
					currentState = PlayerState.Dash;
	
					// rb.gravityScale = 0;
					rb.velocity = Vector2.down*1.0f;
					//StartCoroutine(dashCD_Count());
					Arrow.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
					Arrow.GetComponent<ArrowShow>().LastDir = Vector3.up;
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
			break;

			case PlayerState.Attach:

				//isAttachWall = Physics2D.OverlapCircle(FrontCheck.position,0.05f,WhatIsWall);
				RestoreGas();

				if(Input.GetKeyUp(KeyCode.Q)||Input.GetButtonUp("PS4-L2"))//->Normal
				{
					ResetGravity();
					currentState = PlayerState.Normal;
					StartCoroutine(IntervalTime_Count());
				}

				if(Input.GetMouseButtonDown(0)||Input.GetButtonDown("PS4-Triangle"))//->Dash
				{	
				
					dashSpeed = 0;
					currentState = PlayerState.Dash;
					//rb.gravityScale = 2;
					rb.velocity = Vector2.zero;
					//StartCoroutine(dashCD_Count());
					Arrow.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
					Arrow.GetComponent<ArrowShow>().LastDir = Vector3.up;	
				}

				if(Input.GetKeyDown(KeyCode.C)||Input.GetButtonDown("PS4-L1")&&!Out_Of_Gas)
				{
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
					currentState = PlayerState.Normal;
					StartCoroutine(IntervalTime_Count());
				}

			break;

			case PlayerState.BugFly:

				FlyMovement();
				if(Input.GetButtonDown("PS4-Triangle")&&canAirDash)
				{
					isAirDash = true;
					currentState = PlayerState.AirDash;
					StartCoroutine(AirDash_Count());
					FlyDir = new Vector2(moveInput_X,moveInput_Y);
					canAirDash = false;
				}
				if(Input.GetKeyDown(KeyCode.V)||Input.GetButtonDown("PS4-L1"))
				{
					currentState = PlayerState.Normal;
					rb.gravityScale = OriginGravity;
					//FlyDir = Vector2.zero;
				}
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
					GasUse(40);
					if(dashSpeed<Max_dashSpeed)
					{
						dashSpeed+=75*Time.deltaTime;
					}
					else
					{
						dashSpeed = Max_dashSpeed;
					}

					dashTimer+=Time.deltaTime;

					if(Input.GetMouseButtonUp(0)||Input.GetButtonUp("PS4-Triangle")||dashTimer>Charge_MaxTime||Out_Of_Gas)
					{
						if(dashTimer<Dash_PreTime)//->Normal
						{
							currentState = PlayerState.Normal;
							ResetGravity();	
						}
						else
						{
							isDash = true;
						
							//Mouse_DirCache();
							Joysticks_DirCache();
						}

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
							rb.velocity = Vector2.zero;

							dashTimer = 0; //重置dash 時間
							currentState = PlayerState.Normal;
							isDash = false;
							ResetGravity();
						}
							
					}
					else if(dashTimer<dashTime+holdingTime)
					{
						dashTimer+=Time.deltaTime;
						rb.velocity =Vector2.Lerp(rb.velocity,Vector2.zero,0.1f);
						ResetGravity();
						if(isAttachWall)
						{
							rb.velocity = Vector2.zero;

							dashTimer = 0; //重置dash 時間
							currentState = PlayerState.Normal;
							isDash = false;
							ResetGravity();
						}		
					}
					else
					{
						dashTimer = 0; //重置dash 時間
						currentState = PlayerState.Normal;
						isDash = false;
						
						ResetGravity();			
					}
				}
			break;

			case PlayerState.GetHit:

				if(facingRight)
					KnockBack(20.0f,0.1f,new Vector2(-Mathf.Cos(30*Mathf.Deg2Rad),Mathf.Sin(30*Mathf.Deg2Rad)));
				else
				{
					KnockBack(20.0f,0.1f,new Vector2(Mathf.Cos(30*Mathf.Deg2Rad),Mathf.Sin(30*Mathf.Deg2Rad)));
				}

			break;

			case PlayerState.Attack:

				if(isFlying)
				{
					FlyDir = new Vector2(moveInput_X,moveInput_Y);
					rb.velocity = FlyDir *(flySpeed-5);
					if(AttackTimeCount<AttackAniTime)
					{
						AttackTimeCount+=Time.deltaTime;
					}
					else
					{
						currentState = LastState;
						AttackTimeCount = 0;
					}
				}
				else
				{
					if(AttackTimeCount<AttackAniTime+0.3f)
					{
						AttackTimeCount+=Time.deltaTime;
					}
					else
					{
						currentState = LastState;
						AttackTimeCount = 0;
					}
				}

			break;

			default:
			break;
		}

		isGrounded = Physics2D.OverlapCircle(GroundCheck.position,checkRadius,WhatIsGround);
		isAttachWall = Physics2D.OverlapCircle(FrontCheck.position,0.05f,WhatIsWall)||Physics2D.OverlapCircle(UpCheck.position,0.05f,WhatIsWall);
		isAttachOnTop = Physics2D.OverlapCircle(UpCheck.position,0.05f,WhatIsWall);
		
		isFlying = (currentState == PlayerState.AirDash||currentState == PlayerState.BugFly);

		CheckStability();
		moveInput_X = Input.GetAxis("PS4-L-Horizontal");
		moveInput_Y = Input.GetAxis("PS4-L-Vertical");
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

		if(!isAttachWall)
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

		if(Input.GetButtonDown("PS4-Square")||Input.GetKeyDown(KeyCode.Z))
		{
			if(canAttack)
			{
				canAttack = false;
				LastState = currentState;

				currentState = PlayerState.Attack;
				rb.velocity = Vector2.zero;

				StartCoroutine(AttackCD_Count());
				
				Collider2D[] hitObjs = Physics2D.OverlapBoxAll((Vector2)hitPos.position,(Vector2)HitBox_size,0.0f,EnemyLayer);
				if(hitObjs.Length==0)
				{
					print("Miss");
				}
				foreach(Collider2D c in hitObjs)
				{
					print("Hit"+c.name+"!!!!");
					c.GetComponent<tempGetHit>().isHit = true;
				}
			}
		}
	}
	void Flip()
	{
		facingRight = !facingRight;
		Vector3 Scaler = transform.localScale;
		Scaler.x*=-1;
		transform.localScale = Scaler;
	}
	public void KnockBack(float KnockPwr,float KnockDur,Vector2 KnockDir)
	{
		if(KnockTimer<KnockDur)
		{
			KnockTimer+=Time.deltaTime;
			rb.velocity = KnockDir*KnockPwr;
		}
		else
		{
			KnockTimer = 0;
			currentState = PlayerState.Normal;
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
	private void checkGravity()
	{
		if(rb.gravityScale!=OriginGravity)
		{
			rb.gravityScale = OriginGravity;
		}
	}
	private void ResetGravity()
	{
		rb.gravityScale = OriginGravity;
	}
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
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(FrontCheck.position,checkRadius);
		Gizmos.DrawWireCube(hitPos.position,HitBox_size);
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
	void Joysticks_DirCache()
	{
		//Vector3 L_Joy = new Vector3(Input.GetAxis("PS4-L-Horizontal"),Input.GetAxis("PS4-L-Vertical"),0.0f);
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
		rb.gravityScale = 0;
		FlyDir = Vector2.Lerp(FlyDir,new Vector2(moveInput_X,moveInput_Y),0.05f);
		if(Input.GetButton("PS4-R2"))
		{
			rb.velocity = FlyDir*40;
			GasUse(15);
		}
		else
		{
			rb.velocity = FlyDir*flySpeed;
			GasUse(40);
		}
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
	private void OnCollisionEnter2D(Collision2D other)
	{
		if(other.collider.CompareTag("VisionEnemy"))
		{
			if(isDash)
			{
				other.collider.GetComponent<EnemyAI>().Die();
			}
			else
			{
				Die();
			}	
		}

		if(other.collider.CompareTag("SoundEnemy"))
		{
			if(isDash)
			{
				other.collider.GetComponent<SoundEnemy>().Die();
			}
			else
			{
				Die();
			}
		}
		if(other.collider.CompareTag("Sea"))
		{
			Die();
		}
	}
	void Resurrect()
	{
		gameObject.SetActive(true);
		currentState = PlayerState.Normal;
	}
	public void Die()
	{
		GameManager.ReloadScene();

		// gameObject.SetActive(false);
		// rb.velocity = Vector2.zero;
		// currentGas = 100;
		// Out_Of_Gas = false;
		// transform.position = StartPos;
		// currentState = PlayerState.Idle;

		// Invoke("Resurrect",1.0f);
	}
}
