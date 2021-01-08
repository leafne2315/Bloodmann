using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private PlayerCtroller playerCtrScript;
    public Image healthBar;
    //public GameObject GasUI;
    public GameObject Player;
    void Start()
    {
        healthBar = GetComponent<Image>();
        playerCtrScript = Player.GetComponent<PlayerCtroller>();
    }
    
    void Update() 
    {
        //Vector3 gasbarPos = Camera.main.WorldToScreenPoint(this.transform.position);
        //GasBar.transform.position = gasbarPos;
    }
    private void LateUpdate()
    {
        healthBar.fillAmount = playerCtrScript.hp/playerCtrScript.hp_Max;
    }
}
