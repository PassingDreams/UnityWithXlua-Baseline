using UnityEngine;
using TMPro;
using Ueels;
using Button = UnityEngine.UI.Button;


public class GMWnd : WndBase
{
    // Start is called before the first frame update
    private Transform runBtn;
    private Transform contentField;
    
    void Awake()
    {
        runBtn=GetChildByName("RunBtn");
        runBtn.GetComponent<Button>().onClick.AddListener(RunContentCode);
        contentField = GetChildByName("content");
    }

    void RunContentCode()
    {
        var text=contentField.GetComponent<TMP_InputField>().text;
        LuaScriptRunner.Instance.DoString(text);
    }

}
