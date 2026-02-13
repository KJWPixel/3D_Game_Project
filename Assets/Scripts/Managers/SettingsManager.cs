using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    public int LanguageIndex = 0;

    private GameSettings gameSettings = new GameSettings();
    private string savePath;
    private const string SAVEFOLDER = "Settings";
    private const string FILENAME = "Setting.json";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        savePath = Path.Combine(Application.dataPath, SAVEFOLDER, FILENAME);
        LoadSetting();
        ApplySettings();
    }

    public void SetGraphicsSettings(int screenIndex, int resolutionIndex, int frameRate)
    {
        gameSettings.Screen = screenIndex;
        gameSettings.Resolution = resolutionIndex;
        gameSettings.FrameRate = frameRate;
        SaveSetting();
        ApplySettings();
    }

    public void SetSoundSettings(float masterVolume, float effectVolume, float backGroundVolume)
    {
        gameSettings.MasterVolume = masterVolume;
        gameSettings.EffectVolume = effectVolume;
        gameSettings.BackGroundVolume = backGroundVolume;
        SaveSetting();
        ApplySettings();
    }

    public void SetLanguage(int index)
    {
        LanguageIndex = index;
    }

    private void LoadSetting()
    {
        if(File.Exists(savePath))
        {
            try
            {
                string json = File.ReadAllText(savePath);
                gameSettings = JsonUtility.FromJson<GameSettings>(json);
                Debug.Log($"게임세팅 로드 성공: {gameSettings}");
            }
            catch(System.Exception e)
            {
                Debug.LogError($"세팅 로드 실패: {e.Message}");
                gameSettings = new GameSettings();//기본값 사용
            }            
        }
        else
        {
            gameSettings = new GameSettings();
            SaveSetting();
        }
    }

    private void SaveSetting()
    {
        try
        {
            string directory = Path.GetDirectoryName(savePath);
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string json = JsonUtility.ToJson(gameSettings, true);
            File.WriteAllText(savePath, json);
            Debug.Log($"게임 세팅 저장 성공: {savePath}");
        }
        catch(System.Exception e)
        {
            Debug.LogError($"세팅 저장 실패: {e.Message}");
        }       
    }

    private void ApplySettings()
    {
        int width = 1920;
        int height = 1080;

        switch (gameSettings.Resolution)
        {
            case 0:
                width = 1280;
                height = 720;
                break;
            case 1:
                width = 1920;
                height = 1080;
                break;
            case 2:
                width = 1920;
                height = 1200;
                break;
            case 3:
                width = 2560;
                height = 1440;
                break;
            default:
                gameSettings.Resolution = 1;
                width = 1920;
                height = 1080;
                Debug.LogWarning("잘못된 해상도 인덱스 -> 기본 1920 x 1080으로 fallback");
                break;
        }

        FullScreenMode screenMode = gameSettings.Screen switch
        {
            0 => FullScreenMode.FullScreenWindow,
            1 => FullScreenMode.Windowed,
            2 => FullScreenMode.ExclusiveFullScreen,           
            _ => FullScreenMode.ExclusiveFullScreen,
        };

        Screen.SetResolution(width, height, screenMode);

        QualitySettings.vSyncCount = 0;

        // 프레임레이트 적용
        Debug.Log($"ApplySettings 시작 → 현재 gameSettings.FrameRate: {gameSettings.FrameRate}");
        if (gameSettings.FrameRate == -1)
        {
            Application.targetFrameRate = -1;
        }
        else
        {
            Application.targetFrameRate = gameSettings.FrameRate;
        }

        Debug.Log($"설정 적용: 해상도 = {width} x {height}, 화면 모드 = {screenMode}, 프레임레이트 = {gameSettings.FrameRate}");

        SoundManager soundManager = FindObjectOfType<SoundManager>();
        if (soundManager != null)
        {
            soundManager.ApplySoundSettings(gameSettings);
        }

        ApplyFPS();
    }

    public void ApplyLanguage()
    {
        string localeCode = LanguageIndex switch
        {
            0 => "ko",
            1 => "en",
            _ => "en"
        };

        LocalizationManager.Instance.ChangeLanguage(localeCode);
    }

    public void SetFPSToggle(bool isOn)
    {
        gameSettings.ShowFPS = isOn;
        SaveSetting();
        ApplyFPS(); // 즉시 적용
    }

    private void ApplyFPS()
    {
        // UIManager에 만들어둔 ToggleFPS 함수를 호출
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ToggleFPS(gameSettings.ShowFPS);
            Debug.Log($"[SettingsManager] FPS 표시 적용: {gameSettings.ShowFPS}");
        }
        else
        {
            Debug.LogWarning("[SettingsManager] UIManager 인스턴스를 찾을 수 없어 FPS 설정을 유예합니다.");
        }
    }

    public GameSettings GetSettings()
    {
        return gameSettings;
    }
    
}
