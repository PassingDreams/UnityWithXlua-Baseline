using System;
using System.Collections;
using System.Collections.Generic;
using Ueels.Core;
using UnityEngine;
using XLua;

public class LuaScriptRunner : MonoBehaviour
{
    private static LuaEnv instance;
    
    public static LuaEnv LuaEnvInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new LuaEnv();
            }
            return instance;
        }
    }

    private void LoadLuaScripts()
    {
        string boot = @"
        package.path='./Assets/Program/Game/LuaScripts/?.lua'
        require('/LuaMainInc')
        ";
        LuaEnvInstance.DoString(boot);
        print("Lua Load OK");
    }

    void Awake()
    {
        LoadLuaScripts();
    }
    void Start()
    {
        externalCallHandler=LuaEnvInstance.Global.Get<LuaFunction>("ExternalCall");
    }
    void Update()
    {
        InputManager.Instance.Update();
        LuaCall("OnUpdate");
    }
    private LuaFunction externalCallHandler;
    public void LuaCall(params object[] args)
    {
        externalCallHandler.Call(args);
    }


}
