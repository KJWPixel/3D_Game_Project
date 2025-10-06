using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestToolTip : MonoBehaviour
{
    [SerializeField] private TMP_Text QuestTitleText;
    [SerializeField] private TMP_Text QuestDescriptionText;
    [SerializeField] private TMP_Text QuestRewordText;
    [SerializeField] private Button TrackingButton;
    [SerializeField] private Button CloseButton;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Setup(QuestInstance _Quest)
    {
        QuestTitleText.text = _Quest.Data.QuestName;
        QuestDescriptionText.text = _Quest.Data.QuestDescription;
        QuestRewordText.text = "����: ��� " + _Quest.Data.GoldRewward + " / ����ġ " + _Quest.Data.ExpReward;
    }

    public void OnClickQuestTracking()
    {
        Debug.Log("����Ʈ ����");
    }

    public void OnClickClose()
    {
        Debug.Log("����Ʈ ���� �ݱ�");
        gameObject.SetActive(false);
    }
}
