using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private float damageBarImageShrinkTime = 3f;
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
        SetHealth(playerCtrScript.GetHealthNormalized());
        damageBarImage.fillAmount = healthBar.fillAmount;
        // damageBarColor = damageBarImage.color;
        // damageBarColor.a = 0f;
        // damageBarImage.color = damageBarColor;
    }
    
    void Update() 
    {
        
        // if(damageBarColor.a > 0)
        // {
        //     damageBarImageTimer -= Time.deltaTime;
        //     if(damageBarImageTimer < 0)
        //     {
        //         float fadeAmount = 5f;
        //         damageBarColor.a -= fadeAmount*Time.deltaTime;
        //         damageBarImage.color = damageBarColor;
        //     }
        // }
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
    private void LateUpdate()
    {
        SetHealth(playerCtrScript.GetHealthNormalized());
    }

    public void Healing()
    {
        float recoverySpeed = 0.7f;
        healthBar.fillAmount += recoverySpeed *Time.deltaTime;
        damageBarImage.fillAmount = healthBar.fillAmount;
    }
    public void Damaging()
    {
        // if(damageBarColor.a<=0)
        // {
        //     damageBarImage.fillAmount = healthBar.fillAmount;
        // }
        // damageBarColor.a = 1;
        // damageBarImage.color = damageBarColor;
        // damageBarImageTimer = damageBarImageTime;
        damageBarImageShrinkTimer = damageBarImageShrinkTime;  
    }

   

    private void SetHealth(float healthNormalized)
    {
        healthBar.fillAmount = healthNormalized;
    }
}
