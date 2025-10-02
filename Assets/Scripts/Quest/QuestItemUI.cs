using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text QuestTitleText;
    [SerializeField] private TMP_Text QuestExplanationText;
    [SerializeField] private Image QuestImage;


    public void Setup(QuestInstance _Quest)
    {
        QuestTitleText.text = _Quest.Data.QuestName;
        QuestExplanationText.text = _Quest.Data.QuestDescription;

        //����Ʈ �̹����� ����ƮŬ������ ���� �̹��� ����
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
