using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingAndLoad : MonoBehaviour
{
    // Start is called before the first frame update
    private int Hp;
    private int Hp_Max;
    private int AidKitNum;
    private int Blood;
    private PlayerCtroller playerCtrl;
    void Awake()
    {
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerCtroller>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    void GetPlayerData()
    {
        Hp = playerCtrl.hp;
        Hp_Max = playerCtrl.hp_Max;
        AidKitNum = playerCtrl.AidKitNum;
    }

    public void SaveFile()
    {
        GetPlayerData();
        PlayerPrefs.SetInt("myHP",Hp);
        PlayerPrefs.SetInt("myHP_Max",Hp_Max);
        PlayerPrefs.SetInt("AidKitNum",AidKitNum);
    }
    public void LoadFile()
    {
        PlayerPrefs.GetInt("myHP",Hp);
        PlayerPrefs.GetInt("myHP_Max",Hp_Max);
        PlayerPrefs.GetInt("AidKitNum",AidKitNum);
    }
}
