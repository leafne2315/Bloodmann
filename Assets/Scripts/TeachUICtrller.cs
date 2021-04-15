using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TeachUICtrller : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isActivate;
    public GameObject ActivateUI;
    private PlayerCtroller playerCtrller;
    private InputManager IM;
    public float fadingSpeed;
    public float WaitingForDisapear;

    void Awake()
    {
        IM = GameObject.Find("InputManager").GetComponent<InputManager>();
    }
    void Start()
    {
        ActivateUI.transform.GetComponent<CanvasGroup>().alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActivateDataBool()
    {
        switch(gameObject.name)
        {
            case "RollTeachBlock":

                GameData.CanRoll = true;

            break;

            case "AttackTeachBlock":

                GameData.CanAttack = true;

            break;

        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")&&!isActivate)
        {
            playerCtrller = other.GetComponent<PlayerCtroller>();
            isActivate = true;
            ActivateDataBool();
            StartCoroutine(ShowingUI());
        }
    }
    IEnumerator closeActivateUI()
    {
        for(float a = 1 ; a>0 ; a-=fadingSpeed*Time.deltaTime)
		{
            ActivateUI.transform.GetComponent<CanvasGroup>().alpha = a;
			yield return new WaitForEndOfFrame();
		}
        ActivateUI.transform.GetComponent<CanvasGroup>().alpha = 0;
        Destroy(gameObject);
    }
    IEnumerator OpenActivateUI()
    {
        for(float a = 0 ; a<1 ; a+=fadingSpeed*Time.deltaTime)
		{
            ActivateUI.transform.GetComponent<CanvasGroup>().alpha = a;
			yield return new WaitForEndOfFrame();
		}
    }
    IEnumerator ShowingUI()
    {
        StartCoroutine(OpenActivateUI());
        yield return new WaitForSeconds(WaitingForDisapear);
        StartCoroutine(closeActivateUI());

    }
}
