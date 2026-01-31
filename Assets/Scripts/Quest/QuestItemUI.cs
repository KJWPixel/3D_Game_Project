using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] private string UITABLE = "UI Table";
    [SerializeField] private string QUESTTABLE = "QUEST Table";

    [SerializeField] private TMP_Text QuestNameText;
    [SerializeField] private TMP_Text QuestDescriptionText;
    [SerializeField] private Image QuestClassImage;
    [SerializeField] private Button QuestToolTipButton;
    
    private QuestInstance CurrentQuest;
    private QuestUI QuestUI;

    public void Setup(QuestInstance quest, QuestUI questUI)
    {
        CurrentQuest = quest;
        QuestUI = questUI;

        QuestNameText.text = LocalizationSettings.StringDatabase.GetLocalizedString(QUESTTABLE, quest.Data.QuestName);
        QuestDescriptionText.text = LocalizationSettings.StringDatabase.GetLocalizedString(QUESTTABLE, quest.Data.QuestDescription);

        //퀘스트 이미지는 퀘스트클래스에 따라 이미지 변경
        switch (quest.Data.QuestClass)
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
