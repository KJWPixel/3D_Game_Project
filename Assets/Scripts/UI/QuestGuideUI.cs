using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class QuestGuideUI : MonoBehaviour
{
    [SerializeField] private string QUESTTABLE = "QUEST Table";

    [SerializeField] TMP_Text QuestDescriptionText;
    [SerializeField] TMP_Text QuestProgress;
    [SerializeField] TMP_Text QuestDistance;

    private QuestInstance currentQuest;
    private Transform TargetNPC;

    public void Setup(QuestInstance quest)
    {
        currentQuest = quest;

        QuestDescriptionText.text = LocalizationSettings.StringDatabase.GetLocalizedString(QUESTTABLE, currentQuest.Data.QuestDescription);

        UpdateProgress(currentQuest);
        UpdateDistance(currentQuest);

    }

    private void Update()
    {
        if (currentQuest == null)
        {
            ClearText();
            return;
        }

        if (currentQuest.Data.QuestCondition == QuestCondition.Completed || QuestManager.Instance.ClearQuests.Contains(currentQuest.Data.QuestId))
        {
            ClearText();
            gameObject.SetActive(false);
            return;
        }

        UpdateProgress(currentQuest);
        UpdateDistance(currentQuest);
    } 

    private void UpdateProgress(QuestInstance _Quest)
    {
        switch (_Quest.Data.QuestClassification)
        {
            case QuestClassification.Kill:
                QuestProgress.text = $"{currentQuest.CurrentAmount} / {_Quest.Data.Amount}";
                break;
            case QuestClassification.Collect:
                QuestProgress.text = $"{currentQuest.CurrentAmount} / {_Quest.Data.Amount}";
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

    private void UpdateDistance(QuestInstance quest)
    {
        if (currentQuest.Data.QuestClassification == QuestClassification.NpcTolk)
        {
            QuestProgress.text = string.Empty;
            if (TargetNPC == null) return;
        }

        if(currentQuest.Data.QuestClassification == QuestClassification.Kill)
        {
            QuestProgress.text = string.Empty;
            if(quest.Data.TargetArea == null) return;

            Vector3 target = quest.Data.TargetArea.transform.position;  
            float dir = Vector3.Distance(target, GameManager.Instance.Player.transform.position);

            QuestDistance.text = ((int)dir).ToString() + "m";

        }
    }

    private void ClearText()
    {
        QuestDescriptionText.text = string.Empty;   
        QuestProgress.text = string.Empty;
        QuestDistance.text = string.Empty;
    }
}
