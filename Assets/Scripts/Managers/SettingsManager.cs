using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
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

    private void LoadSetting()
    {
        if(File.Exists(savePath))
        {
            try
            {
                string json = File.ReadAllText(savePath);
                gameSettings = JsonUtility.FromJson<GameSettings>(json);
                Debug.Log($"���Ӽ��� �ε� ����: {gameSettings}");
            }
            catch(System.Exception e)
            {
                Debug.LogError($"���� �ε� ����: {e.Message}");
                gameSettings = new GameSettings();//�⺻�� ���
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
            Debug.Log($"���� ���� ���� ����: {savePath}");
        }
        catch(System.Exception e)
        {
            Debug.LogError($"���� ���� ����: {e.Message}");
        }       
    }

    private void ApplySettings()
    {
        Resolution[] resolutions = Screen.resolutions;
        if(gameSettings.Resolution >= 0 && gameSettings.Resolution < resolutions.Length)
        {
            Resolution targetResolution = resolutions[gameSettings.Resolution];
            FullScreenMode screenMode = gameSettings.Screen switch
            {
                0 => FullScreenMode.ExclusiveFullScreen,
                1 => FullScreenMode.Windowed,
                _ => FullScreenMode.ExclusiveFullScreen,
            };
            Screen.SetResolution(targetResolution.width, targetResolution.height, screenMode);
            Application.targetFrameRate = gameSettings.FrameRate;
            Debug.Log($"���� ����: �ػ� = {targetResolution.width} x {targetResolution.height}, ȭ�� ��� = {screenMode}, �����ӷ���Ʈ = {gameSettings.FrameRate}");
        }
        else
        {
            Debug.LogWarning("��ȿ���� ���� �ػ� �ε���, �⺻�� ����");
            gameSettings.Resolution = 0;
            ApplySettings();
        }

        SoundManager soundManager = FindObjectOfType<SoundManager>();
        if (soundManager != null)
        {
            soundManager.ApplySoundSettings(gameSettings);
        }
    }
    public GameSettings GetSettings()
    {
        return gameSettings;
    }
    
}
