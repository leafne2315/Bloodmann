﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirEnemyHealthBar : MonoBehaviour
{
    private AirEnemy airEnemyScript;
    public Image airEnemyHPBar;
    private GameObject HP_UI;
    public GameObject HpPf;
    public Transform Canvas;
    
    //public GameObject AirEnemy;
    void Start()
    {
        //airEnemyHPBar = GetComponent<Image>();
        GenerateUI();
        airEnemyScript = transform.parent.GetComponent<AirEnemy>();
    }
    
    void Update() 
    {
        Vector3 airEnemyBarPos = Camera.main.WorldToScreenPoint(this.transform.position);
        airEnemyHPBar.transform.position = airEnemyBarPos;
    }
    private void LateUpdate()
    {
        airEnemyHPBar.fillAmount = (float)airEnemyScript.health/(float)airEnemyScript.maxhealth;
    }

    void GenerateUI()
    {
        Vector3 GeneratePos = Camera.main.WorldToScreenPoint(this.transform.position);
        HP_UI = Instantiate(HpPf,GeneratePos,Quaternion.identity,Canvas);
        airEnemyHPBar = HP_UI.GetComponent<Image>();
    }
    public void DestroyUI()
    {
        Destroy(airEnemyHPBar.gameObject);
        print("Done");
    }
}
