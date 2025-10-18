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
    }

    public void SetSetting()
    {
        


        SaveSetting();
    }

    private void LoadSetting()
    {
        if(File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            gameSettings = JsonUtility.FromJson<GameSettings>(json);
            Debug.Log($"게임세팅 로드 성공: {gameSettings}");
        }
    }

    private void SaveSetting()
    {
        string json = JsonUtility.ToJson(gameSettings);
        File.WriteAllText(savePath, json);
        Debug.Log($"게임세팅 저장 성공{savePath}");
    }
    
}
