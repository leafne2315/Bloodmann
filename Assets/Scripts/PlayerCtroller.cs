using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using System;


public class PlayerCtroller : MonoBehaviour {
	public GameObject GM;
	private LevelLoader LevelLoader;
	public Transform RealWorldUICanVas;
	private GameManager GameManager;
	private InputManager IM;
	public GameObject inputmanager;
	private SavingAndLoad SLManager;
	private Vector3 StartPos;
	public GameObject Arrow;
	public GameObject Player3D;
	public Vector2 FlyDir;
	public int hp;
	public int hp_Max = 100;
	public int AidKitNum;
	public int AidKitNum_Max;
	public int Blood;
	public float speed;
	public float moveInput_X;
	private float moveInput_Y;
	private float JumpInput;
	private Vector3 MoveDir;
	public Rigidbody rb;
	public Vector2 RealMovement;
	public bool facingRight = true;
	public bool TouchGroundOnce;

	[Header("UI")]
	public Image CoreHalo;
	public Image GasBar;
	public Image GasBarBase;

	[Header("Collider Detail")]
	public float RollingHeight;
	public Vector3 RollingCenter;
	private float OriginalHeight;
	private Vector3 OriginalCenter;

	[Header("Effect Settings")]
 	public GameObject dashEffect;
	private ParticleSystem DashEffect;
	private ParticleSystem.EmissionModule emission;
	[SerializeField]private Quaternion originalRotation;
	
	//偵測
	[Header("Detect Settings")]
	public bool isRolling;
	public bool isGrounded;
	public bool isAttachWall;
	private bool isAttachOnTop;
	private Collider AttachingObj;
	public float checkRadius;
	public Transform GroundCheck;
	public Transform FrontCheck;
	public Transform UpCheck;
	public LayerMask WhatIsGround;
	public LayerMask WhatIsWall;
	public LayerMask WhatIsJar;

	[Header("Height Detect")]
	private float LandDist;
	private float HighestY;
	public float DamageHeight;
	public float PerDist;
	private float heightdamage;
	public float LandDamage;
	public float DamageUp;
	public bool isLandDamage;
	public float MaxLandDamage;

	[Header("HeavyLanding Detect")]
	public float LandingTime;
	public float quickLandTime;
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
	public Vector3 DashVel;

	[Header("ReBound Settings")]
	public float BoundSpeed;
	Vector3 BoundDir;
	public float BoundTime;
	public float ReboundAngle;
	public float StabbingTime;
	public bool isDownStab;
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
	public enum PlayerState{Normal,GetHit,Dash,Rebound,Attach,BugFly,AirDash,PreAttack,Attack,AfterAttack,Reloading,Roll,Throw,Recovery,BloodCollect,Idle,Rest,HeavyLanding,Die};
	public bool canAttach = true;
	public bool isFlying;
	public bool isStill;
	public bool isFlyAttack;
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
	public bool inAttacktime;
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

	[Header("Roll Settings")]
	public float RollTime;
	public float RollSpeed;
	public float SaveTime;
	public bool canRoll;
	public float RollCD;
	public float AfterRollTime;

	[Header("Recovery Settings")]
	public float RecoveryTime;
	public bool isRecovery;
	public int recoverAmount;
	public int damageAmount;

	[Header("Blood Collect Settings")]
	public bool canCollect;
	public float CollectTime;
	private float bloodAmount = 0;
	public bool CollectBegin;
	public GameObject BloodUI;
	public GameObject pf_BloodUI;
	public Image BloodBar;
	private GameObject currentActivateBloodPoint;
	private BloodPoint currentBp;
	[Header("Rest Settings")]
	public bool canRest;

	[Header("Die Settings")]
	public float TimeToReset;
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
	public bool noGravity = false;
	private SavePoint currentSp;
	private TranSceneCtrller currentTs;
	public bool canGoScene;
	private HealthBar healthBarScript;
 	public GameObject healthBar;
	[Header("VFX Setting")]
	public GameObject jumpDirtVFX;
	public Transform jumpDirtPos;
	public GameObject fallDirtVFX;
	public Transform fallDirtPos;
	public GameObject rollVFX;
	public Transform rollEffectPos;
	public bool isGroundedFrame;
	public AudioSource playerWalkSFX;
	public bool isPlayerWalkSFXPlaying;
	public AudioSource playerFlySFX;
	public bool isPlayerFlySFXPlaying;

