using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class LoginPanelUI : BaseUI
{
    [SerializeField] private LoginManager loginManager;
 
    public void OnClickLogin()
    {
        if (loginManager == null)
        {
            Debug.LogError("LoginManager가 할당되지 않음");
            return;
        }
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        loginManager.TryLogin();
        
    }
}
