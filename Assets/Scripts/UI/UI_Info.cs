using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class UI_Info : MonoBehaviour
{
    [Header("로컬라이제이션 테이블")]
    
    [SerializeField] private Image InfoImage;
    [SerializeField] private TextMeshProUGUI InfoText;
    [SerializeField] private float DisplayDuration = 0f; 
    [SerializeField] private List<Color> Colors = new List<Color>();

    private Coroutine hideCoroutine;
    private const string UITable = "UI_Table";

    private void Awake()
    {
        InitInfo();
        
    }

    private void InitInfo()
    {
        InfoText.text = null;
        gameObject.SetActive(false);
    }

    public void showInfo(string key)
    {
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);

        gameObject.SetActive(true);

        //로컬라이제이션
        string message = LocalizationSettings.StringDatabase.GetLocalizedString(UITable, key);
        InfoText.text = message;

        hideCoroutine = StartCoroutine(HideInfo());
    }

    private IEnumerator HideInfo()
    {
        yield return new WaitForSeconds(DisplayDuration);

        gameObject.SetActive(false);
        hideCoroutine = null;
    }
}
