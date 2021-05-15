using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    // Start is called before the first frame update
    public float AimingTime;
    private GameObject Player;
    public RectTransform rectTransform;
    public Vector3 pos3D;
    void Awake()
    {
        Player = GameObject.Find("Player");
        rectTransform = transform.GetComponent<RectTransform>();
    }
    void Start()
    {
        StartCoroutine(Aiming());
        pos3D = Player.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Aiming()
    {
        for(float i =0 ; i<=AimingTime ; i+=Time.deltaTime)
		{
            pos3D = Vector3.Lerp(pos3D,Player.transform.position,0.1f);

            
            rectTransform.position = Camera.main.WorldToScreenPoint(pos3D);
			yield return new WaitForFixedUpdate();
		}
        
    }
}
