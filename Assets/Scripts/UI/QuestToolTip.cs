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
        Debug.Log("Äù½ºÆ® ÃßÀû");
    }

    
}
