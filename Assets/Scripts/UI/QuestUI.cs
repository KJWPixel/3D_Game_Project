using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [Header("퀘스트 아이템 프리팹")]
    [SerializeField] GameObject QuestItemPrefab;
    [SerializeField] Transform QuestItemParent;

    [Header("퀘스트 툴팁 프리팹")]
    [SerializeField] QuestToolTip QuestTooltip;

    [Header("퀘스트 가이드 프리팹")]
    [SerializeField] GameObject QuestGuidePrefab;

    [Header("퀘스트 풀링")]
    [SerializeField] private List<QuestItemUI> pooledQuestItemUI = new List<QuestItemUI>();
    [SerializeField] private int pool;

    private void Awake()
    {
       
    }
    private void Start()
    {
        pool = QuestManager.Instance.MaxQuestList;

        CreatePool(pool);

        QuestManager.Instance.QuestListChanged += RefreshQuest;
    }

    public void CreatePool(int _Count)
    {
        for (int i = 0; i < _Count; i++)
        {
            GameObject obj = Instantiate(QuestItemPrefab, QuestItemParent);
            obj.SetActive(false);
            pooledQuestItemUI.Add(obj.GetComponent<QuestItemUI>());
        }
    }

    public void RefreshQuest(QuestClass? Type = null)
    {
        //pooling으로 만들어진 퀘스트리스트 obj를 Active false
        foreach (var item in pooledQuestItemUI)
        {
            item.gameObject.SetActive(false);
        }
        //필터에 따라 같은 타입이면 같은 타입으로 List정리
        var quests = QuestManager.Instance.ActiveQuests;
        if (Type != null)
        {
            quests = quests.Where(q => q.Data.QuestClass == Type).ToList();
        }

        for (int i = 0; i < quests.Count && i < pooledQuestItemUI.Count; i++)
        {
            pooledQuestItemUI[i].gameObject.SetActive(true);
            pooledQuestItemUI[i].Setup(quests[i], this);
        }
    }
    public void OnClickAllButton()
    {
        RefreshQuest();
    }
    public void OnClickFilterButton(int _TypeIndex)
    {
        QuestClass Type = (QuestClass)_TypeIndex;
        RefreshQuest(Type);
    }
    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }

    public void OnClickShowTooltip(QuestInstance _Quest)
    {
        QuestTooltip.gameObject.SetActive(true);
        QuestTooltip.Setup(_Quest);
    }
}
