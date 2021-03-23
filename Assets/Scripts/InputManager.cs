using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool PS4_X_Input;
    public bool PS4_O_Input;
    public bool PS4_Triangle_Input;
    public bool PS4_Square_Input;
    public bool PS4_Up;
    private bool isReset;
    public float Delaytime;
    public bool isInGameInput;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isInGameInput)
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

            if(Input.GetAxisRaw("PS4-UpDown")>0 && isReset)
            {
                PS4_Up = true;
                StartCoroutine(InputDelay_PS4Up());
                isReset = false;
            }
            else
            {
                isReset  = true;
            }
        }

    } 
    IEnumerator InputDelay_PS4Up()
    {
        for(int i = 0;i<2;i++)
		{
			yield return 0;
		}
        PS4_Up = false;
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


    public void SwitchInputAbility()
    {
        if(!isInGameInput)
        {
            StartCoroutine(Do_afterFrame());
        }
        else
        {
            isInGameInput = !isInGameInput;
        }
        
    }
    IEnumerator Do_afterFrame()
	{
		for(int i = 0;i<2;i++)
		{
			yield return 0;
		}
        isInGameInput = !isInGameInput;
    }
}
