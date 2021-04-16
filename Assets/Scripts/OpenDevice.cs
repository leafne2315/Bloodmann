using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OpenDevice : MonoBehaviour
{
    // Start is called before the first frame update
    public bool showUI;
    private InputManager Im;
    public GameObject ActivateUI;
    public GameObject ActiveUI_Pf;
    public Transform RealWorldCanvas;
    public GameObject targetDoor;
    public bool canActivate = true;
    private LevelLoader LvLoader;
    private PlayerCtroller playerScript;
    public Camera OriginCam;
    public Camera camera2;
    public GameObject Canvas;
    void Awake()
    {
        LvLoader = GameObject.Find("LevelLoder").GetComponent<LevelLoader>();
        playerScript = GameObject.Find("Player").GetComponent<PlayerCtroller>();
    } 
    void Start()
    {
        ActivateUI = Instantiate(ActiveUI_Pf,transform.position + Vector3.forward*-2 + Vector3.up*1,Quaternion.identity,RealWorldCanvas);
        ActivateUI.transform.GetComponent<Image>().CrossFadeAlpha(0,0,false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(canActivate)
            {
                showUI = true;
                OpenActivateUI();
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            showUI = false;
            closeActivateUI();
        }
    }
    public void closeActivateUI()
    {
        ActivateUI.transform.GetComponent<Image>().CrossFadeAlpha(0.0f,0.5f,false);
    }
    public void OpenActivateUI()
    {
        ActivateUI.transform.GetComponent<Image>().CrossFadeAlpha(1.0f,0.5f,false);
    }
    
    public IEnumerator OpenDoor()
    {
        GameData.isDoorOpen = true;
        LvLoader.FadeOut();
        yield return new WaitForSeconds(1.5f);
        //相機換過去
        changeCamTo2();
        LvLoader.FadeIn();
        yield return new WaitForSeconds(1.5f);
        targetDoor.GetComponent<DoorCtrller>().isOpen = true;
        //音效
        GameObject sfx = Instantiate(Resources.Load("SoundPrefab/OpenDoor") as GameObject, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(targetDoor.GetComponent<DoorCtrller>().openTime+1.5f);
        LvLoader.FadeOut();
        yield return new WaitForSeconds(1.5f);
        //相機喚回來
        ChangeBack();
        LvLoader.FadeIn();
        yield return new WaitForSeconds(1.0f);
        playerScript.currentState = PlayerCtroller.PlayerState.Normal;
        playerScript.canOpenDoor = false;
        Destroy(gameObject);
    }

    void changeCamTo2()
    {
        OriginCam.enabled = false;
        camera2.enabled = true;
        Canvas.SetActive(false);
    }
    void ChangeBack()
    {
        OriginCam.enabled = true;
        camera2.enabled = false;
        Canvas.SetActive(true);
    }
}
