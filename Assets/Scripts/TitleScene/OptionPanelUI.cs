using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanelUI : BaseUI
{
    [SerializeField] private TitleUIController uiController;

    [Header("옵션 패널")]
    [SerializeField] private GameObject GraphicsPanel;
    [SerializeField] private GameObject SoundPanel;
    [SerializeField] private GameObject GamePlayPanel;
    [SerializeField] private Button GraphicsButton;
    [SerializeField] private Button SoundButton;
    [SerializeField] private Button GamePlayButton;

    [Header("그래픽 옵션")]
    [SerializeField] private TMP_Dropdown ScreenOptions;
    [SerializeField] private TMP_Dropdown ResolutionOptions; 
    [SerializeField] private TMP_Dropdown FrameRateOptions;

    [Header("사운드 옵션")]
    [SerializeField] private Slider MasterVolumeSlider;
    [SerializeField] private Slider EffectVolumeSlider;
    [SerializeField] private Slider BackGroundVolumeSlider;

    private void Awake()
    {
        InitializeDropdowns();
        InitializedSliders();
    }
    protected override void OnClose()
    {
        uiController.OnClickCloseTopUI();
    }

    private void InitializeDropdowns()
    {
        //해상도 옵션 초기화
        ResolutionOptions.options.Clear();
        Resolution[] resolutions = Screen.resolutions;
        for(int i = 0; i < resolutions.Length; i++)
        {
            Resolution res = resolutions[i];
            ResolutionOptions.options.Add(new TMP_Dropdown.OptionData($"{res.width}x{res.height}"));
        }
        ResolutionOptions.value = Mathf.Clamp(SettingsManager.Instance.GetSettings().Resolution, 0, resolutions.Length - 1);
        ResolutionOptions.RefreshShownValue();

        //화면 모드 옵션 초기화
        ScreenOptions.options.Clear();
        ScreenOptions.options.Add(new TMP_Dropdown.OptionData("전체 화면"));
        ScreenOptions.options.Add(new TMP_Dropdown.OptionData("창 모드"));
        ScreenOptions.value = Mathf.Clamp(SettingsManager.Instance.GetSettings().Screen, 0, ScreenOptions.options.Count - 1);
        ScreenOptions.RefreshShownValue();

        //프레임레이트 옵션 초기화
        FrameRateOptions.options.Clear();
        FrameRateOptions.options.Add(new TMP_Dropdown.OptionData("30"));
        FrameRateOptions.options.Add(new TMP_Dropdown.OptionData("60"));
        FrameRateOptions.options.Add(new TMP_Dropdown.OptionData("120"));
        int frameRateIndex = FrameRateOptions.options.FindIndex(opt => opt.text == SettingsManager.Instance.GetSettings().FrameRate.ToString());
        FrameRateOptions.value = frameRateIndex >= 0 ? frameRateIndex : 1;//기본 60fps
        FrameRateOptions.RefreshShownValue();
    }

    private void InitializedSliders()
    {
        if(MasterVolumeSlider != null && EffectVolumeSlider != null && BackGroundVolumeSlider != null)
        {
            GameSettings settings = SettingsManager.Instance.GetSettings();
            MasterVolumeSlider.value = settings.MasterVolume;
            EffectVolumeSlider.value = settings.EffectVolume;
            BackGroundVolumeSlider.value = settings.BackGroundVolume;

            //슬라이더 범위 설정
            MasterVolumeSlider.minValue = 0f;
            MasterVolumeSlider.maxValue = 1f;
            EffectVolumeSlider.minValue = 0f;
            EffectVolumeSlider.maxValue = 1f;
            BackGroundVolumeSlider.minValue = 0f;
            BackGroundVolumeSlider.maxValue = 1f;
        }
    }

    public void OnClickGraphicsButton()
    {
        ShowPanel(GraphicsPanel);
    }
    public void OnClickSoundButton()
    {
        ShowPanel(SoundPanel);
    }
    public void OnClickGamePlayButton()
    {
        ShowPanel(GamePlayPanel);
    }
    private void ShowPanel(GameObject activePanel)
    {
        GraphicsPanel.SetActive(activePanel == GraphicsPanel);
        SoundPanel.SetActive(activePanel == SoundPanel);
        GamePlayPanel.SetActive(activePanel == GamePlayPanel);
    }

    public void OnClickApply()
    {
        if(ResolutionOptions != null && ScreenOptions != null && FrameRateOptions != null)
        {
            int resolutionIndex = ResolutionOptions.value;
            int screenIndex = ScreenOptions.value;
            int frameRate = int.Parse(FrameRateOptions.options[FrameRateOptions.value].text);

            SettingsManager.Instance.SetGraphicsSettings(screenIndex, resolutionIndex, frameRate);
            Debug.Log($"그래픽 설정 적용: 해상도 인덱스 = {resolutionIndex}, 화면모드 = {ScreenOptions.options[screenIndex].text}, 프레임레이트 = {frameRate}");
        }
        else
        {
            Debug.Log("드롭다운 컴포넌트가 할당되지 않았습니다.");
        }

        if (MasterVolumeSlider != null && EffectVolumeSlider != null && BackGroundVolumeSlider != null)
        {
            float masterVolume = MasterVolumeSlider.value;
            float effectVolume = EffectVolumeSlider.value;
            float backGroundVolume = BackGroundVolumeSlider.value;
            SettingsManager.Instance.SetSoundSettings(masterVolume, effectVolume, backGroundVolume);
            Debug.Log($"사운드 설정 적용: Master = {masterVolume}, Effect = {effectVolume}, BGM = {backGroundVolume}");
        }
        else
        {
            Debug.Log("사운드 슬라이더 컴포넌트가 할당되지 않았습니다.");
        }
    }
}