	void Awake()
	{
		LevelLoader = GameObject.Find("LevelLoder").GetComponent<LevelLoader>();
		healthBarScript = healthBar.GetComponent<HealthBar>();
		DashEffect = GetComponentInChildren<ParticleSystem>();
		GameManager = GM.GetComponent<GameManager>();
		rb = GetComponent<Rigidbody>();
		//OriginGravity = rb.gravityScale;
		Ef = GetComponent<ExternalForce>();
		IM = inputmanager.GetComponent<InputManager>();
		SLManager = GameObject.Find("Save&Load").GetComponent<SavingAndLoad>();

		OriginalCenter = GetComponent<CapsuleCollider>().center;
		OriginalHeight = GetComponent<CapsuleCollider>().height;
	}
	void Start()
	{
		SLManager.LoadPlayerDetail();
		SLManager.SavePlayerSavePos();

		StartCoroutine(DelayForStart());

		originalRotation = Player3D.transform.localRotation;

		KnockTimer = 0;
		extraJumps = extraJumpValue;
		JumpTimer = JumpTime;
		currentState = PlayerState.Normal;
		currentGas = Gas_MaxValue;
		Arrow.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
		GasBar.enabled = false;
		GasBarBase.enabled = false;

		emission = DashEffect.emission;

		HighestY = transform.position.y;
	}
	void FixedUpdate()
	{
		if(currentState!=PlayerState.BugFly&&currentState!=PlayerState.Attach&&currentState!=PlayerState.Dash&&!noGravity&&currentState!=PlayerState.Rest)
		{
			if(!noGravity)
			{
				rb.AddForce(Physics.gravity*4.5f,ForceMode.Acceleration);
			}
			
		}
		
	}
	void Update()
	{
		//print(currentState);
		HpCheck();
		RestCheck();
		ChangeSceneCheck();
		
		checkAttackRemain();

		switch(currentState)
		{
			case PlayerState.Idle:

				

			break;

			case PlayerState.Normal:
				
				CheckStability();
				BloodCollectCheck();

				if(moveInput_X!=0)
				{
					if(moveInput_X>0)
					{
						MoveDir = Vector3.right;
					}
					else
					{
						MoveDir = Vector3.left;
					}
				}
				else
				{
					
					MoveDir = Vector3.zero;
				}

				RealMovement = new Vector2(Mathf.Lerp(rb.velocity.x,MoveDir.x * speed,StableValue) , rb.velocity.y);

				if(RealMovement.x!=0 &&!isPlayerWalkSFXPlaying&&isGrounded)
				{
					isPlayerWalkSFXPlaying = true;
					playerWalkSFX.Play();
				}
				else if(RealMovement.x == 0)
				{
					isPlayerWalkSFXPlaying = false;
					playerWalkSFX.Stop();
				}

				if(!isGrounded)
				{
					if(isPlayerFlySFXPlaying)
					{
						isPlayerWalkSFXPlaying = false;
						playerWalkSFX.Stop();
						
					}

					if(TouchGroundOnce)
					{
						TouchGroundOnce = false;
					}

				}

				
				RealMovementFix();

				
				rb.velocity = RealMovement + Ef.OtherForce;
				showflySpeed = rb.velocity.y;
				
				//AccelControll();
				LimitGSpeed();
				HeightCheck();

				if(isGrounded)
				{
					LandDist = HighestY-transform.position.y;

					if(LandDist>DamageHeight)
					{
						isLandDamage = true;

						heightdamage = (int)((LandDist-DamageHeight)/PerDist)*DamageUp + LandDamage;
						Mathf.Clamp(heightdamage,0,MaxLandDamage);

						Damage((int)heightdamage);
						GameObject sfx = Instantiate(Resources.Load("SoundPrefab/PlayerLandingHard") as GameObject, transform.position, Quaternion.identity);
						PlayerAni.SetBool("isHeavyLanding",true);
						currentState = PlayerState.HeavyLanding;
						isPlayerWalkSFXPlaying = false;
						playerWalkSFX.Stop();
						break;
					}
					else
					{
						isLandDamage = false;

						HighestY = transform.position.y;
					}		
				}

				if(isGrounded&&!TouchGroundOnce)
				{
					TouchGroundOnce = true;
					Instantiate(fallDirtVFX, fallDirtPos.position, Quaternion.identity);
					//落地
					GameObject sfx = Instantiate(Resources.Load("SoundPrefab/PlayerLanding") as GameObject, transform.position, Quaternion.identity);
				}
				
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
				if(Input.GetKeyDown(KeyCode.Space)||IM.PS4_X_Input && isGrounded == true)
				{
					transform.Find("JumpEffect").GetComponent<VisualEffect>().SendEvent("OnPlay");
					rb.velocity =  new Vector2 (moveInput_X*speed,JumpForce);
					Instantiate(jumpDirtVFX, jumpDirtPos.position, Quaternion.identity);
					PlayerAni.SetTrigger("Jump");
					IM.PS4_X_Input = false;
				}

				// if(isGrounded)
				// {
				// 		RestoreGas();
				// }
				if(IM.PS4_Up && AidKitNum>0)
				{
					IM.PS4_Up = false;
					currentState = PlayerState.Recovery;
					isRecovery = true;
					AidKitNum-=1;
					GameObject sfx = Instantiate(Resources.Load("SoundPrefab/PlayerHealing") as GameObject, transform.position, Quaternion.identity);

					float amount = recoverAmount/(float)hp_Max;
					healthBarScript.Healing(amount);
				}

				if(IM.PS4_O_Input)//->Roll
				{
					if(canRoll)
					{
						IM.PS4_O_Input = false;
						Roll();

						currentState = PlayerState.Roll;
					}
				}

				if(Input.GetMouseButtonDown(0)||IM.PS4_R2_KeyDown&&!isIneffective)//->Dash
				{	
					GameObject sfx = Instantiate(Resources.Load("SoundPrefab/PreDash") as GameObject, transform.position, Quaternion.identity);
					transform.Find("GasooEffect").GetComponent<VisualEffect>().SendEvent("OnPlay");
					dashTimer = 0; //重置dash 時間
					// dashSpeed = 0;
					currentState = PlayerState.Dash;
					PlayerAni.SetBool("DashPrepared",true);
					isPlayerWalkSFXPlaying = false;
					playerWalkSFX.Stop();

					// rb.gravityScale = 0;
					rb.velocity = Vector3.zero;
					//StartCoroutine(dashCD_Count());
					Arrow.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
					Arrow.GetComponent<ArrowShow>().LastDir = Vector3.up;
				}
/*
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
					PlayerAni.SetTrigger("Attach");
					
					rb.velocity = Vector3.zero;
				}
*/
				if(Input.GetKeyDown(KeyCode.C)||Input.GetButtonDown("PS4-L1")&&!Out_Of_Gas)
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
					isPlayerWalkSFXPlaying = false;
					playerWalkSFX.Stop();
				}

				if(IM.PS4_Square_Input||Input.GetKeyDown(KeyCode.Z))
				{
					rb.velocity = Vector3.zero;
					canAttack = false;
					//StartCoroutine(AttackCD_Count());
					currentState = PlayerState.PreAttack;
					PlayerAni.SetTrigger("Attack");
					isPlayerWalkSFXPlaying = false;
					playerWalkSFX.Stop();
					
				}

				if(Input.GetButtonDown("PS4-L2")&&isGrounded)
				{
					rb.velocity = Vector3.zero;

					if(AttackRemain!=FullRemain)
					{
						currentState = PlayerState.Reloading;
						PlayerAni.SetTrigger("Reload");
					}
					isPlayerWalkSFXPlaying = false;
					playerWalkSFX.Stop();
				}

			break;

			case PlayerState.Attach:

				//isAttachWall = Physics2D.OverlapCircle(FrontCheck.position,0.05f,WhatIsWall);
				rb.velocity = Vector3.zero;
				RestoreGas();

				if(Input.GetMouseButtonDown(0)||IM.PS4_R2_KeyDown&&!isIneffective)//->Dash
				{	
					transform.Find("GasooEffect").GetComponent<VisualEffect>().SendEvent("OnPlay");
					GameObject sfx = Instantiate(Resources.Load("SoundPrefab/PreDash") as GameObject, transform.position, Quaternion.identity);

					currentState = PlayerState.Dash;
					PlayerAni.SetBool("DashPrepared",true);
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

				if(IM.PS4_X_Input)
				{
					PlayerAni.SetTrigger("Jump");
					if(!isAttachOnTop)
					{
						if(!facingRight)
						{
							rb.velocity = Vector2.one*JumpForce;
						}
						else
						{
							rb.velocity = new Vector2(-1,1)*JumpForce;
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

				if(!isAttachWall)
				{
					currentState = PlayerState.Normal;
				}

			break;

			case PlayerState.BugFly:

				if(!isPlayerFlySFXPlaying&&!isGrounded)
				{
					isPlayerFlySFXPlaying = true;
					playerFlySFX.Play();
				}
				else if(isGrounded)
				{
					isPlayerFlySFXPlaying = false;
					playerFlySFX.Stop();
				}

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
					isPlayerFlySFXPlaying = false;
					playerFlySFX.Stop();
					//rb.gravityScale = OriginGravity;
					//FlyDir = Vector2.zero;
				}
				if(Input.GetMouseButtonDown(0)||IM.PS4_R2_KeyDown&&!isIneffective)//->Dash
				{	
					GameObject sfx = Instantiate(Resources.Load("SoundPrefab/PreDash") as GameObject, transform.position, Quaternion.identity);
					dashTimer = 0; //重置dash 時間
					// dashSpeed = 0;
					currentState = PlayerState.Dash;
					PlayerAni.SetBool("DashPrepared",true);
	
					// rb.gravityScale = 0;
					rb.velocity = Vector3.zero;
					//StartCoroutine(dashCD_Count());
					Arrow.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
					Arrow.GetComponent<ArrowShow>().LastDir = Vector3.up;
					transform.Find("GasooEffect").GetComponent<VisualEffect>().SendEvent("OnPlay");
					isPlayerFlySFXPlaying = false;
					playerFlySFX.Stop();
				}

				if(IM.PS4_Square_Input||Input.GetKeyDown(KeyCode.Z))
				{

					rb.velocity = Vector3.zero;
					currentState = PlayerState.PreAttack;
					PlayerAni.SetTrigger("Attack");

					isFlyAttack = true;
					isPlayerFlySFXPlaying = false;
					playerFlySFX.Stop();
				}

				if(Out_Of_Gas)
				{
					currentState = PlayerState.Normal;
					isPlayerFlySFXPlaying = false;
					playerFlySFX.Stop();
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

					dashTimer+=Time.deltaTime;


					if(Input.GetMouseButtonUp(0)||Input.GetButtonUp("PS4-R2"))
					{
						if(dashTimer<Dash_PreTime)//->Normal
						{
							currentState = PlayerState.Normal;
							PlayerAni.SetBool("DashPrepared",false);
							transform.Find("GasooEffect").GetComponent<VisualEffect>().SendEvent("OnStop");
						}
						else
						{
							emission.enabled = true;
							transform.Find("GasooEffect").GetComponent<VisualEffect>().SendEvent("OnStop");
							GameObject sfx = Instantiate(Resources.Load("SoundPrefab/Dash") as GameObject, transform.position, Quaternion.identity);

							isDash = true;
							
							PlayerAni.SetBool("isDash",true);
							PlayerAni.SetBool("DashPrepared",false);
							
							//Mouse_DirCache();
							GetDashDir();
							if(facingRight)
							{
								
								Player3D.transform.rotation = Quaternion.LookRotation(DashDir,Vector3.left);
							}
							else
							{
								Player3D.transform.rotation = Quaternion.LookRotation(DashDir,Vector3.right);
							}
							
							
							StartCoroutine(IntervalTime_Count());
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

						DashAttack();
						if(hitConfirm)
						{
							dashTimer = 0;
							hitConfirm = false;

							emission.enabled = false;
							transform.Find("BloodEffect").GetComponent<VisualEffect>().SendEvent("OnPlay");
							HitEffect();
							GameObject sfx = Instantiate(Resources.Load("SoundPrefab/DashATKHit") as GameObject, transform.position, Quaternion.identity);
							
							isDash = false;
							PlayerAni.SetBool("isDash",false);
							getBoundDir();
							rb.velocity = Vector3.zero;
							
							currentState = PlayerState.Rebound;
							Bound();
							Player3D.transform.localRotation = originalRotation;//reset rotate
							PlayerAni.SetBool("isStabbing",true);
							AttackRemain--;
							
							break;
						}	

						if(isGrounded && dashTimer>0.2f)
						{
							currentState = PlayerState.Normal;
							Player3D.transform.localRotation = originalRotation;//reset rotate
							rb.velocity = Vector3.zero;
							AttackRemain--;

							emission.enabled = false;
							dashTimer = 0; //重置dash 時間
							isDash = false;
							PlayerAni.SetBool("isDash",false);
							print("Dashing Hit Ground");
						}
						else if(isAttachWall&&canAttach&&!isGrounded)
						{
							currentState = PlayerState.Attach;
							Player3D.transform.localRotation = originalRotation;//reset rotate
							canAttach = false;
							AttackRemain--;

							PlayerAni.SetTrigger("Attach");
							rb.velocity = Vector3.zero;
							
							// Debug.LogError("Attach!!");
							emission.enabled = false;
							dashTimer = 0; //重置dash 時間
							isDash = false;
							PlayerAni.SetBool("isDash",false);

							print("DashingTime Attach");
						}

						
					}
					else if(dashTimer<dashTime+holdingTime)
					{
						dashTimer+=Time.deltaTime;
						rb.velocity = DashDir * Mathf.Lerp(dashSpeed,0.0f,(dashTimer-dashTime)/holdingTime);
						
						if(isAttachWall&&canAttach)
						{
							rb.velocity = Vector2.zero;
							

							dashTimer = 0; //重置dash 時間
							emission.enabled = false;
							currentState = PlayerState.Attach;
							canAttach = false;
							AttackRemain--;
							Player3D.transform.localRotation = originalRotation;//reset rotate
							
							isDash = false;
							PlayerAni.SetBool("isDash",false);
							PlayerAni.SetTrigger("Attach");

							print("HoldingTime Attach");
						}		
					}
					else
					{
						dashTimer = 0; //重置dash 時間
						

						emission.enabled = false;
						currentState = PlayerState.Normal;
						Player3D.transform.localRotation = originalRotation;//reset rotate
						AttackRemain--;
						isDash = false;
						PlayerAni.SetBool("isDash",false);

						print("No Attach");
					}
				}

			break;

			case PlayerState.HeavyLanding:

				rb.velocity = Vector3.zero;

				

				if(Timer<LandingTime)
				{
					Timer+=Time.deltaTime;

					if(Timer>=quickLandTime)
					{
						if(IM.PS4_O_Input)//->Roll
						{
							if(canRoll)
							{
								getMoveInput();
								Roll();
								currentState = PlayerState.Roll;
								PlayerAni.SetBool("isHeavyLanding",false);
								Timer = 0;
							}
						}
					}
				}
				else
				{
					Timer = 0;
					currentState = PlayerState.Normal;
					PlayerAni.SetBool("isHeavyLanding",false);
				}

			break;
			
			case PlayerState.Rebound:

				isInvincible = true;
				if(Timer<BoundTime)
				{
					Timer+=Time.deltaTime;
				}
				else
				{
					currentState = PlayerState.Normal;
					isInvincible = false;
					PlayerAni.SetBool("isStabbing",false);
					Timer = 0;
				}
				/*
				if(Timer<StabbingTime)
				{
					noGravity = true;
					rb.velocity = Vector3.zero;

					Timer+=Time.deltaTime;
					
				}
				else if(Timer<StabbingTime+BoundTime)
				{
					noGravity = false;
					Timer+=Time.deltaTime;

					rb.velocity = BoundDir*BoundSpeed;
					rb.AddForce(BoundDir*BoundSpeed,ForceMode.)

				}
				else
				{
					Timer = 0;
					currentState = PlayerState.Normal;
					isInvincible = false;
					PlayerAni.SetBool("Rebound",false);
					print("a");
				}
				*/

			break;

			case PlayerState.Roll:

				if(Timer<RollTime)
				{
					if(Timer<SaveTime)
					{
						
					}
					else
					{
						isInvincible = false;
					}

					Timer+=Time.deltaTime;
				}
				else if(Timer<RollTime+AfterRollTime)
				{
					ResetCollider();
					Timer+=Time.deltaTime;
					rb.velocity = new Vector3(0.0f,rb.velocity.y,0.0f);
				}
				else 
				{
					Timer=0;
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

				if(isFlyAttack)
				{
					FlyMovement();
				}

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
					GameObject sfx = Instantiate(Resources.Load("SoundPrefab/PlayerATK") as GameObject, transform.position, Quaternion.identity);
				}


			break;

			case PlayerState.Attack:
				
				if(isFlyAttack)
				{
					FlyMovement();
				}

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
					inAttacktime = false;
				}


			break;

			case PlayerState.AfterAttack:

				if(isFlyAttack)
				{
					FlyMovement();
				}

				if(Timer<AfterAttackTime)
				{
					Timer+=Time.deltaTime;
				}
				else
				{
					Timer = 0;
					hitConfirm = false;

					if(isFlyAttack)
					{
						currentState = PlayerState.BugFly;
						isFlyAttack = false;
					}
					else
					{
						currentState = PlayerState.Normal;
					}

					
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
					GameObject sfx = Instantiate(Resources.Load("SoundPrefab/DashReload") as GameObject, transform.position, Quaternion.identity);
					AttackRemain = FullRemain;
					currentState = PlayerState.Normal;
				}
				
			break;

			case PlayerState.Recovery:

				if(Timer<RecoveryTime)
				{
					rb.velocity = Vector3.zero;

					Timer+= Time.deltaTime;
				}
				else
				{
					Timer = 0;
					Recover(recoverAmount);
					GameObject sfx = Instantiate(Resources.Load("SoundPrefab/PlayerAfterHealing") as GameObject, transform.position, Quaternion.identity);
					isRecovery = false;
					
					currentState = PlayerState.Normal; 
				}

			break;

			case PlayerState.BloodCollect:

				rb.velocity = Vector3.zero;

				if(CollectBegin)
				{
					PlayerAni.SetBool("CollectingBlood",true);
					CollectingBlood();
				}

			break;

			case PlayerState.Rest:

				rb.velocity = Vector3.zero;

				if(IM.PS4_O_Input)
				{
					//change motion
					currentState = PlayerState.Normal;
					IM.currentState = InputManager.InputState.InGame;
					PlayerAni.SetBool("isRest",false);
					transform.position = new Vector3(transform.position.x,transform.position.y,0.0f);
					
					currentSp.canActivate = true;
					currentSp.SavePointMenu_Close();
				}

			break;

			case PlayerState.Die:

				if(Timer<TimeToReset)
				{
					//wait time
					Timer+= Time.deltaTime;

				}
				else
				{
					//fade scene
					LevelLoader.ReloadScene();
					print("reload");
					currentState = PlayerState.Idle;
				}

			break;

			default:
			break;
		}


		isGrounded = Physics.CheckSphere(GroundCheck.position,checkRadius,WhatIsGround);
		//isAttachOnTop = Physics.CheckSphere(UpCheck.position,0.05f,WhatIsWall);
		isAttachWall = Physics.CheckSphere(FrontCheck.position,0.05f,WhatIsWall);
		
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
		isRolling = currentState== PlayerState.Roll;

		

		CheckStability();
		BooleanCorrectCheck();

		if(!isAttacking&&!isRolling && currentState!=PlayerState.Roll && currentState!=PlayerState.Recovery&& currentState!=PlayerState.Rest 
			&& currentState!=PlayerState.HeavyLanding &&currentState!=PlayerState.Rebound &&currentState!=PlayerState.Dash 
			&&currentState!=PlayerState.Die &&currentState!=PlayerState.Idle && IM.isInGameInput)
		{
			getMoveInput();
		}
		

		if(currentState!=PlayerState.Normal)
		{
			RealMovementReset();

			HighestY = transform.position.y;
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

		RayToGround();
		/*
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
		*/
		
	}
	void LowerCollider()
	{
		CapsuleCollider cc = GetComponent<CapsuleCollider>();
		GetComponent<CapsuleCollider>().height = RollingHeight;
		GetComponent<CapsuleCollider>().center = RollingCenter;
	}
	void ResetCollider()
	{
		CapsuleCollider cc = GetComponent<CapsuleCollider>();
		cc.height = OriginalHeight;
		cc.center = OriginalCenter;
	}
	void HitEffect()
	{
		Vector3 HitEffectRightAngle;
       	HitEffectRightAngle = new Vector3(0f,20f,40f);
       	Vector3 HitEffectLeftAngle;
       	HitEffectLeftAngle = new Vector3(0f,20f,-40f);

       	if(facingRight)
		{
			transform.Find("HitEffect").GetComponent<VisualEffect>().SetVector3("HitEffectAngle", HitEffectRightAngle);
		}
		else
		{
			transform.Find("HitEffect").GetComponent<VisualEffect>().SetVector3("HitEffectAngle", HitEffectLeftAngle);
		}
		transform.Find("HitEffect").GetComponent<VisualEffect>().SendEvent("OnPlay");
	}
	void Bound()
	{
		rb.AddForce(BoundDir*BoundSpeed,ForceMode.VelocityChange);
	}
	void RayToGround()
	{
		RaycastHit hit;
		if(Physics.Raycast(transform.position,Vector3.down,out hit,WhatIsGround))
		{
			
			if(hit.distance>DamageHeight)
			{
				Debug.DrawLine(GroundCheck.position,hit.point,Color.red);
			}
			else
			{
				Debug.DrawLine(GroundCheck.position,hit.point,Color.green);
			}
			
		}
	}
	
	void HeightCheck()
	{
		
		if(HighestY<transform.position.y)
		{
			HighestY = transform.position.y;
		}

	}
	public void StartCollect()
	{
		Vector3 WorldPos = transform.position + Vector3.right*1.6f + Vector3.up*1f;
		Vector3 UI_pos = new Vector3(WorldPos.x,WorldPos.y);
		BloodUI = Instantiate(pf_BloodUI,UI_pos,Quaternion.identity,RealWorldUICanVas);
		BloodBar = BloodUI.transform.GetChild(1).GetComponent<Image>();

		GameObject sfx = Instantiate(Resources.Load("SoundPrefab/PlayerCollectBlood") as GameObject, transform.position, Quaternion.identity);
		CollectBegin = true;
	}
	void CollectingBlood()
	{
		if(Timer<CollectTime)
		{
			Timer+=Time.deltaTime;

			bloodAmount += (1/CollectTime)*Time.deltaTime;
			BloodBar.fillAmount = bloodAmount;

			print(BloodBar.fillAmount);
		}
		else
		{
			Timer = 0;

			bloodAmount = 0;
			Blood += currentBp.bloodStock;

			IM.currentState = InputManager.InputState.InGame;

			CollectBegin = false;
			StartCoroutine(BloodCollectUI_Remove());
			currentState = PlayerState.Normal;
			currentBp.canActivate = false;
			
		}
	}
	IEnumerator BloodCollectUI_Remove()
	{
		for(int i =0;i<BloodUI.transform.childCount;i++)
		{
			BloodUI.transform.GetChild(i).GetComponent<Image>().CrossFadeAlpha(0.0f,0.5f,false);
		}

		for(float i =0 ; i<=1 ; i+=Time.deltaTime)
		{
			yield return 0;
		}
		Destroy(BloodUI.gameObject);
	}
	void BloodCollectCheck()
	{
		if(canCollect)
		{
			if(IM.PS4_Triangle_Input && isGrounded )
			{
				IM.currentState = InputManager.InputState.CollectingBlood;
				currentState = PlayerState.BloodCollect;
				
				currentBp.closeActivateUI();
				//採血動畫-->觸發startCollect
				//以下測試用
				StartCollect();
			}
		}
	}
	void RestCheck()
	{
		if(canRest)
		{
			if(IM.PS4_Triangle_Input && isGrounded)
			{
				IM.currentState = InputManager.InputState.SavePointMenu;

				SLManager.SavePlayerSavePos();

				hp = hp_Max;
				healthBarScript.damageBarImage.fillAmount = 1;
				healthBarScript.healthBar.fillAmount = 1;

				currentSp.SavePointMenu_Open();
				currentSp.closeActivateUI();
				currentSp.canActivate = false;
				transform.position = currentSp.RestPos.position;
				PlayerAni.SetBool("isRest",true);

				currentState = PlayerState.Rest;
			}
		}
	}
	void ChangeSceneCheck()
	{
		if(canGoScene)
		{
			if(IM.PS4_Triangle_Input && isGrounded)
			{
				IM.PS4_Triangle_Input = false;
				currentState = PlayerState.Idle;
				
				currentTs.ChangeScene();
				SLManager.SavePlayerDetail();

				print("Go other Scene");
			}
		}
	}
	void HpCheck()
	{
		hp = Mathf.Clamp(hp,0,100);

		if(hp<=0)
		{
			Die();
		}
	}
	void Recover(int recoveryAmount)
	{
		hp+=recoveryAmount;
	}
	
	public void Damage(int damageAmount)
	{
		hp -= damageAmount;
		healthBarScript.Damaging();
	}
	void Roll()
	{
		isInvincible = true;
		canRoll = false;
		StartCoroutine(RollCD_Count());
		PlayerAni.SetTrigger("Roll");
		LowerCollider();
		GameObject sfx = Instantiate(Resources.Load("SoundPrefab/PlayerRoll") as GameObject, transform.position, Quaternion.identity);

		if(moveInput_X!=0)
		{
			Vector3 rollDir;

			if(moveInput_X>0)
			{
				rollDir = Vector3.right;

				Instantiate(rollVFX, rollEffectPos.position, Quaternion.Euler(0,0,76));
			}
			else
			{
				rollDir = Vector3.left;

				Instantiate(rollVFX, rollEffectPos.position, Quaternion.Euler(0,0,-76));
			}
			
			rb.velocity = rollDir*RollSpeed;
		}
		else
		{
			if(facingRight)
			{
				rb.velocity = Vector3.right*RollSpeed;

				Instantiate(rollVFX, rollEffectPos.position, Quaternion.Euler(0,0,76));
			}
			else
			{
				rb.velocity = Vector3.left*RollSpeed;

				Instantiate(rollVFX, rollEffectPos.position, Quaternion.Euler(0,0,-76));
			}
		}
		
	}
	IEnumerator RollCD_Count()
	{
		for(float i =0 ; i<=RollCD ; i+=Time.deltaTime)
		{
			yield return 0;
		}
		canRoll = true;
	}
	void getBoundDir()
	{
		if(DashDir.x>0)
		{
			if(DashDir.y>0.5f)
			{
				BoundDir = new Vector3(-Mathf.Cos(ReboundAngle*Mathf.Deg2Rad),Mathf.Sin(ReboundAngle*Mathf.Deg2Rad),0);
			}
			else
			{
				BoundDir = new Vector3(-Mathf.Cos(ReboundAngle*Mathf.Deg2Rad),Mathf.Sin(ReboundAngle*Mathf.Deg2Rad),0);
			}
		}
		else
		{
			if(DashDir.y>0.5f)
			{
				BoundDir = new Vector3(Mathf.Cos(ReboundAngle*Mathf.Deg2Rad),Mathf.Sin(ReboundAngle*Mathf.Deg2Rad),0);
			}
			else
			{
				BoundDir = new Vector3(Mathf.Cos(ReboundAngle*Mathf.Deg2Rad),Mathf.Sin(ReboundAngle*Mathf.Deg2Rad),0);
			}
		}
	}
	void DashAttack()
	{
		Vector3 attackPos = transform.position + DashDir*1.5f;
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
						StartCoroutine(c.GetComponent<tempGetHit>().StabbedTrigger());
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
			CoreHalo.enabled = true;
		}
		else
		{
			CoreHalo.enabled = false;
			isIneffective = false;
		}
	}
	void getMoveInput()
	{
		if(IM.currentState == InputManager.InputState.InGame)
		{
			moveInput_X = Input.GetAxis("PS4-L-Horizontal");
			moveInput_Y = Input.GetAxis("PS4-L-Vertical");
		}
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
			
			inAttacktime = true;
			GameObject Ftx = Instantiate(AttackFTX,currentHitPos.position,AttackAngle);
			Ftx.transform.SetParent(transform);
	}
	void getHitReset()
	{
		transform.Find("GasooEffect").GetComponent<VisualEffect>().SendEvent("OnStop");

		if(Player3D.transform.localRotation != originalRotation)
		{
			Player3D.transform.localRotation = originalRotation;
		}

		if(isRecovery)
		{
			isRecovery = false;
			healthBarScript.InterruptRecover();
		}

		if(CollectBegin)
		{
			CollectBegin = false;
			IM.currentState = InputManager.InputState.InGame;

			bloodAmount = 0;//顯示數值重置
			StartCoroutine(BloodCollectUI_Remove());
		}
	}
	void AttackHitCheck()
	{	
		
		if(!hitConfirm)
		{
			Collider[] hitObjs = Physics.OverlapBox(currentHitPos.position,HitBox_size,AttackAngle,EnemyLayer);
			
			if(hitObjs.Length==0)
			{
				//print("Miss");
			}
			else
			{
				hitConfirm = true;
				HitEffect();
				GameObject sfx = Instantiate(Resources.Load("SoundPrefab/PlayerATK_Monster") as GameObject, transform.position, Quaternion.identity);

				foreach(Collider c in hitObjs)
				{
					transform.Find("BloodEffect").GetComponent<VisualEffect>().SendEvent("OnPlay");
					print("Hit"+c.name+"!!!!");
					StartCoroutine(c.GetComponent<tempGetHit>().HitTrigger(AttackDir));
				}
			}
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

	void RealMovementReset()
	{
		if(RealMovement.x!=0)
			RealMovement.x = 0.0f;
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
		GasBar.enabled = true;
		GasBarBase.enabled = true;
		if(currentGas>0)
			currentGas-=comsumePS*Time.deltaTime;
		else
		{
			Out_Of_Gas = true;
		}
	}
	
	void RestoreGas()
	{
		GasBar.enabled = true;
		GasBarBase.enabled = true;
		if(currentGas<Gas_MaxValue)
		{
			currentGas+=60*Time.deltaTime;

			if(Out_Of_Gas)
				Out_Of_Gas = false;	
		}
		else
		{
			currentGas = Gas_MaxValue;
			GasBar.enabled = false;
			GasBarBase.enabled = false;
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
		DashDir = Arrow.GetComponent<ArrowShow>().LastDir.normalized;
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
		if(Input.GetButton("PS4-R1"))
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
	void resetAniTrigger()
	{
		PlayerAni.ResetTrigger("Reload");
		PlayerAni.ResetTrigger("Jump");
		PlayerAni.ResetTrigger("Attack");
		PlayerAni.ResetTrigger("Roll");
		PlayerAni.ResetTrigger("Attach");
		
		
		PlayerAni.SetBool("isStabbing",false);
		PlayerAni.SetBool("Rebound",false);
		PlayerAni.SetBool("DashPrepared",false);
		PlayerAni.SetBool("isRecovery",false);
		PlayerAni.SetBool("isHeavyLanding",false);
	}
	void ResetAllTimer()
	{
		Timer = 0;
		dashTimer = 0;
		JumpTimer = 0;
	}
	public void gettingHit()
	{
		if(!isInvincible)
		{
			resetAniTrigger();
			ResetAllTimer();
			getHitReset();
			PlayerAni.SetTrigger("getHit");

			isInvincible = true;
			Damage(damageAmount);
			//StartCoroutine(HitTrigger());
			StartCoroutine(ReviveTime_Count());
			LastState = currentState;
			currentState = PlayerState.GetHit;
			transform.Find("GetHitEffect").GetComponent<VisualEffect>().SendEvent("OnPlay");
			GameObject sfx = Instantiate(Resources.Load("SoundPrefab/PlayerGetHit") as GameObject, transform.position, Quaternion.identity);
			//UI 關閉
			emission.enabled = false;
   			Arrow.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

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
	void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("SavePoint"))
		{
			if(other.GetComponent<SavePoint>().showUI)
			{
				canRest = true;
				currentSp = other.GetComponent<SavePoint>();	
			}
		}

		if(other.CompareTag("BloodPoint"))
		{
			if(other.GetComponent<BloodPoint>().showUI)
			{
				canCollect = true;
				currentBp = other.transform.GetComponent<BloodPoint>();
			}
		}

		if(other.CompareTag("TransScene"))
		{
			if(other.GetComponent<TranSceneCtrller>().showUI)
			{
				canGoScene = true;
				currentTs = other.GetComponent<TranSceneCtrller>();
			}
		}
		
	}
	void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("SavePoint"))
		{
			canRest = false;
		}
		if(other.CompareTag("BloodPoint"))
		{
			canCollect = false;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(FrontCheck.position,checkRadius);
		if(inAttacktime)
		{
			Gizmos.color = Color.red;
		}
		else
		{
			Gizmos.color = Color.white;
		}
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
	IEnumerator DelayForStart()
	{
		currentState = PlayerState.Idle;
		yield return new WaitForSeconds(2.5f);
		currentState = PlayerState.Normal;
	}
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
			
			Damage(damageAmount);
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
	void BooleanCorrectCheck()
	{
		if(currentState!=PlayerState.Dash)
		{
			isDash = false;
		}
	}
	public void Die()
	{
		gameObject.tag = "DeadObject";
        gameObject.layer = LayerMask.NameToLayer("DeadObject");

		currentState = PlayerState.Die;
		rb.velocity = Vector3.zero;
	}
	IEnumerator GroundedFrame()
    {
        for(int i = 0;i<1;i++)
		{
			yield return new WaitForEndOfFrame();
		}
        isGroundedFrame = false;
		//transform.GetChild(6).GetComponent<VisualEffect>().SendEvent("OnPlay");
    }
	
}