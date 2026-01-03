using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

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
    }

    public void ChangeLanguage(string localCode)
    {
        StartCoroutine(SetLocale(localCode));
    }

    IEnumerator SetLocale(string localCode)
    {
        yield return LocalizationSettings.InitializationOperation;

        var selectedLocale = LocalizationSettings.AvailableLocales.GetLocale(localCode);

        if (selectedLocale != null)
        {
            LocalizationSettings.SelectedLocale = selectedLocale;
        }
        else
        {
            Debug.Log($"Locale not found :: {localCode}");
        }
    }
}
