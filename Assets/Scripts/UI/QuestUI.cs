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
    [SerializeField] QuestGuideUI QuestGuideUI;

    [Header("퀘스트 클리어 UI")]
    [SerializeField] QuestClear QuestClear;

    [Header("퀘스트 풀링")]
    [SerializeField] private List<QuestItemUI> pooledQuestItemUI = new List<QuestItemUI>();
    [SerializeField] private int pool;

    private bool isInitialized = false;

    private void Awake()
    {
        pooledQuestItemUI.Clear();

        TryInitialize();
    }

    private void Start()
    {
        QuestManager.Instance.QuestListChanged += RefreshQuest;
        QuestManager.Instance.QuestCleared += ShowQuestClearUI;
    }

    private void OnEnable()
    {
        TryInitialize();

        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.QuestListChanged += RefreshQuest;
            RefreshQuest(); 
        }
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.QuestCleared += ShowQuestClearUI;
        }           
    }

    private void OnDisable()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.QuestListChanged -= RefreshQuest;
        }           
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.QuestCleared -= ShowQuestClearUI;
        }
            
    }
    private void TryInitialize()
    {
        // 이미 초기화가 완료됐다면 재시도 불필요
        if (isInitialized) return;

        // QuestManager가 아직 생성되지 않았다면 초기화 불가능
        if (QuestManager.Instance == null)
            return;

        // QuestManager 정보로 풀링 시작
        pool = QuestManager.Instance.MaxQuestList;
        CreatePool(pool);

        isInitialized = true;
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

        if (pooledQuestItemUI.Count == 0) // 아직 풀링이 안 되었을 때
        {
            pool = QuestManager.Instance.MaxQuestList;
            CreatePool(pool);
        }

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
        QuestTooltip.Setup(_Quest, this);
    }

    public void OnClickTrackQuest(QuestInstance _Quest)
    {
        QuestGuideUI.gameObject.SetActive(true);
        QuestGuideUI.Setup(_Quest);
    }

    private void ShowQuestClearUI(QuestInstance _Quest)
    {
        QuestClear.ShowClearUI(_Quest);
    }
}
