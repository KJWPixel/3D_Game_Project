using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestUIManager : MonoBehaviour
{
    [Header("����Ʈ ���̵� UI")]
    [SerializeField] GameObject QuestSmallPrefab;

    [Header("����Ʈ UI")]
    [SerializeField] GameObject QuestItemUIPrefab;
    QuestItemUI QuestItemUI;
    [SerializeField] Transform QuestScrollView;
    [SerializeField] QuestToolTip QuestTooltip;

    [Header("����Ʈ Ǯ��")]
    [SerializeField] private List<QuestItemUI> pooledQuestItemUI = new List<QuestItemUI>();
    [SerializeField] private int pool;
    List<QuestInstance> poolist;

    private void Awake()
    {
        if (QuestManager.Instance != null)
        {
            poolist = new List<QuestInstance>(QuestManager.Instance.MaxQuestList);
            // ���⼭ CreatePool ��� '������ �ʱ�ȭ'��
        }
    }


    private void Start()
    {
        QuestManager.Instance.QuestListChanged += RefreshQuestUI;
        int pool = QuestManager.Instance.MaxQuestList;
        CreatePool(pool);
    }

    private void Update()
    {
       
    }

    private void OnEnable()
    {
        RefreshQuestUI();
    }

    private void CreatePool(int _Count)
    {
        for(int i = 0; i < _Count; i++)
        {
            GameObject obj = Instantiate(QuestItemUIPrefab, QuestScrollView);
            obj.SetActive(false);

            QuestItemUI questItem = obj.GetComponent<QuestItemUI>();
            pooledQuestItemUI.Add(obj.GetComponent<QuestItemUI>());
        }
    }

    public void RefreshQuestUI(QuestClass? Type = null)
    {
        //pooling���� ������� ����Ʈ����Ʈ obj�� Active false
        foreach(var item in pooledQuestItemUI)
        {
            item.gameObject.SetActive(false);
        }
        //���Ϳ� ���� ���� Ÿ���̸� ���� Ÿ������ List����
        var quests = QuestManager.Instance.ActiveQuests;
        if(Type != null)
        {
            quests = quests.Where(q => q.Data.QuestClass == Type).ToList();
        }

        for(int i = 0; i < quests.Count && i < pooledQuestItemUI.Count; i++)
        {
            pooledQuestItemUI[i].gameObject.SetActive(true);
            pooledQuestItemUI[i].Setup(quests[i]);
        }
    }

    public void OnClickAllButton()
    {
        RefreshQuestUI();
    }
    public void OnClickFilterButton(int _TypeIndex)
    {
        QuestClass Type = (QuestClass) _TypeIndex;
        RefreshQuestUI(Type);
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


