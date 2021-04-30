using System.Collections;
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
        if(EnemyScript.MaxHp-EnemyScript.hp >= 1)
        {
            EnemyHPBar.enabled = true;
            EnemyHPBar.transform.GetChild(0).GetComponent<Image>().enabled = true;
            //EnemyHPBase.enabled = true;
        }
        else
        {
            EnemyHPBar.enabled = false;
            EnemyHPBar.transform.GetChild(0).GetComponent<Image>().enabled = false;
            //EnemyHPBase.enabled = false;
        }
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
        Destroy(EnemyHPBar.gameObject,1.5f);
        print("Done");
    }
}
