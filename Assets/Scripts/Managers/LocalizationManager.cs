using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    public static event Action OnLanguageReady;

    private Dictionary<string, string> currentTable;
    private Dictionary<LanguageType, Dictionary<string, string>> tables = new Dictionary<LanguageType, Dictionary<string, string>>();

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
            return;
        }

        LoadTables();
        SetLanguage(LanguageType.English);
    }

    private void LoadTables()
    {
        tables[LanguageType.Korean] = new Dictionary<string, string>()
        {
            { "START", "시작" },
            { "OPTION", "옵션" },
            { "EXIT", "게임 종료" },
        };
        tables[LanguageType.English] = new Dictionary<string, string>()
        {
            { "START", "Start" },
            { "OPTION", "Options" },
            { "EXIT", "GameExit" },
        };
    }

    public void SetLanguage(LanguageType language)
    {
        if (!tables.ContainsKey(language))
        {
            Debug.LogError($"언어 테이블 없음: {language}");
            return;
        }

        currentTable = tables[language];
        OnLanguageReady?.Invoke();
    }

    public string Get(string key)
    {
        if (currentTable == null)
        {
            Debug.LogError("currentTable is NULL (SetLanguage 안 됨)");
            return key;
        }

        return currentTable.TryGetValue(key, out var value) ? value : key;
    }
    public void RefreshAllTexts()
    {
        foreach (var text in FindObjectsOfType<LocalizedText>())
        {
            text.Refresh();
        }
    }
}
