﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RushEnemyUIController : MonoBehaviour
{
    // Start is called before the first frame update
    private RushingBug EnemyScript;
    public Image EnemyHPBar;
    private GameObject HP_UI;
    public GameObject HpPf;
    public Transform Canvas;
    void Start()
    {
        //airEnemyHPBar = GetComponent<Image>();
        GenerateUI();
        EnemyScript = transform.parent.GetComponent<RushingBug>();
    }
    
    void Update() 
    {
        Vector3 EnemyBarPos = Camera.main.WorldToScreenPoint(this.transform.position);
        EnemyHPBar.transform.position = EnemyBarPos;
    }
    private void LateUpdate()
    {
        EnemyHPBar.fillAmount = (float)EnemyScript.hp/(float)EnemyScript.MaxHp;
    }

    void GenerateUI()
    {
        Vector3 GeneratePos = Camera.main.WorldToScreenPoint(this.transform.position);
        HP_UI = Instantiate(HpPf,GeneratePos,Quaternion.identity,Canvas);
        EnemyHPBar = HP_UI.GetComponent<Image>();
    }
    public void DestroyUI()
    {
        Destroy(EnemyHPBar.gameObject);
        print("Done");
    }
}