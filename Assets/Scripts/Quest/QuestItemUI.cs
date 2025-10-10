using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text QuestTitleText;
    [SerializeField] private TMP_Text QuestExplanationText;
    [SerializeField] private Image QuestClassImage;
    [SerializeField] private Button QuestToolTipButton;
    
    private QuestInstance CurrentQuest;
    private QuestUI QuestUI;

    public void Setup(QuestInstance _Quest, QuestUI _QuestUI)
    {
        CurrentQuest = _Quest;
        QuestUI = _QuestUI;

        QuestTitleText.text = _Quest.Data.QuestName;
        QuestExplanationText.text = _Quest.Data.QuestDescription;

        //퀘스트 이미지는 퀘스트클래스에 따라 이미지 변경
        switch (_Quest.Data.QuestClass)
        {
            case QuestClass.Main:
                QuestClassImage.color = Color.cyan;
                break;
            case QuestClass.Sub:
                QuestClassImage.color = Color.blue;
                break;
            case QuestClass.Repeat:
                QuestClassImage.color = Color.green;
                break;
            case QuestClass.Daily:
                QuestClassImage.color = new Color(0.6f, 0f, 1f);
                break;
        }
    }    

    public void OnClickTooltip()
    {
        QuestUI.OnClickShowTooltip(CurrentQuest);
    }
}
