using System;
using UnityEngine;
using TMPro;
using Ueels;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using Button = UnityEngine.UI.Button;
using Logger = Ueels.Core.Debug.Logger;


public class GMWnd : SingletonWnd<GMWnd>
{
    // Start is called before the first frame update
    private Button runBtn;
    private Button[] tabButtons;
    private TMP_InputField inputField;
    private int curSelectingTabIndex = -1;

    [Serializable]
    private class MemBuffer
    {
        public string[] cmdsBuffer ;
        public MemBuffer()
        {
            cmdsBuffer= new string[]{"","","","",""};
        }
    }

    private MemBuffer memBuffer;
    
    void Awake()
    {
        runBtn=GetChildByName("RunBtn")?.GetComponent<Button>();
        runBtn.onClick.AddListener(RunContentCode);
        inputField = GetChildByName("content")?.GetComponent<TMP_InputField>();
        tabButtons = new Button[5];
        for (int i = 0; i<5; i++)
        {
            tabButtons[i] = GetChildByName("TabBar.Button" + (i+1))?.GetComponent<Button>();
            if (tabButtons[i])
            {
                int idx = i;
                tabButtons[i].onClick.AddListener(() =>
                { OnClickTabBtn(idx); });
            }
        }
        
        memBuffer= JsonSaveManager.LoadData<MemBuffer>("GMWndInfoDoc");
        if (memBuffer==null) { memBuffer= new MemBuffer(); }
        SwitchToTab(0);
    }

    void SwitchToTab(int tabIndex)
    {
        if (curSelectingTabIndex >= 0)
        { memBuffer.cmdsBuffer[curSelectingTabIndex] = inputField.text; }
        curSelectingTabIndex = tabIndex;
        inputField.text = memBuffer.cmdsBuffer[curSelectingTabIndex]==null ? "" :  memBuffer.cmdsBuffer[curSelectingTabIndex] ;
    }

    void RunContentCode()
    {
        var text=inputField.text;
        LuaScriptRunner.LuaEnvInstance.DoString(text);
    }
    
    void OnClickTabBtn(int btnIdx)
    {
        SwitchToTab(btnIdx);
    }
    void OnApplicationQuit()
    {
        memBuffer.cmdsBuffer[curSelectingTabIndex] = inputField.text;
        JsonSaveManager.SaveDataTo(memBuffer,"GMWndInfoDoc");
    }

}
