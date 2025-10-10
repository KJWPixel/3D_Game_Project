using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    /*
    QuestData: 퀘스트 정보Interaction
    퀘스트 ID: int
    퀘스트 이름: string
    퀘스트 설명: string
    선행 퀘스트: questData
    퀘스트 Target: NPC, Monster, QuestMaterial: Enum
    퀘스트 상태: Enum
    퀘스트 보상: List<int>
    */

    /*QuestManager: 퀘스트제어 및 업데이트 
    퀘스트수 제한: [], List로 퀘스트수락 제한

    퀘스트 수락: AddQuest()
    퀘스트 포기: RemoveQuest()
    퀘스트 업데이트: QuestUpdate()
    퀘스트 완료 딕셔너리 관리

    퀘스트 목적함수: NPCInteraction(): OnTrigger 또는 NPC를 참조하여 거리계산
    퀘스트 목적함수: EnemyKill(): 해당 Enemy(id로 판별) Die()함수에 참조

    */

    //퀘스트매니저  중앙관리
    //NCP(NPC한테 QuestID 부여) >> 대화(Dialogue창) >> 기본적인 대화 후 퀘스트or상점or기타 등등 List목록 SetActive >> 퀘스트 수락

    //NPC퀘스트(퀘스트ID, Level, 선행)데이터 >> GiveQuest()함수로 QuestManager한테 전달 >> 퀘스트 조건? 참:거짓 bool에 따라 퀘스트 수락

    [Header("최대 퀘스트 수")]
    [SerializeField] private int maxQuestList;
    public int MaxQuestList => maxQuestList;

    [Header("수락한 퀘스트")]
    [SerializeField] public List<QuestInstance> ActiveQuests = new List<QuestInstance>();
    public HashSet<int> ClearQuests = new HashSet<int>();

    public event Action<QuestClass?> QuestListChanged;

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
                Debug.Log("퀘스트 완료");
            }
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

        if(_Quest.PrerequisiteQuest != null)
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

        ClearQuests.Add(_Quest.Data.QuestId);
        ActiveQuests.Remove(_Quest);

        PlayerStat.Instance.AddExp(_Quest.Data.ExpReward);
        PlayerStat.Instance.Gold += _Quest.Data.GoldRewward;

        Debug.Log($"{_Quest.Data.QuestName} 완료! 퀘스트 보상: {_Quest.Data.GoldRewward}골드, {_Quest.Data.ExpReward}경험치");
    }
}
