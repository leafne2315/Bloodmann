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
    private bool Up_Exit;
    public bool PS4_LH_Up;
    public float PS4_LH_axis;
    public bool LH_up_Exit = true;
    public float Delaytime;
    public bool isInGameInput;
    public enum InputState{MainMenu,InGame,SavePointMenu,CollectingBlood}
    public InputState currentState;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case InputState.MainMenu:

            break;

            case InputState.InGame:

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

                if(Input.GetAxis("PS4-UpDown")>0.3f && Up_Exit)
                {
                    PS4_Up = true;
                    StartCoroutine(InputDelay_PS4Up());
                    Up_Exit = false;
                }
                else
                {
                    Up_Exit  = true;
                }

                LH();

            break;

            case InputState.SavePointMenu:



            break;

            case InputState.CollectingBlood:

            break;
        }

        if(isInGameInput)
        {
            currentState = InputState.InGame;
        }
        else
        {
            currentState = InputState.MainMenu;
        }

    }
    void LH()
    {
        PS4_LH_axis = Input.GetAxis("PS4-L-Vertical");

        if(Input.GetAxis("PS4-L-Vertical")>0.3f && LH_up_Exit)
        {
            PS4_LH_Up = true;
            StartCoroutine(InputDelay_PS4_LH_Up());
            LH_up_Exit = false;
        }
        else
        {
            LH_up_Exit = true;
        }
        
    }
    IEnumerator InputDelay_PS4_LH_Up()
    {
        for(int i = 0;i<2;i++)
		{
			yield return 0;
		}
        PS4_LH_Up = false;
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
