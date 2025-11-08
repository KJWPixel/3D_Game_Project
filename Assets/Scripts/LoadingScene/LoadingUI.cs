using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private TMP_Text percentText;
    [SerializeField] private GameObject pressAnyKeyPanel;

    public void SetProgress(float t)
    {
        t = Mathf.Clamp01(t);
        if(progressBar != null) progressBar.fillAmount = t;
        if (progressBar != null) percentText.text = $"Loading... {Mathf.RoundToInt(t * 100)}%";
    }

    public void ShowPressAnyKey(bool show)
    {
        if(pressAnyKeyPanel != null)
        {
            pressAnyKeyPanel.SetActive(show);
        }
    }
}
