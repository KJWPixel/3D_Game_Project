using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ExitPanelUI : BaseUI
{
    [SerializeField] private TitleUIController uiController;

    protected override void OnOpen()
    {
        
    }

    protected override void OnClose()
    {
        uiController.OnClickCloseTopUI();
    }

    public void OnConfirmExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
