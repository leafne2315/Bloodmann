using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float damageShrinkTime = 3f;
    public float shrinkSpeed;
    //public float damageBarImageTime = 1.5f;
    //private float damageBarImageTimer;
    private float timer;
    public bool isDamaging;
    public bool isHealing;
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

        healthBar.fillAmount = (float)playerCtrScript.hp/(float)playerCtrScript.hp_Max;
        damageBarImage.fillAmount = healthBar.fillAmount;   
    }
    
    void Update() 
    {
        
        if(isDamaging)
        {
            if(timer<damageShrinkTime)
            {
                timer+=Time.deltaTime;
            }
            else
            {
                if(damageBarImage.fillAmount>healthBar.fillAmount)
                {
                    damageBarImage.fillAmount-=shrinkSpeed*Time.deltaTime;
                }
                else
                {
                    damageBarImage.fillAmount = healthBar.fillAmount;
                    isDamaging = false;
                    timer = 0;
                }
            }
        }
    }
    void LateUpdate()
    {
        healthBar.fillAmount = (float)playerCtrScript.hp/(float)playerCtrScript.hp_Max;
    }

    public void Healing(float fillAmount)
    {
        damageBarImage.fillAmount += fillAmount; 
    }
    public void Damaging()
    {
        isDamaging = true;
    }
    public void InterruptRecover()
    {
        damageBarImage.fillAmount = healthBar.fillAmount;
    }
   

    
}