using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PreBossCntrller : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isActivate;
    public bool isDone;
    public Animator BossAni;
    public CinemachineVirtualCamera MainCam;
    public CinemachineVirtualCamera TalkCam;
    public CinemachineConfiner CameraRestrict;
    public Transform Mid;
    public GameObject Player;
    public GameObject Boss;
    public GameObject UIBack;
    public float fadingSpeed;
    public GameObject Dialog01;
    public GameObject Dialog02;
    public GameObject Dialog03;
    private LevelLoader LvLoader;
    private SavingAndLoad SLManager;
    public GameObject CloseDoor;
    
    void Awake()
    {
        LvLoader = GameObject.Find("LevelLoder").GetComponent<LevelLoader>();
        SLManager = GameObject.Find("Save&Load").GetComponent<SavingAndLoad>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isActivate&&!isDone)
        {
            isDone = true;
            Mid.position = (Player.transform.position + Boss.transform.position)/2;
            Mid.position = new Vector3(Mid.position.x,Player.transform.position.y,Player.transform.position.z);
            MainCam.m_Priority = 9;
            StartCoroutine(BossFightBegin());
            //
        }
    }
    IEnumerator BossFightBegin()
    {
        
        print("startBoss");
        yield return new WaitForSeconds(1.5f);
        print("openUI");
        StartCoroutine(OpenUIBack());
        Dialog01.GetComponent<TextMeshProStarter>().ActivateType();
        yield return new WaitForSeconds(4.5f);
        Destroy(Dialog01);
        Dialog02.GetComponent<TextMeshProStarter>().ActivateType();
        yield return new WaitForSeconds(4.5f);
        Destroy(Dialog02);
        Dialog03.GetComponent<TextMeshProStarter>().ActivateType();
        
        
        BossAni.SetTrigger("ReadyToFight");
        
        yield return new WaitForSeconds(7.0f);
        Destroy(Dialog03);
        StartCoroutine(closeUIBack());
        
        yield return new WaitForSeconds(3.0f);
        /*轉場回開頭
        StartCoroutine(LvLoader.LoadLevelWithDelay(1,1.5f));
        SLManager.ResetFile();
        GameData.ResetBool();
        */
        Player.GetComponent<PlayerCtroller>().currentState = PlayerCtroller.PlayerState.Normal;
        Boss.GetComponent<BossController>().isFighting = true;

        MainCam.m_Priority = 11;
        CameraRestrict.enabled = true;
        CloseDoor.SetActive(true);
        Destroy(gameObject);
    }
    IEnumerator closeUIBack()
    {
        for(float a = 1 ; a>0 ; a-=fadingSpeed*Time.deltaTime)
		{
            UIBack.transform.GetComponent<CanvasGroup>().alpha = a;
			yield return new WaitForEndOfFrame();
		}
        UIBack.transform.GetComponent<CanvasGroup>().alpha = 0;
    }
    IEnumerator OpenUIBack()
    {
        for(float a = 0 ; a<1 ; a+=fadingSpeed*Time.deltaTime)
		{
            UIBack.transform.GetComponent<CanvasGroup>().alpha = a;
			yield return new WaitForEndOfFrame();
		}
    }
}
