using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class QuestSmallUI : MonoBehaviour
{
    [SerializeField] TMP_Text QuestTitleText;
    [SerializeField] TMP_Text QuestDescriptionText;
    [SerializeField] TMP_Text QuestTipText;

    public void Setup(QuestInstance _Quest)
    {
        QuestTitleText.text = _Quest.Data.QuestName;
        QuestDescriptionText.text = _Quest.Data.QuestDescription;
        
        switch(_Quest.Data.QuestClassification)
        {
            case QuestClassification.NpcTolk:
                //플레이어와 NPC의 거리 계산함수()
                break;
            case QuestClassification.Kill:
                QuestTipText.text = $"{_Quest.CurrentAmount} / {_Quest.Data.Amount}";
                break;
            case QuestClassification.Collect:
                QuestTipText.text = $"{_Quest.CurrentAmount} / {_Quest.Data.Amount}";
                break ;
        }
    }

    private void Update()
    {
        
    }

}
