using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHintCtrller : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isActivate;
    public GameObject ActivateUI;
    public float fadingSpeed;
    public float WaitingForDisapear;
    void Start()
    {
        ActivateUI.transform.GetComponent<CanvasGroup>().alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")&& !isActivate && !GameData.isDoorOpen)
        {
            isActivate = true;
            StartCoroutine(ShowingUI());
            ActivateUI.transform.GetChild(0).GetComponent<TextMeshProStarter>().ActivateType();
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
