using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class LoginPanelUI : BaseUI
{
    [SerializeField] private TitleUIController uiController;
    [SerializeField] private LoginManager LoginManager;
    protected override void OnOpen()
    {
        
    }
    protected override void OnClose()
    {
        uiController.OnClickCloseTopUI();
    }

    public void OnClickLogin()
    {
        if(LoginManager == null)
        {
            Debug.LogError("LoginManager가 할당되지 않음");
            return;
        }

        LoginManager.TryLogin();
    }
}
