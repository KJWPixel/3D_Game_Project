using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestClear : MonoBehaviour
{
    [SerializeField] private TMP_Text QuestClassText;
    [SerializeField] private TMP_Text QuestClearText;
    [SerializeField] private TMP_Text QuestTitleText;
    [SerializeField] private TMP_Text RewordExpText;
    [SerializeField] private TMP_Text RewordGoldText;

    private QuestInstance CurrentQuest;
    private Coroutine FadeCoroutine;
    private void Setup(QuestInstance _Quest)
    {
        QuestClassText.text = _Quest.Data.QuestClass.ToString();
        QuestClearText.text = "¿Ï·á";
        QuestTitleText.text = _Quest.Data.QuestName;
        RewordExpText.text = _Quest.Data.ExpReward.ToString();
        RewordGoldText.text = _Quest.Data.GoldRewward.ToString();
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
