using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatObjGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] FloatObj;
    public Transform GeneratePos;
    private float GenerateFrequency;
    private bool canGenerate;
    void Start()
    {
        canGenerate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(canGenerate)
        {
            LaunchBox();
        }
    }
    void LaunchBox()
    {
        int RandomNum = Random.Range(0,FloatObj.Length);
        // print(RandomNum);

        Instantiate(FloatObj[RandomNum],GeneratePos.position,Quaternion.identity);
        canGenerate = false;

        int RandomValue = Random.Range(-3,6);
        GenerateFrequency = 6.0f + 0.5f * RandomValue;
        // print(GenerateFrequency+"sec");
        StartCoroutine(Generate_Count());
    }

    IEnumerator Generate_Count()
    {
        for(float i =0 ; i<=GenerateFrequency ; i+=Time.deltaTime)
		{
			yield return 0;
		}
        canGenerate = true;
    }
}
