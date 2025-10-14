using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("최대 퀘스트 수")]
    [SerializeField] private int maxQuestList;
    public int MaxQuestList => maxQuestList;

    [Header("수락한 퀘스트")]
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
            Debug.Log("퀘스트 완료");
            CompletedQuest(quest);
        }
    }

    public void AddQuest(QuestData _Quest)
    {
        if (_Quest == null) return;

        if (ActiveQuests.Count >= MaxQuestList)
        {
            Debug.Log($"퀘스트 최대 {MaxQuestList}까지만 받을 수 있습니다.");
            return;
        }

        if (_Quest.PrerequisiteQuest != null)
        {
            Debug.Log("선행 퀘스트가 완료되지 않았습니다.");
            if (!ClearQuests.Contains(_Quest.QuestId)) return;
        }

        if (ActiveQuests.Exists(q => q.Data.QuestId == _Quest.QuestId))
        {
            Debug.Log("동일 퀘스트를 복수로 받을 수 없습니다. ");
            return; //동일 퀘스트 중복방지
        }

        QuestInstance NewQuest = new QuestInstance(_Quest);
        NewQuest.State = QuestCondition.Precess;
        ActiveQuests.Add(NewQuest);
        Debug.Log("퀘스트 수락완료");


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
        Debug.Log($"{_Quest.Data.QuestName} 완료! 퀘스트 보상: {_Quest.Data.GoldRewward}골드, {_Quest.Data.ExpReward}경험치");
      
        QuestListChanged?.Invoke(null); 
    }
}
