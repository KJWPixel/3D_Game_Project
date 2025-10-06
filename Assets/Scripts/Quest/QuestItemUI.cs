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
    [SerializeField] private Image QuestImage;
    [SerializeField] private Button QuestToolTipButton;

    private QuestToolTip QuestToolTip;
    private QuestInstance CurrentQuest;

    private void Start()
    {

    }

    public void Setup(QuestInstance _Quest)
    {
        CurrentQuest = _Quest;

        QuestTitleText.text = _Quest.Data.QuestName;
        QuestExplanationText.text = _Quest.Data.QuestDescription;

        //퀘스트 이미지는 퀘스트클래스에 따라 이미지 변경
        switch (_Quest.Data.QuestClass)
        {
            case QuestClass.Main:
                QuestImage.color = Color.cyan;
                break;
            case QuestClass.Sub:
                QuestImage.color = Color.blue;
                break;
            case QuestClass.Repeat:
                QuestImage.color = Color.green;
                break;
            case QuestClass.Daily:
                QuestImage.color = new Color(0.6f, 0f, 1f);
                break;
        }
    }    
}
