using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestGuideUI : MonoBehaviour
{
    [SerializeField] TMP_Text QuestTitleText;
    [SerializeField] TMP_Text QuestDescriptionText;
    [SerializeField] TMP_Text QuestProgress;

    private QuestInstance CurrentQuest;
    private Transform TargetNPC;

    public void Setup(QuestInstance _Quest)
    {
        CurrentQuest = _Quest;

        QuestTitleText.text = _Quest.Data.QuestName;
        QuestDescriptionText.text = _Quest.Data.QuestDescription;

        UpdateProgress(CurrentQuest);
        UpdateDistance(CurrentQuest);

    }

    private void Update()
    {
        if (CurrentQuest == null) return;

        UpdateProgress(CurrentQuest);
        UpdateDistance(CurrentQuest);
    }

    private void UpdateProgress(QuestInstance _Quest)
    {
        if(CurrentQuest.Data.QuestClassification == QuestClassification.Kill)
        {
            QuestProgress.text = $"{CurrentQuest.CurrentAmount} / {_Quest.Data.Amount}";
        }
        else
        {
            QuestProgress.text = string.Empty;
        }
    }

    private void UpdateDistance(QuestInstance _Quest)
    {
        if(CurrentQuest.Data.QuestClassification == QuestClassification.NpcTolk)
        {
            QuestProgress.text = string.Empty;
            if (TargetNPC == null) return;
        }

        float Dis = Vector3.Distance(PlayerStat.Instance.transform.position, TargetNPC.position);
        QuestProgress.text = $"{Dis:F1}";
    }

    private void OnClickTrackQuest(QuestInstance _Quest)
    {

    }
}
