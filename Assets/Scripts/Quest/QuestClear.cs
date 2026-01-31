using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class QuestClear : MonoBehaviour
{
    [SerializeField] private string UITABLE = "UI Table";
    [SerializeField] private string QUESTTABLE = "QUEST Table";

    [SerializeField] private TMP_Text QuestClassText;
    [SerializeField] private TMP_Text QuestClearText;//String Event
    [SerializeField] private TMP_Text QuestNameText;
    [SerializeField] private TMP_Text RewordExpText;//String Event
    [SerializeField] private TMP_Text RewordGoldText;//String Event

    private QuestInstance CurrentQuest;
    private Coroutine FadeCoroutine;
    private void Setup(QuestInstance quest)
    {

        QuestClassText.text = LocalizationSettings.StringDatabase.GetLocalizedString(QUESTTABLE, quest.Data.QuestClassKey);

        QuestNameText.text = LocalizationSettings.StringDatabase.GetLocalizedString(QUESTTABLE, quest.Data.QuestName);

        // 4. 보상 수치 (숫자이므로 그대로 유지하거나 포맷팅)
        RewordExpText.text = quest.Data.ExpReward.ToString();
        RewordGoldText.text = quest.Data.GoldRewward.ToString();

        // 알파값 초기화 (FadeOut을 위해)
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null) canvasGroup.alpha = 1f;
    }

    public void ShowClearUI(QuestInstance _Quest)
    {
        CurrentQuest = _Quest;
        Setup(CurrentQuest);
        gameObject.SetActive(true);

        if(FadeCoroutine != null)
        {
            StopCoroutine(FadeCoroutine);
        }

        FadeCoroutine = StartCoroutine(FadeOutAndDisable(2f, 1f));
    }

    private IEnumerator FadeOutAndDisable(float delay, float fadeDuration)
    {
        yield return new WaitForSeconds(delay);

        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
