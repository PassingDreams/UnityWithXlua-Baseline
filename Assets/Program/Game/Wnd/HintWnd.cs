using UnityEngine;
using TMPro;
using Button = UnityEngine.UI.Button;

public class HintWnd : SingletonWnd<HintWnd>
{
    // Start is called before the first frame update
    private Transform closeBtn;
    private Transform contentField;
    
    void Awake()
    {
        closeBtn=GetChildByName("CloseBtn");
        closeBtn.GetComponent<Button>().onClick.AddListener(CloseWnd);
        contentField = GetChildByName("content");
    }

    void CloseWnd()
    {
        SetVisible(false);
    }
    public void SetText(string text)
    {
        contentField.GetComponent<TMP_InputField>().text = text;
    }

}