using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public class QuestToolTip : MonoBehaviour
{
    [SerializeField] private TMP_Text QuestTitleText;
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

    public void Setup(QuestInstance _Quest, QuestUI _UI)
    {
        CurrentQuest = _Quest;
        QuestUI = _UI;

        QuestTitleText.text = $"<color=orange>{_Quest.Data.QuestName}</color>";
        QuestDescriptionText.text = _Quest.Data.QuestDescription;
        QuestRewordText.text = $"<color=orange>º¸»ó: °ñµå </color>" + _Quest.Data.GoldRewward + $"<color=orange> / °æÇèÄ¡ </color>" + _Quest.Data.ExpReward;
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
