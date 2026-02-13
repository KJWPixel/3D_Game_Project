using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class OptionPanelUI : BaseUI
{
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

    [Header("언어 옵션")]
    [SerializeField] private TMP_Dropdown LanguageOptions;
    [SerializeField] private Toggle FPSOption;

    [Header("로컬라이제이셔 테이블")]
    [SerializeField] private const string Table = "OPTION Table";

    private void Awake()
    {
        
    }

    private void Start()
    {
        InitializeDropdowns();
        InitializedSliders();
        IntializeToggle();

        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }
    protected override void OnClose()
    {
        
    }

    private void InitializeDropdowns()
    {
        //해상도 옵션 초기화
        ResolutionOptions.options.Clear();

        // 해상도 4개만 추가
        ResolutionOptions.options.Add(new TMP_Dropdown.OptionData("1280 x 720"));
        ResolutionOptions.options.Add(new TMP_Dropdown.OptionData("1920 x 1080"));
        ResolutionOptions.options.Add(new TMP_Dropdown.OptionData("1920 x 1200"));
        ResolutionOptions.options.Add(new TMP_Dropdown.OptionData("2560 x 1400"));

        //저장된 값 읽어서 선택 
        int saveResolutionIndex = SettingsManager.Instance.GetSettings().Resolution;
        ResolutionOptions.value = Mathf.Clamp(saveResolutionIndex, 0, ResolutionOptions.options.Count - 1);
        ResolutionOptions.RefreshShownValue();

        //화면 모드 옵션 초기화
        ScreenOptions.options.Clear();
        // 테이블 키 배열 
        string[] screenModeKeys = { "BORDERLASSWINDOWED","WINDOWED","FULLSCREEN" };

        foreach (string key in screenModeKeys)
        {
            var localizedStr = new LocalizedString(Table, key);
            ScreenOptions.options.Add(new TMP_Dropdown.OptionData(localizedStr.GetLocalizedString()));
        }
        ScreenOptions.value = Mathf.Clamp(SettingsManager.Instance.GetSettings().Screen, 0, ScreenOptions.options.Count - 1);
        ScreenOptions.RefreshShownValue();

        //프레임레이트 옵션 초기화
        FrameRateOptions.options.Clear();
        FrameRateOptions.options.Add(new TMP_Dropdown.OptionData("30"));
        FrameRateOptions.options.Add(new TMP_Dropdown.OptionData("60"));
        FrameRateOptions.options.Add(new TMP_Dropdown.OptionData("120"));

        // 무한 옵션 로컬라이즈
        var unlimitedLocalized = new LocalizedString(Table, "UNLIMITED");
        string unlimitedText = unlimitedLocalized.GetLocalizedString();
        FrameRateOptions.options.Add(new TMP_Dropdown.OptionData(unlimitedText));

        //설정에 저장된 값으로 드롭다운 선택
        int savedFrame = SettingsManager.Instance.GetSettings().FrameRate; // -1이면 무한
        int frameRateIndex;
        //int frameRateIndex = FrameRateOptions.options.FindIndex(opt => opt.text == SettingsManager.Instance.GetSettings().FrameRate.ToString());
        //FrameRateOptions.value = frameRateIndex >= 0 ? frameRateIndex : 1;//기본 60fps

        switch (savedFrame)
        {
            case 30:
                frameRateIndex = 0;
                break;
            case 60:
                frameRateIndex = 1;
                break;
            case 120:
                frameRateIndex = 2;
                break;
            case -1:
                frameRateIndex = 3;  // 무한은 항상 마지막 (인덱스 3)
                break;
            default:
                frameRateIndex = 1;  // 기본 60fps
                break;
        }
        FrameRateOptions.value = frameRateIndex;
        FrameRateOptions.RefreshShownValue();

        //게임플레이 옵션 초기화
        LanguageOptions.options.Clear();
        LanguageOptions.options.Add(new TMP_Dropdown.OptionData("한국어"));
        LanguageOptions.options.Add(new TMP_Dropdown.OptionData("English"));

        int savedLanguageIndex = SettingsManager.Instance.LanguageIndex;  // 또는 SettingsManager.Instance.GetSettings().LanguageIndex;
        LanguageOptions.value = Mathf.Clamp(savedLanguageIndex, 0, LanguageOptions.options.Count - 1);
        LanguageOptions.RefreshShownValue();
    }

    private void InitializedSliders()
    {
        if(MasterVolumeSlider != null && EffectVolumeSlider != null && BackGroundVolumeSlider != null)
        {
            GameSettings settings = SettingsManager.Instance.GetSettings();
            MasterVolumeSlider.value = settings.MasterVolume;
            EffectVolumeSlider.value = settings.EffectVolume;
            BackGroundVolumeSlider.value = settings.BackGroundVolume;

            //실시간 적용을 위한 이벤트 연결
            //MasterVolumeSlider.onValueChanged.AddListener((value) => 
            //{ 
            //    SettingsManager.Instance.SetSoundSettings(value, settings.EffectVolume, settings.BackGroundVolume); 
            //});
            //EffectVolumeSlider.onValueChanged.AddListener(value =>
            //{
            //    SettingsManager.Instance.SetSoundSettings(settings.MasterVolume, value, settings.BackGroundVolume);
            //});
            //BackGroundVolumeSlider.onValueChanged.AddListener((value) =>
            //{
            //    SettingsManager.Instance.SetSoundSettings(settings.MasterVolume, settings.EffectVolume, value);
            //});

            //슬라이더 범위 설정
            MasterVolumeSlider.minValue = 0f;
            MasterVolumeSlider.maxValue = 1f;
            EffectVolumeSlider.minValue = 0f;
            EffectVolumeSlider.maxValue = 1f;
            BackGroundVolumeSlider.minValue = 0f;
            BackGroundVolumeSlider.maxValue = 1f;
        }
    }

    private void IntializeToggle()
    {
        if(FPSOption != null)
        {
            FPSOption.isOn = SettingsManager.Instance.GetSettings().ShowFPS;

            //FPSOption.onValueChanged.AddListener((isOn) =>
            //{
            //    SettingsManager.Instance.SetFPSToggle(isOn);
            //    Debug.Log($"FPS 표시 설정 변경: {isOn}");
            //});
        }
    }

    private void OnLocaleChanged(Locale newLocale)
    {
        if(gameObject.activeInHierarchy)
        {
            InitializeDropdowns();
        }
    }

    private void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
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
        if (GamePlayPanel != null)
        {
            GamePlayPanel.SetActive(activePanel == GamePlayPanel);
        }
    }

    public void OnClickApply()
    {
        if(ResolutionOptions != null && ScreenOptions != null && FrameRateOptions != null)
        {
            int resolutionIndex = ResolutionOptions.value;
            int screenIndex = ScreenOptions.value;

            // FrameRate 처리: "무한"이면 -1 아니면 int.Parse
            //int frameRate = int.Parse(FrameRateOptions.options[FrameRateOptions.value].text);
            int selectedIndex = FrameRateOptions.value;  // 드롭다운 인덱스 가져오기
            int frameRate;

            if (selectedIndex == 3)  // 무한은 항상 4번째 옵션(인덱스 3)
            {
                frameRate = -1;
            }
            else
            {
                string selectedFrameText = FrameRateOptions.options[selectedIndex].text;
                if (!int.TryParse(selectedFrameText, out frameRate))
                {
                    frameRate = 60; // fallback
                }
            }

            // 로그 추가 (테스트용, 나중에 제거 가능)
            Debug.Log($"Apply 클릭 → 선택 인덱스: {selectedIndex}, 프레임레이트 변환값: {frameRate}");

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

        if (LanguageOptions != null)
        {
            int languageIndex = LanguageOptions.value;
            SettingsManager.Instance.SetLanguage(languageIndex);
            SettingsManager.Instance.ApplyLanguage(); // 새로 만들 함수
        }

        if(FPSOption != null)
        {
            bool currentToggleState = FPSOption.isOn;
            SettingsManager.Instance.SetFPSToggle(currentToggleState);
        }
    }

    public void TitleClickButton()
    {
        //저장 후 타이틀 씬으로 이동
        if(SceneMgr.Instance != null)
        {
            SceneMgr.Instance.ChangeScene(SCENE.TITLE, false);
        }
        else
        {
            Debug.Log("SceneMgr Instance null");
        }
        SceneManager.LoadScene(0);
    }

    public void ExitClickButton()
    {
        //저장 후 게임 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
