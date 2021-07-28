using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
public class BossController : MonoBehaviour{
    // Start is called before the first frame update
    public BossState currentState;
	public enum BossState{Idle,Move,QuickBack,TwiceAttack,AirDown,DashAttack,ShootAir,Razer,KnockDown};
    private PlayerCtroller PlayerCtroller;
    public GameObject Player;
    public LayerMask PlayerLayer;
    private Rigidbody rb;
    public bool facingRight;
    public float timer;
    public bool isFighting;
    public Animator BossAni;
    public GameObject Canvas;
    public bool notDie;
    [Header("Data")]
    public float hp;
    [Header("Detect Settings")]
    public bool isClose;
    public bool isFar;
    public float FarRange;
    private bool DeriveCheck;

    [Header("Move Settings")]
    public float MoveSpeed;
    public Vector3 MoveDir;
    public float MoveTime;

    [Header("TwiceAttack Settings")]
    public bool isAttack;
    public float AttackMoveSpeed;
    public Vector3 HitBox_size;
    public float AttackStart_1;
    public float AttackEnd_1;
    public float AttackStart_2;
    public float AttackEnd_2;
    public float TA_StateTime;
    public bool hitConfirm;
    public Transform HitPos;
    public GameObject FirstHit;
    public GameObject SecondHit;

    [Header("AirDown Settings")]
    public float WaitToJump;
    public float JumpUpTime;
    public float StayUpTime;
    private Vector3 targetAirPos;
    public float Height; 
    public float DownSpeed;
    public float AD_StateTime;
    private bool isAirDownDamage;
    public Vector3 AirD_hitBox;
    public GameObject AD_VFX;

    [Header("DashAttack Settings")]
    public Transform DA_hitPos;
    public Vector3 DA_hitBox;
    public float DA_Speed;
    public float DA_Time;
    public float DA_Dir_Start;
    private Vector3 DA_Dir;
    public float DA_StateTime;
    public GameObject DaFTX_Pf;
    public Transform DAFTX_GeneratePos;
    public VisualEffect currentDaFTX;

    [Header("ShootAir")]
    public float StartAim;
    private bool isAiming;
    private bool isShoot;
    public float StartShoot;
    public float ShootDur;
    public float SA_StateTime;
    public GameObject AimUIPf;
    private GameObject currentAim;
    public GameObject ShootFTX;
    public Vector3 ShootAir_HitBox;
    [Header("Razer Settings")]
    public Vector3 RazerDir;
    public float Razer_startT;
    public float RazerDur;
    public float Razer_StateT;
    public float SweepAngle;
    public LayerMask RazerLayer;
    public bool canUseRazer;
    public float RazerCD;
    private float RazerLength;
    public float RazerWidth;
    private float currentAngle;
    public Transform shootPos;
    public GameObject Razer_beforeVFX;
    public GameObject Razer_VFX;
    private bool isDamaging;

    [Header("QuickBack")]
    public float BackTime;
    private Vector3 BackDir;
    public float BackSpeed;
    private float currentBackSpeed;
    public float AnimationBackTime;
    public float QB_StateTime;
    
