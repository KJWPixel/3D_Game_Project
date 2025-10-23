using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    public void SetSetting()
    {       
        SaveSetting();
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
    }
    
}
