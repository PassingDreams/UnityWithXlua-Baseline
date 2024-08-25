using System;
using System.Collections;
using System.Collections.Generic;
using Ueels.Core;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private List<int> downKeyBuffer = new List<int>();

    public bool GetKey(int keyCode)
    {
        return Input.GetKey((KeyCode)keyCode);
    }
    public bool GetKeyDown(int keyCode)
    {
        return Input.GetKeyDown((KeyCode)keyCode);
    }
    public void Update()
    {
        if (Input.anyKeyDown)
        {
            LuaScriptRunner.Instance.LuaCall("OnKeyDown");
        }
    }
}
