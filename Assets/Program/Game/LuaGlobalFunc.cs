using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

namespace LuaSharp
{
    public class LuaGlobalFunc 
    {
        public static void LogOnWnd(string s)
        {
            var hintWnd = HintWnd.Instance;
            hintWnd.gameObject.SetActive(true);
            hintWnd.SetText(s);
        }
        public static void PrintParams(params object[] infos)
        {
            Ueels.Core.Debug.Logger.PrintParams(infos);
        }
        public static void ConsoleLog(string msg)
        {
            Debug.Log(msg);
        }

    }
        
}
