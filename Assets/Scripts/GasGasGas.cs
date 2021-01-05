using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasGasGas : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerCtroller playerCtrScript;
    public Image GasBar;
    //public GameObject GasUI;
    public GameObject Player;
    void Start()
    {
        //GasBar = GetComponent<Image>();
        playerCtrScript = Player.GetComponent<PlayerCtroller>();
    }
    
    void Update() 
    {
        Vector3 gasbarPos = Camera.main.WorldToScreenPoint(this.transform.position);
        GasBar.transform.position = gasbarPos;
    }
    private void LateUpdate()
    {
        GasBar.fillAmount = playerCtrScript.currentGas/playerCtrScript.Gas_MaxValue;
    }
}
