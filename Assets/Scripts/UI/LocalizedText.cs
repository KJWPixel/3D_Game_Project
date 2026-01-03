using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string key;
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        LocalizationManager.OnLanguageReady += Refresh;
    }

    private void OnDisable()
    {
        LocalizationManager.OnLanguageReady -= Refresh;
    }

    public void Refresh()
    {
        if (LocalizationManager.Instance == null)
            return;

        text.text = LocalizationManager.Instance.Get(key);
    }
}
