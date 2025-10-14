using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestGuideUI : MonoBehaviour
{
    [SerializeField] TMP_Text QuestDescriptionText;
    [SerializeField] TMP_Text QuestProgress;
    [SerializeField] TMP_Text QuestDistance;

    private QuestInstance CurrentQuest;
    private Transform TargetNPC;

    public void Setup(QuestInstance _Quest)
    {
        CurrentQuest = _Quest;

        QuestDescriptionText.text = _Quest.Data.QuestDescription;

        UpdateProgress(CurrentQuest);
        UpdateDistance(CurrentQuest);

    }

    private void Update()
    {
        if (CurrentQuest == null)
        {
            ClearText();
            return;
        }

        if (CurrentQuest.Data.QuestCondition == QuestCondition.Completed || QuestManager.Instance.ClearQuests.Contains(CurrentQuest.Data.QuestId))
        {
            ClearText();
            gameObject.SetActive(false);
            return;
        }

        UpdateProgress(CurrentQuest);
        UpdateDistance(CurrentQuest);
    } 

    private void UpdateProgress(QuestInstance _Quest)
    {
        switch (_Quest.Data.QuestClassification)
        {
            case QuestClassification.Kill:
                QuestProgress.text = $"{CurrentQuest.CurrentAmount} / {_Quest.Data.Amount}";
                break;
            case QuestClassification.Collect:
                QuestProgress.text = $"{CurrentQuest.CurrentAmount} / {_Quest.Data.Amount}";
                break;
            default:
                QuestProgress.text = string.Empty;
                break;
        }


        //if (CurrentQuest.Data.QuestClassification == QuestClassification.Kill)
        //{
        //    QuestProgress.text = $"{CurrentQuest.CurrentAmount} / {_Quest.Data.Amount}";
        //    //float Dis = Vector3.Distance(PlayerStat.Instance.transform.position, _Quest.Data.TargetArea.transform.position);
        //    //QuestDistance.text = $"{Dis:F1}";
        //}
        //else
        //{
        //    QuestProgress.text = string.Empty;
        //}
    }

    private void UpdateDistance(QuestInstance _Quest)
    {
        if (CurrentQuest.Data.QuestClassification == QuestClassification.NpcTolk)
        {
            QuestProgress.text = string.Empty;
            if (TargetNPC == null) return;
        }

        //float Dis = Vector3.Distance(PlayerStat.Instance.transform.position, _Quest.Data.TargetArea.transform.position);
        //QuestProgress.text = $"{Dis:F1}";
    }

    private void ClearText()
    {
        QuestDescriptionText.text = string.Empty;   
        QuestProgress.text = string.Empty;
        QuestDistance.text = string.Empty;
    }
}
