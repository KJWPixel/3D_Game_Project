using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [Header("����Ʈ ������ ������")]
    [SerializeField] GameObject QuestItemPrefab;
    [SerializeField] Transform QuestItemParent;

    [Header("����Ʈ ���� ������")]
    [SerializeField] QuestToolTip QuestTooltip;

    [Header("����Ʈ ���̵� ������")]
    [SerializeField] QuestGuideUI QuestGuideUI;

    [Header("����Ʈ Ŭ���� UI")]
    [SerializeField] QuestClear QuestClear;

    [Header("����Ʈ Ǯ��")]
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
        // �̹� �ʱ�ȭ�� �Ϸ�ƴٸ� ��õ� ���ʿ�
        if (isInitialized) return;

        // QuestManager�� ���� �������� �ʾҴٸ� �ʱ�ȭ �Ұ���
        if (QuestManager.Instance == null)
            return;

        // QuestManager ������ Ǯ�� ����
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

        if (pooledQuestItemUI.Count == 0) // ���� Ǯ���� �� �Ǿ��� ��
        {
            pool = QuestManager.Instance.MaxQuestList;
            CreatePool(pool);
        }

        //pooling���� ������� ����Ʈ����Ʈ obj�� Active false
        foreach (var item in pooledQuestItemUI)
        {
            item.gameObject.SetActive(false);
        }
        //���Ϳ� ���� ���� Ÿ���̸� ���� Ÿ������ List����
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
