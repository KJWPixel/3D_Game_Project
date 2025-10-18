using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class OptionPanelUI : BaseUI
{
    [SerializeField] private TitleUIController uiController;

    protected override void OnOpen()
    {
        
    }
    protected override void OnClose()
    {
        uiController.OnClickCloseTopUI();
    }

    public void OnClickApply()
    {

    }
}
