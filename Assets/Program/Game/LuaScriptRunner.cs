using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class LuaScriptRunner : MonoBehaviour
{
    private static LuaEnv instance;
    
    public static LuaEnv Instance
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

    private void RunLuaMain()
    {
        string boot = @"
        package.path='./Assets/Program/Game/LuaScripts/?.lua'
        require('/LuaMain')
        ";
        Instance.DoString(boot);
        print("Lua Load OK");
    }
    void Start()
    {
        RunLuaMain();
    }


}
