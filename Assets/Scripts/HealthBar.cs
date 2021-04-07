using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float damageBarImageShrinkTime = 3f;
    //public float damageBarImageTime = 1.5f;
    //private float damageBarImageTimer;
    private float damageBarImageShrinkTimer;
    private PlayerCtroller playerCtrScript;
    public Image healthBar;
    public Image damageBarImage;
    private Color damageBarColor;
    //public GameObject GasUI;
    public GameObject Player;
    void Start()
    {
        healthBar = GetComponent<Image>();
        playerCtrScript = Player.GetComponent<PlayerCtroller>();
        
        damageBarImage.fillAmount = healthBar.fillAmount;
        
    }
    
    void Update() 
    {
        
        
        damageBarImageShrinkTimer -= Time.deltaTime;

        if(damageBarImageShrinkTimer <0)
        {
            if(healthBar.fillAmount<damageBarImage.fillAmount)
            {
                float shrinkSpeed = 0.3f;
                damageBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
            }
        }

        if(healthBar.fillAmount>damageBarImage.fillAmount)
        {
            damageBarImage.fillAmount = healthBar.fillAmount;
        }

    }
    void LateUpdate()
    {
        healthBar.fillAmount = (float)playerCtrScript.hp/(float)playerCtrScript.hp_Max;
    }

    public void Healing()
    {
        float recoverySpeed = 0.7f;
        healthBar.fillAmount += recoverySpeed *Time.deltaTime;
        damageBarImage.fillAmount = healthBar.fillAmount;
    }
    public void Damaging()
    {
        damageBarImageShrinkTimer = damageBarImageShrinkTime;
    }

   

    
}
