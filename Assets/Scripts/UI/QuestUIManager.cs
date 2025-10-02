using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestUIManager : MonoBehaviour
{
    [Header("Äù½ºÆ® °¡ÀÌµå UI")]
    [SerializeField] GameObject QuestSmallPrefab;

    [Header("Äù½ºÆ® UI")]
    [SerializeField] GameObject QuestItemUIPrefab;
    QuestItemUI QuestItemUI;
    [SerializeField] Transform QuestScrollView;

    private void Start()
    {
       //QuestManager.Instance.QuestListChanged +=
    }

    private void Update()
    {
       
    }

    public void AddQuestList()
    {      
        foreach (var quest in QuestManager.Instance.ActiveQuests)
        {
            if(quest != null)
            {
                GameObject QuestObj = Instantiate(QuestItemUIPrefab, QuestScrollView);
                QuestItemUI QuestItem = QuestObj.GetComponent<QuestItemUI>();
                QuestItem.Setup(quest);
            }
        }           
    }

    public void RemoveQuestList()
    {
        foreach(var quest in QuestManager.Instance.ClearQuests)
        {
            
        }
    }
}


