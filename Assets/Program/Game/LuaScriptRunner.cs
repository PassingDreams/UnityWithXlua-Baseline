using System;
using System.Collections;
using System.Collections.Generic;
using Ueels.Core;
using UnityEngine;
using XLua;

public class LuaScriptRunner : MonoSingleton<LuaScriptRunner>
{
    private static LuaEnv luaEnvInstance;
    
    public static LuaEnv LuaEnvInstance
    {
        get
        {
            if (luaEnvInstance == null)
            {
                luaEnvInstance = new LuaEnv();
            }
            return luaEnvInstance;
        }
    }

    private void LoadLuaScripts()
    {
        string boot = @"
        package.path='./Assets/Program/Game/LuaScripts/?.lua'
        require('/LuaMainInc')
        ";
        
        print("Lua Load Start>>>");
        LuaEnvInstance.DoString(boot);
        print("Lua Load OK<<<");
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
    
    void LateUpdate()
    {
        LuaCall("OnUpdateEnd");
    }
    private LuaFunction externalCallHandler;
    public void LuaCall(params object[] args)
    {
        externalCallHandler.Call(args);
    }


}
