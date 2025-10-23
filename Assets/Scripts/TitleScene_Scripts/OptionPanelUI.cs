using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanelUI : BaseUI
{
    [SerializeField] private TitleUIController uiController;

    [Header("�׷��� �ɼ�")]
    [SerializeField] private TMP_Dropdown ScreenOptions;
    [SerializeField] private TMP_Dropdown ResolutionOptions; 
    [SerializeField] private TMP_Dropdown FrameRateOptions;

    protected override void OnOpen()
    {
        
    }
    protected override void OnClose()
    {
        uiController.OnClickCloseTopUI();
    }

    public void OnClickApply()
    {
        if (ScreenOptions != null)
        {
            string Screen = ScreenOptions.options[ScreenOptions.value].text;
            Debug.Log($"ȭ��: {Screen}");
        }

        if (ResolutionOptions != null)
        {
            string Resolution = ResolutionOptions.options[ResolutionOptions.value].text;
            Debug.Log($"�ػ�: {Resolution}");
        }

        if(FrameRateOptions != null)
        {
            string FrameRate = FrameRateOptions.options[FrameRateOptions.value].text;
            Debug.Log($"������: {FrameRate}");
            Application.targetFrameRate = int.Parse(FrameRate);
        }       
    }
}