    void Awake()
    {
        PlayerCtroller = Player.GetComponent<PlayerCtroller>();
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        if(currentState!=BossState.AirDown)
        {
            rb.AddForce(Physics.gravity*4.5f,ForceMode.Acceleration);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckRange();
        

        if(Input.GetKeyDown(KeyCode.K))
        {
            currentState = BossState.ShootAir;
            timer = 0;
            ResetAniTrigger();
            BossAni.SetTrigger("AirShoot");
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            timer = 0;
            currentState = BossState.Razer;
            rb.velocity = Vector3.zero;
            BossAni.SetTrigger("Razer");
            Razer_OnStart();
        }

        

        switch(currentState)
        {
            case BossState.Move:

                getMoveDir();
                facingCheck();
                
                if(canUseRazer)
                {
                    if(isClose)
                    {
                        BossAni.SetTrigger("QuickBack");
                        currentState = BossState.QuickBack;
                        rb.velocity = Vector3.zero;
                        BackDir = new Vector3(-MoveDir.x,0,0);
                        timer = 0;
                        break;
                    }

                    currentState = BossState.Razer;
                    rb.velocity = Vector3.zero;
                    timer = 0;
                    ResetAniTrigger();
                    BossAni.SetTrigger("Razer");
                    Razer_OnStart();
                    break;
                }
                
                if(timer<MoveTime)
                {
                    timer+=Time.deltaTime;

                    rb.velocity = MoveDir*MoveSpeed;
                }
                else
                {

                    if(isClose)
                    {
                        int RandomNum = Random.Range(0,101);
                        print(RandomNum);

                        if(RandomNum<=40)
                        {
                            //-->Twice Attack
                            BossAni.SetTrigger("DoubleAttack");
                            currentState = BossState.TwiceAttack;
                            rb.velocity = Vector3.zero;
                        }
                        else if(RandomNum>40&&RandomNum<=70)
                        {
                            //-->Quick Back
                            BossAni.SetTrigger("QuickBack");
                            currentState = BossState.QuickBack;
                            rb.velocity = Vector3.zero;
                            BackDir = new Vector3(-MoveDir.x,0,0);
                        }
                        else
                        {
                            DA_Dir = MoveDir;
                            currentState = BossState.DashAttack;
                            BossAni.SetTrigger("DashAttack");
                        }
                    }
                    else
                    {
                        int RandomNum = Random.Range(0,101);
                        print(RandomNum);
                        if(RandomNum<=30)
                        {
                            DA_Dir = MoveDir;
                            currentState = BossState.DashAttack;
                            BossAni.SetTrigger("DashAttack");
                        }
                        else if(RandomNum>30&&RandomNum<=60)
                        {
                            
                            currentState = BossState.AirDown;
                            BossAni.SetTrigger("AirDown");
                        }
                        else
                        {
                            currentState = BossState.ShootAir;
                            timer = 0;
                            ResetAniTrigger();
                            BossAni.SetTrigger("AirShoot");
                        }
                    }
                    timer = 0;
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
                    rb.velocity = AttackMoveSpeed*MoveDir;
                    AttackHitCheck();
                }
                else if(timer<AttackStart_2)
                {
                    //attack_end1~attack_start2
                    rb.velocity  = Vector3.zero;
                    hitConfirm = false;
                    isAttack = false;
                }
                else if(timer<AttackEnd_2)
                {
                    //Attack_2 start~end
                    rb.velocity = AttackMoveSpeed*MoveDir;
                    AttackHitCheck();
                }
                else if(timer<TA_StateTime)
                {
                    //attack_2 End ~ StateTime
                    rb.velocity  = Vector3.zero;
                    hitConfirm = false;
                    isAttack = false;
                }
                else
                {
                    timer = 0;
                    currentState = BossState.Move;
                    BossAni.SetTrigger("BackToWalk");
                    //next state
                }
                
            break;

            case BossState.AirDown:

                timer+= Time.deltaTime;

                if(timer<WaitToJump)
                {
                    rb.velocity = Vector3.zero;
                    targetAirPos = Player.transform.position;
                    targetAirPos.y = 228f+Height;
                }
                else if(timer<WaitToJump+JumpUpTime)
                {
                    transform.position = Vector3.Lerp(transform.position,targetAirPos,(timer-WaitToJump)/JumpUpTime);
                }
                else if(timer<WaitToJump+JumpUpTime+StayUpTime)
                {
                    rb.velocity = Vector3.zero;
                }
                else if(timer<AD_StateTime)
                {
                    rb.velocity = Vector3.down*DownSpeed;
                }
                else
                {
                    timer = 0;
                    currentState = BossState.Move;
                    BossAni.SetTrigger("BackToWalk");
                    //next state
                    isAirDownDamage = false;
                }

                if(isAirDownDamage)
                {
                    AirDownHit();
                }


            break;

            case BossState.DashAttack:

                timer += Time.deltaTime;
                if(timer<DA_Dir_Start)
                {
                    rb.velocity = Vector3.zero;
                }
                else if(timer<DA_Dir_Start +DA_Time)
                {
                    rb.velocity = DA_Dir*DA_Speed;
                    //DA
                    DashAttackHit();


                    currentDaFTX = Instantiate(DaFTX_Pf,DAFTX_GeneratePos.position,Quaternion.identity).GetComponent<VisualEffect>();

                    Vector3 a = currentDaFTX.GetVector3("VelocityA");
                    currentDaFTX.SetVector3("VelocityA",a*transform.localScale.x);
                    Vector3 b = currentDaFTX.GetVector3("VelocityB");
                    currentDaFTX.SetVector3("VelocityB",b*transform.localScale.x);

                    Destroy(currentDaFTX.gameObject,3.0f);
                    // DaFTX.SetVector3("Custom Space",transform.localScale);
                    // DaFTX.SendEvent("OnPlay");
                }
                else if(timer<DA_StateTime)
                {
                    //DA_over
                    rb.velocity = Vector3.zero;
                }
                else
                {
                    timer = 0;
                    currentState = BossState.Move;
                    BossAni.SetTrigger("BackToWalk");
                    //next state
                }

            break;

            case BossState.Razer:

                if(timer<Razer_startT)
                {
                    //wait for shootRazer
                    timer+=Time.deltaTime;

                    RaycastHit hit;
                    if(Physics.Raycast(shootPos.position,RazerDir,out hit,Mathf.Infinity,RazerLayer))
                    {
                        Debug.DrawLine(shootPos.position,hit.point,Color.red);
                        RazerLength = Vector3.Distance(hit.point,shootPos.position);
                    }

                    Razer_beforeVFX.transform.position = shootPos.position;
                    
                    Razer_beforeVFX.transform.GetChild(0).GetComponent<VisualEffect>().SetFloat("Length",RazerLength);


                }
                else if(timer<Razer_startT+RazerDur)
                {
                    timer+=Time.deltaTime;

                    RaycastHit hit;
                    if(Physics.Raycast(shootPos.position,RazerDir,out hit,Mathf.Infinity,RazerLayer))
                    {
                        Debug.DrawLine(shootPos.position,hit.point,Color.red);
                        RazerLength = Vector3.Distance(hit.point,shootPos.position);
                    }

                    

                    if(facingRight)
                    {
                        RazerDir = Quaternion.AngleAxis((-SweepAngle/RazerDur)*Time.deltaTime,Vector3.back)*RazerDir;
                        currentAngle = SweepAngle*(timer-Razer_startT)/RazerDur;
                        
                    }
                    else
                    {
                        RazerDir = Quaternion.AngleAxis((SweepAngle/RazerDur)*Time.deltaTime,Vector3.back)*RazerDir;
                        currentAngle = 180-(SweepAngle*(timer-Razer_startT)/RazerDur);
                        
                    }
                    //雷射判定
                    Vector3 hitbox = new Vector3(RazerLength/2,RazerWidth/2,RazerWidth/2);
                    Collider[] hitObjs = Physics.OverlapBox((hit.point+transform.position)/2,hitbox,Quaternion.Euler(0,0,currentAngle),PlayerLayer);
                    
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
                            PlayerCtroller p = Player.GetComponent<PlayerCtroller>();

                            if(transform.position.x>Player.transform.position.x)
                            {
                                p.getHitByRight = true;
                            }
                            else
                            {
                                p.getHitByRight = false;
                            }
                            p.gettingHit();
                            
                        }
                    }
                    //
                    
                    if(!isDamaging)
                    {
                        Razer_beforeVFX.transform.GetChild(0).GetComponent<VisualEffect>().Stop();
                        Razer_beforeVFX.transform.GetChild(0).GetComponent<VisualEffect>().SetBool("Alive",false);
                        Razer_VFX.transform.GetChild(0).GetComponent<VisualEffect>().SendEvent("OnPlay");
                        isDamaging = true;
                    }
                     
                    Razer_VFX.transform.GetChild(0).GetComponent<VisualEffect>().SetFloat("Length",RazerLength);
                    Razer_VFX.transform.localRotation = Quaternion.Euler(0,0,currentAngle);
                }
                else if(timer<Razer_StateT)
                {
                    //stop razer wait for end state
                    timer+=Time.deltaTime;

                    if(isDamaging)
                    {
                        Razer_VFX.transform.GetChild(0).GetComponent<VisualEffect>().SetBool("Alive",false);
                        Razer_VFX.transform.GetChild(0).GetComponent<VisualEffect>().Stop();
                        isDamaging = false;
                    }
                }
                else
                {
                    timer = 0;
                    currentState = BossState.Move;
                    ResetAniTrigger();
                    BossAni.SetTrigger("BackToWalk");
                    StartCoroutine(RazerCD_Count());
                }
                

            break;

            case BossState.QuickBack:

                timer+=Time.deltaTime;

                if(timer<BackTime)
                {
                    currentBackSpeed = Mathf.Lerp(BackSpeed,0,timer/BackTime);
                    rb.velocity = BackDir*BackSpeed;
                }
                else if(timer<AnimationBackTime)
                {
                    rb.velocity = Vector3.zero;
                }
                else if(timer<QB_StateTime)
                {
                    
                    if(canUseRazer)
                    {
                        currentState = BossState.Razer;
                        rb.velocity = Vector3.zero;
                        timer = 0;
                        ResetAniTrigger();
                        BossAni.SetTrigger("Razer");
                        Razer_OnStart();
                        break;
                    }

                    if(!DeriveCheck&&!canUseRazer)
                    {
                        DeriveCheck = true;
                        if(isClose)
                        {
                            
                            int RandomNum = Random.Range(0,101);
                            if(RandomNum<=70)
                            {
                                //CounterAttack
                                int RandomNum2 = Random.Range(0,101);
                                if(RandomNum2<=40)
                                {
                                    DA_Dir = MoveDir;
                                    currentState = BossState.DashAttack;
                                    DeriveCheck = false;
                                    BossAni.SetTrigger("DashAttack");
                                    timer = 0;
                                    break;
                                }
                                else
                                {
                                    BossAni.SetTrigger("DoubleAttack");
                                    currentState = BossState.TwiceAttack;
                                    DeriveCheck = false;
                                    rb.velocity = Vector3.zero;
                                    timer = 0;
                                    break;
                                }

                            }
                        }
                        else
                        {
                            int RandomNum = Random.Range(0,101);
                            if(RandomNum<=70)//70%
                            {
                                int RandomNum2 = Random.Range(0,101);
                                if(RandomNum2<=50)//50%50%
                                {
                                    DA_Dir = MoveDir;
                                    currentState = BossState.DashAttack;
                                    DeriveCheck = false;
                                    BossAni.SetTrigger("DashAttack");
                                    timer = 0;
                                    break;
                                }
                                else
                                {
                                    currentState = BossState.AirDown;
                                    BossAni.SetTrigger("AirDown");
                                    timer = 0;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    timer = 0;
                    //next state

                    

                    DeriveCheck = false;
                    currentState = BossState.Move;
                    BossAni.SetTrigger("BackToWalk");

                    
                }

            break;

            case BossState.ShootAir:

                timer+=Time.deltaTime;

                if(timer<StartAim)
                {
                    rb.velocity = Vector3.zero;
                }
                else if(timer<StartShoot)//aiming
                {
                    if(!isAiming)
                    {
                        isAiming = true;
                        GenerateAimUI();
                    }
                }
                else if(timer<StartShoot+ShootDur)
                {
                    if(!isShoot)
                    {
                        isShoot = true;
                        ShootAttack();
                        Destroy(currentAim.gameObject);
                    }
                }
                else if(timer<SA_StateTime)
                {
                    isAiming = false;
                    isShoot = false;
                }
                else
                {
                    //nextState
                    currentState = BossState.Move;
                    timer = 0;
                    BossAni.SetTrigger("BackToWalk");
                }

            break;

            case BossState.Idle:

                if(isFighting)
                {
                    currentState = BossState.Move;
                    BossAni.SetTrigger("StartFight");
                    BossAni.SetTrigger("BackToWalk");
                    StartCoroutine(RazerCD_Count());
                }

            break;

        }

        GetHitCheck();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(notDie)
            {
                PlayerCtroller p = other.GetComponent<PlayerCtroller>();

                if(transform.position.x>Player.transform.position.x)
                {
                    p.getHitByRight = true;
                }
                else
                {
                    p.getHitByRight = false;
                }
                p.gettingHit();
            }
        }
    }
    public void AirDownDamageSwitch()
    {
        isAirDownDamage = !isAirDownDamage;
    }
    public void AD_EffectShow()
    {
        AD_VFX.transform.position = new Vector3(transform.position.x,AD_VFX.transform.position.y,AD_VFX.transform.position.z);
        AD_VFX.GetComponent<VisualEffect>().SendEvent("OnPlay");
    }
    public void FirstHit_Effect()
    {
        FirstHit.GetComponent<VisualEffect>().SendEvent("OnPlay");
    }
    public void SecondHit_Effect()
    {
        SecondHit.GetComponent<VisualEffect>().SendEvent("OnPlay");
    }
    void facingCheck()
    {
        if(facingRight==false&&MoveDir.x>0)
        {
            Flip();
        }
        else if(facingRight == true&&MoveDir.x<0)
        {
            Flip();
        }
    }
    void getRazerInitialDir()
    {
        if(Player.transform.position.x<transform.position.x)
        {
            RazerDir = Vector3.left;
        }
        else
        {
            RazerDir = Vector3.right;
        }
    }
    void Razer_OnStart()
    {
        Razer_beforeVFX.transform.position = shootPos.position;
        Razer_VFX.transform.position = shootPos.position;
        getRazerInitialDir();

        Razer_beforeVFX.transform.GetChild(0).GetComponent<VisualEffect>().SetBool("Alive",true);
        Razer_VFX.transform.GetChild(0).GetComponent<VisualEffect>().SetBool("Alive",true);
        
        Razer_beforeVFX.transform.localScale = transform.localScale;
        Razer_beforeVFX.transform.GetChild(0).GetComponent<VisualEffect>().SendEvent("OnPlay");
    }
    void DashAttackHit()
    {
        Collider[] hitObjs = Physics.OverlapBox(DA_hitPos.position,DA_hitBox,Quaternion.identity,PlayerLayer);
        
        if(hitObjs.Length==0)
        {
            print("Miss");
        }
        else
        {        
            foreach(Collider c in hitObjs)
            {
                print("DA_Hit"+c.name+"!!!!");
                PlayerCtroller p = Player.GetComponent<PlayerCtroller>();

                if(transform.position.x>Player.transform.position.x)
                {
                    p.getHitByRight = true;
                }
                else
                {
                    p.getHitByRight = false;
                }
                p.gettingHit();
                
            }
        }
    }
    void AirDownHit()
    {
        Collider[] hitObjs = Physics.OverlapBox(transform.position,DA_hitBox,Quaternion.identity,PlayerLayer);

        if(hitObjs.Length==0)
        {
            print("Miss");
        }
        else
        {        
            foreach(Collider c in hitObjs)
            {
                print("DA_Hit"+c.name+"!!!!");
                PlayerCtroller p = Player.GetComponent<PlayerCtroller>();

                if(transform.position.x>Player.transform.position.x)
                {
                    p.getHitByRight = true;
                }
                else
                {
                    p.getHitByRight = false;
                }
                p.gettingHit();
                
            }
        }
    }
    
    void GetHitCheck()
    {
        if(GetComponent<tempGetHit>().isHit)
        {
            hp--;
        }
    }
    void Flip()
	{
		facingRight = !facingRight;
		Vector3 Scaler = transform.localScale;
		Scaler.x*=-1;
		transform.localScale = Scaler;
	}
    void ShootAttack()
    {
        Vector3 ShotPos = currentAim.GetComponent<AimController>().pos3D;
        ShootFTX.transform.position = ShotPos;
        ShootFTX.GetComponent<VisualEffect>().SendEvent("OnPlay");
		
        Collider[] hitObjs = Physics.OverlapBox(ShotPos,ShootAir_HitBox,Quaternion.identity,PlayerLayer);
        
        if(hitObjs.Length==0)
        {
            print("Miss");
        }
        else
        {        
            foreach(Collider c in hitObjs)
            {
                print("Hit"+c.name+"!!!!");
                PlayerCtroller p = Player.GetComponent<PlayerCtroller>();

                if(transform.position.x>Player.transform.position.x)
                {
                    p.getHitByRight = true;
                }
                else
                {
                    p.getHitByRight = false;
                }
                p.gettingHit();
                
            }
        }
		
    }
    void GenerateAimUI()
    {
        Vector2 generatePos = Camera.main.WorldToScreenPoint(Player.transform.position);
        currentAim = Instantiate(AimUIPf,generatePos,Quaternion.identity,Canvas.transform);
        currentAim.GetComponent<AimController>().AimingTime = StartShoot-StartAim;
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
    void CheckRange()
    {
        isClose = Physics.CheckBox(transform.position,4*Vector3.one,Quaternion.identity,PlayerLayer);

        if(Physics.CheckSphere(transform.position,FarRange,PlayerLayer))
        {
            isFar = false;
        }
        else
        {
            isFar = true;
        }
    }
    void AttackHitCheck()
    {
        isAttack = true;
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
                    PlayerCtroller p = Player.GetComponent<PlayerCtroller>();

                    if(transform.position.x>Player.transform.position.x)
                    {
                        p.getHitByRight = true;
                    }
                    else
                    {
                        p.getHitByRight = false;
                    }
                    p.gettingHit();
					
				}
			}
		}
    }
    void MoveChoice()
    {
        if(isClose)
        {
            int RandomNum = Random.Range(0,101);
            print(RandomNum);

            if(RandomNum<=40)
            {
                //-->Twice Attack
                BossAni.SetTrigger("DoubleAttack");
                currentState = BossState.TwiceAttack;
                rb.velocity = Vector3.zero;
            }
            else if(RandomNum>40&&RandomNum<=70)
            {
                //-->Quick Back
                BossAni.SetTrigger("QuickBack");
                currentState = BossState.QuickBack;
                rb.velocity = Vector3.zero;
                BackDir = new Vector3(-MoveDir.x,0,0);
            }
            else
            {
                DA_Dir = MoveDir;
                currentState = BossState.DashAttack;
                BossAni.SetTrigger("DashAttack");
            }
        }
        else
        {
            int RandomNum = Random.Range(0,101);
            print(RandomNum);
            if(RandomNum<=50)
            {
                DA_Dir = MoveDir;
                currentState = BossState.DashAttack;
                BossAni.SetTrigger("DashAttack");
            }
            else
            {
                
                currentState = BossState.AirDown;
                BossAni.SetTrigger("AirDown");
            }
        }
    }
    void NextState()
    {
        bool isClose = Physics.CheckBox(transform.position,2*Vector3.one,Quaternion.identity,PlayerLayer);
        int RandomNum = Random.Range(1,101);
        if(isClose)
        {
            currentState = BossState.TwiceAttack;
        }
        else
        {
            if(RandomNum<=50)
            {
                currentState = BossState.DashAttack;
            }
            else
            {
                currentState = BossState.AirDown;
            }
        }
    }
    void ResetAniTrigger()
    {
        BossAni.ResetTrigger("DoubleAttack");
        BossAni.ResetTrigger("DashAttack");
        BossAni.ResetTrigger("BackToWalk");
        BossAni.ResetTrigger("AirDown");
        BossAni.ResetTrigger("QuickBack");
        BossAni.ResetTrigger("AirShoot");
        BossAni.ResetTrigger("Razer");
    }
    IEnumerator RazerCD_Count()
    {
        canUseRazer = false;
        for(float i =0 ; i<=RazerCD ; i+=Time.deltaTime)
		{
			yield return 0;
		}
		canUseRazer = true;

    }
    void OnDrawGizmos()
    {
        if(isAttack)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }
        Gizmos.DrawWireCube(HitPos.position,2*HitBox_size);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position,8*Vector3.one);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,2*FarRange);

        if(isAiming&&currentAim!=null)
        {
            Gizmos.color = Color.red;
            
            Vector3 AimPos = currentAim.GetComponent<AimController>().pos3D; 
            Gizmos.DrawWireCube(AimPos,2*ShootAir_HitBox);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(DA_hitPos.position,2*DA_hitBox);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, 2*AirD_hitBox);
        
    }
}