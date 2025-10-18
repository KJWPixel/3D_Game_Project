using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TitleUIController : BaseUI
{
    [Header("로그인 창")]
    [SerializeField] private GameObject LoginPanel;

    [Header("옵션 창")]
    [SerializeField] private GameObject OptionPanel;

    [Header("종료 창")]
    [SerializeField] private GameObject ExitPanel;

    private LoginPanelUI LoginUI;
    private OptionPanelUI OptionUI;
    private ExitPanelUI ExitUI;

    [SerializeField] private Stack<BaseUI> uiStack = new Stack<BaseUI>();    

    private void Awake()
    {
        UIInitialize();
    }

    private void UIInitialize()
    {
        LoginUI = LoginPanel.GetComponent<LoginPanelUI>();
        OptionUI = OptionPanel.GetComponent<OptionPanelUI>();
        ExitUI = ExitPanel.GetComponent<ExitPanelUI>();

        LoginUI.Close();
        OptionUI.Close();
        ExitUI.Close();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && uiStack.Count > 0)
        {          
            OnClickCloseTopUI();//close함수
            Debug.Log($"UIStack Count: {uiStack.Count}");
        }
    }

    public void OnClickExit()
    {
        OpenUI(ExitUI);
    }

    private void OpenUI(BaseUI _Ui)
    {
        if (_Ui == null) return;
        //활성화되여있는 UI를 닫을려면
        //if(uiStack.Count > 0)
        //{
        //    uiStack.Peek().gameObject.SetActive(false);
        //}
        uiStack.Push(_Ui);
        _Ui.Opne();
    }

    public void OnClickCloseTopUI()
    {
        if(uiStack.Count > 0)
        {
            BaseUI TopUI = uiStack.Pop();
            TopUI.Close();

            if(uiStack.Count > 0)
            {
                uiStack.Peek().gameObject.SetActive(true);
            }
            Debug.Log($"Close: {TopUI.name} uiStack Count: {uiStack.Count}");
        }
        else
        {
            Debug.Log("모든 ui 닫힘");
        }
    }
}
