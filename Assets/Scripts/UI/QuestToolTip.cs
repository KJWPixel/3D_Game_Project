using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class QuestToolTip : MonoBehaviour
{
    [SerializeField] private string UITABLE = "UI Table";
    [SerializeField] private string QUESTTABLE = "QUEST Table";

    [SerializeField] private TMP_Text QuestNameText;
    [SerializeField] private TMP_Text QuestDescriptionText;
    [SerializeField] private TMP_Text QuestRewordText;
    [SerializeField] private Button TrackingButton;
    [SerializeField] private Button CloseButton;

    private QuestInstance CurrentQuest;
    private QuestUI QuestUI;

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }

    public void Setup(QuestInstance quest, QuestUI _UI)
    {
        CurrentQuest = quest;
        QuestUI = _UI;

        QuestNameText.text = $"<color=orange>{LocalizationSettings.StringDatabase.GetLocalizedString(QUESTTABLE, quest.Data.QuestName)}</color>";
        QuestDescriptionText.text = LocalizationSettings.StringDatabase.GetLocalizedString(QUESTTABLE, quest.Data.QuestDescription);
        QuestRewordText.text = $"<color=orange>{LocalizationSettings.StringDatabase.GetLocalizedString(UITABLE, "UI_GOLD")} </color>" + quest.Data.GoldRewward 
            + $"<color=orange> / {LocalizationSettings.StringDatabase.GetLocalizedString(UITABLE, "UI_EXP")}</color>" + quest.Data.ExpReward;
    }

    public void OnClickClose()
    {
        Debug.Log("Äù½ºÆ® ÅøÆÁ ´Ý±â");
        gameObject.SetActive(false);
    }

    public void OnClickQuestTracking()
    {
        QuestUI.OnClickTrackQuest(CurrentQuest);
    }
}
