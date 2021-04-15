using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TeachVideoCtrller : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isActivate;
    public GameObject ActivateUI;
    private PlayerCtroller playerCtrller;
    private InputManager IM;
    public float fadingSpeed;
    private GameManager GM;
    public bool isDone;
    void Awake()
    {
        IM = GameObject.Find("InputManager").GetComponent<InputManager>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Start()
    {
        ActivateUI.transform.GetComponent<CanvasGroup>().alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerCtrller!=null)
        {
            if(playerCtrller.isGrounded && !isActivate)
            {
                isActivate = true;
                playerCtrller.currentState = PlayerCtroller.PlayerState.Idle;
                ActivateDataBool();
                
                StartCoroutine(OpenActivateUI());
                
                IM.currentState = InputManager.InputState.Teaching;
                GM.isTeachPause = true;
                
            }
        }

        if(isActivate)
        {
            if(Input.GetButtonDown("PS4-O")&&!isDone)
            {
                isDone = true;
                StartCoroutine(closeActivateUI());
                playerCtrller.currentState = PlayerCtroller.PlayerState.Normal;
                StartCoroutine(TurnInputAfterFrame());
                GM.isTeachPause = false;
            }
        }
    }

    void ActivateDataBool()
    {
        switch(gameObject.name)
        {
            case "FlyTeachBlock":

                GameData.CanFly = true;

            break;

            case "DashTeachBlock":

                GameData.CanDash = true;

            break;

        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerCtrller = other.GetComponent<PlayerCtroller>();
        }
    }
    IEnumerator closeActivateUI()
    {
        for(float a = 1 ; a>0 ; a-=fadingSpeed*Time.unscaledDeltaTime)
		{
            ActivateUI.transform.GetComponent<CanvasGroup>().alpha = a;
			yield return new WaitForEndOfFrame();
		}
        ActivateUI.transform.GetComponent<CanvasGroup>().alpha = 0;
        Destroy(gameObject);
    }
    IEnumerator OpenActivateUI()
    {
        for(float a = 0 ; a<1 ; a+=fadingSpeed*Time.unscaledDeltaTime)
		{
            ActivateUI.transform.GetComponent<CanvasGroup>().alpha = a;
			yield return new WaitForEndOfFrame();
		}
    }
    IEnumerator TurnInputAfterFrame()
    {
        yield return new WaitForEndOfFrame();
        IM.currentState = InputManager.InputState.InGame;
    }
    
}
