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
            Debug.LogError("LoginManager�� �Ҵ���� ����");
            return;
        }

        LoginManager.TryLogin();
    }
}
