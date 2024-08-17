using System;
using System.Collections;
using System.Collections.Generic;
using Ueels.Core;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public void Update()
    {
        if ( Input.GetKey(KeyCode.LeftControl))
        {
            if ( Input.GetKeyDown(KeyCode.G))
            {
                GMWnd.Instance.Toggle();
            }
        }

        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in Enum.GetValues((typeof(KeyCode))))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    LuaScriptRunner.Instance.LuaCall("OnKeyDown",(int)keyCode);
                }
            }
        }
    }
}
