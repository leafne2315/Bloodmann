using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingAndLoad : MonoBehaviour
{
    // Start is called before the first frame update
    private int Hp;
    [SerializeField]private int Hp_Max;
    [SerializeField]private int AidKitNum;
    [SerializeField]private int Blood;
    [SerializeField]private float posX;
    [SerializeField]private float posY;
    [SerializeField]private float posZ;
    [SerializeField]private Vector3 SavePos;

    [Header("Game Reset")]
    public int Origin_Hp_Max;
    public int Origin_AidKitNum;
    public int Origin_Blood;
    public Vector3 StartPos;
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
        SavePos = new Vector3(playerCtrl.transform.position.x,playerCtrl.transform.position.y,0.0f);

        posX = SavePos.x;
        posY = SavePos.y;
        posZ = SavePos.z;
    }
    void LoadPlayerData()
    {
        playerCtrl.hp = Hp_Max;
        playerCtrl.hp_Max = Hp_Max;
        playerCtrl.AidKitNum = AidKitNum;

        SavePos = new Vector3(posX,posY,posZ);
        playerCtrl.transform.position = SavePos;
    }
    public void SaveFile()
    {
        GetPlayerData();
        //PlayerPrefs.SetInt("myHP",Hp);
        PlayerPrefs.SetInt("myHP_Max",Hp_Max);
        PlayerPrefs.SetInt("AidKitNum",AidKitNum);

        PlayerPrefs.SetFloat("posX",posX);
        PlayerPrefs.SetFloat("posY",posY);
        PlayerPrefs.SetFloat("posZ",posZ);

        PlayerPrefs.Save();
        print("Save");
    }
    public void LoadFile()
    {
        //PlayerPrefs.GetInt("myHP",Hp);
        Hp_Max = PlayerPrefs.GetInt("myHP_Max");
        AidKitNum = PlayerPrefs.GetInt("AidKitNum");

        posX = PlayerPrefs.GetFloat("posX");
        posY = PlayerPrefs.GetFloat("posY");
        posZ = PlayerPrefs.GetFloat("posZ");
        LoadPlayerData();

        print("Load");
    }
    public void ResetFile()
    {
        Hp = Origin_Hp_Max;
        Hp_Max = Origin_Hp_Max;
        AidKitNum = Origin_AidKitNum;

        SavePos = StartPos;
        
        posX = SavePos.x;
        posY = SavePos.y;
        posZ = SavePos.z;

        PlayerPrefs.SetInt("myHP_Max",Hp_Max);
        PlayerPrefs.SetInt("AidKitNum",AidKitNum);

        PlayerPrefs.SetFloat("posX",posX);
        PlayerPrefs.SetFloat("posY",posY);
        PlayerPrefs.SetFloat("posZ",posZ);
        
        PlayerPrefs.Save();
        print("reset data");
    }
}
