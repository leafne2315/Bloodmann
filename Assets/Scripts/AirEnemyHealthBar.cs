using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirEnemyHealthBar : MonoBehaviour
{
   private AirEnemy airEnemyScript;
    public Image airEnemyBar;
    //public GameObject GasUI;
    public GameObject AirEnemy;
    void Start()
    {
        //airEnemyBar = GetComponent<Image>();
        airEnemyScript = AirEnemy.GetComponent<AirEnemy>();
    }
    
    void Update() 
    {
        Vector3 airEnemyBarPos = Camera.main.WorldToScreenPoint(this.transform.position);
        airEnemyBar.transform.position = airEnemyBarPos;
    }
    private void LateUpdate()
    {
        airEnemyBar.fillAmount = (float)airEnemyScript.health/(float)airEnemyScript.maxhealth;
    }
}
