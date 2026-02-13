using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class LoginPanelUI : BaseUI
{
    [SerializeField] private LoginManager loginManager;
    [SerializeField] private TMP_InputField idInputField;
    [SerializeField] private TMP_InputField pwInputField;

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

    public override void Open()
    {
        base.Open();
        ClearFields();

        if (idInputField != null) idInputField.ActivateInputField();
    }

    public void ClearFields()
    {
        if (idInputField != null) idInputField.text = "";
        if (pwInputField != null) pwInputField.text = "";
    }
}
