using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBossCntrller : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isActivate;
    public bool isDone;
    public Animator BossAni;
    public Camera MainCam;
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
            MainCam.GetComponent<LerpCameraFollow>().Target = Mid;
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
        BossAni.SetTrigger("ReadyForFight");
        yield return new WaitForSeconds(7.0f);
        Destroy(Dialog03);
        StartCoroutine(closeUIBack());
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(LvLoader.LoadLevelWithDelay(1,1.5f));
        SLManager.ResetFile();
        GameData.ResetBool();
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
