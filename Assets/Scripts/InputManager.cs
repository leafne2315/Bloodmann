using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    public bool PS4_X_Input;
    public bool PS4_O_Input;
    public bool PS4_Triangle_Input;
    public bool PS4_Square_Input;

    public float Delaytime; 
     
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("PS4-x")&&!PS4_X_Input)
        {
            PS4_X_Input = true;
            StartCoroutine(InputDelay_x());
        }

        if(Input.GetButtonDown("PS4-O")&& !PS4_O_Input)
        {
            PS4_O_Input = true;
            StartCoroutine(InputDelay_O());
        }

        if(Input.GetButtonDown("PS4-Triangle")&& !PS4_Triangle_Input)
        {
            PS4_Triangle_Input = true;
            StartCoroutine(InputDelay_Tria());
        }

        if(Input.GetButtonDown("PS4-Square")&& !PS4_Square_Input)
        {
            PS4_Square_Input = true;
            StartCoroutine(InputDelay_Square());
        }

    }

    IEnumerator InputDelay_x()
    {
        
        for(float i =0 ; i<=Delaytime ; i+=Time.deltaTime)
		{
			yield return 0;
		}

		PS4_X_Input = false;
    }
    IEnumerator InputDelay_O()
    {
        
        for(float i =0 ; i<=Delaytime ; i+=Time.deltaTime)
		{
			yield return 0;
		}
		PS4_O_Input = false;
    }
    IEnumerator InputDelay_Tria()
    {
        
        for(float i =0 ; i<=Delaytime ; i+=Time.deltaTime)
		{
			yield return 0;
		}
		PS4_Triangle_Input = false;
    }
    IEnumerator InputDelay_Square()
    {
        
        for(float i =0 ; i<=Delaytime ; i+=Time.deltaTime)
		{
			yield return 0;
		}
		PS4_Square_Input = false;
    }

}
