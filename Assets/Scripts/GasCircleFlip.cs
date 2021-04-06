using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCircleFlip : MonoBehaviour
{
    private PlayerCtroller playerCtrScript;
    //public GameObject GasUI;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        playerCtrScript = Player.GetComponent<PlayerCtroller>();
    }

    // Update is called once per frame
    void Update()
    {
         if(playerCtrScript.facingRight)
        {
            Vector3 Scaler = transform.localScale;
		    Scaler.x = Mathf.Abs(transform.localScale.x);
		    transform.localScale = Scaler;
        }
        else
        {
            Vector3 Scaler = transform.localScale;
		    Scaler.x = -Mathf.Abs(transform.localScale.x);
		    transform.localScale = Scaler;
        }
    }
}
