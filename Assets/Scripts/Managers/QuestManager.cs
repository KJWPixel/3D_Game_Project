using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("�ִ� ����Ʈ ��")]
    [SerializeField] private int maxQuestList;
    public int MaxQuestList => maxQuestList;

    [Header("������ ����Ʈ")]
    [SerializeField] public List<QuestInstance> ActiveQuests = new List<QuestInstance>();
    public HashSet<int> ClearQuests = new HashSet<int>();

    public event Action<QuestClass?> QuestListChanged;
    public event Action<QuestInstance> QuestCleared;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    public void UpdateQuestPrecess(QuestClassification _Type, int _id, int _Amount = 1)
    {
        List<QuestInstance> CompletedQuests = new List<QuestInstance>();

        foreach (var quest in ActiveQuests)
        {
            if (quest.Data.QuestClassification == QuestClassification.NpcTolk && quest.Data.QuestId == _id)
            {
                quest.AddProgress(_Amount);
            }
            if (quest.Data.QuestClassification == QuestClassification.Kill && quest.Data.QuestId == _id)
            {
                quest.AddProgress(_Amount);
            }
            if (quest.Data.QuestClassification == QuestClassification.Collect && quest.Data.QuestId == _id)
            {
                quest.AddProgress(_Amount);
            }

            if (quest.State == QuestCondition.Completed)
            {
                CompletedQuests.Add(quest);
            }
        }

        foreach(var quest in CompletedQuests)
        {
            Debug.Log("����Ʈ �Ϸ�");
            CompletedQuest(quest);
        }
    }

    public void AddQuest(QuestData _Quest)
    {
        if (_Quest == null) return;

        if (ActiveQuests.Count >= MaxQuestList)
        {
            Debug.Log($"����Ʈ �ִ� {MaxQuestList}������ ���� �� �ֽ��ϴ�.");
            return;
        }

        if (_Quest.PrerequisiteQuest != null)
        {
            Debug.Log("���� ����Ʈ�� �Ϸ���� �ʾҽ��ϴ�.");
            if (!ClearQuests.Contains(_Quest.QuestId)) return;
        }

        if (ActiveQuests.Exists(q => q.Data.QuestId == _Quest.QuestId))
        {
            Debug.Log("���� ����Ʈ�� ������ ���� �� �����ϴ�. ");
            return; //���� ����Ʈ �ߺ�����
        }

        QuestInstance NewQuest = new QuestInstance(_Quest);
        NewQuest.State = QuestCondition.Precess;
        ActiveQuests.Add(NewQuest);
        Debug.Log("����Ʈ �����Ϸ�");


        QuestListChanged?.Invoke(null);
    }

    private void RemoveQuest(QuestInstance _Quest)
    {
        if (_Quest == null) return;

        ActiveQuests.Remove(_Quest);

    }

    private void CompletedQuest(QuestInstance _Quest)
    {
        if (_Quest == null) return;

        if(_Quest.Data.QuestClass != QuestClass.Repeat)
        {
            ClearQuests.Add(_Quest.Data.QuestId);
        }

        

        ActiveQuests.Remove(_Quest);

        PlayerStat.Instance.AddExp(_Quest.Data.ExpReward);
        PlayerStat.Instance.Gold += _Quest.Data.GoldRewward;
        QuestCleared?.Invoke(_Quest);
        Debug.Log($"{_Quest.Data.QuestName} �Ϸ�! ����Ʈ ����: {_Quest.Data.GoldRewward}���, {_Quest.Data.ExpReward}����ġ");
      
        QuestListChanged?.Invoke(null); 
    }
}
