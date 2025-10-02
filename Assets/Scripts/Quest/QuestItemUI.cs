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
        //QuestImage
    }
}
