using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    /*
    QuestData: ����Ʈ ����Interaction
    ����Ʈ ID: int
    ����Ʈ �̸�: string
    ����Ʈ ����: string
    ���� ����Ʈ: questData
    ����Ʈ Target: NPC, Monster, QuestMaterial: Enum
    ����Ʈ ����: Enum
    ����Ʈ ����: List<int>
    */

    /*QuestManager: ����Ʈ���� �� ������Ʈ 
    ����Ʈ�� ����: [], List�� ����Ʈ���� ����

    ����Ʈ ����: AddQuest()
    ����Ʈ ����: RemoveQuest()
    ����Ʈ ������Ʈ: QuestUpdate()
    ����Ʈ �Ϸ� ��ųʸ� ����

    ����Ʈ �����Լ�: NPCInteraction(): OnTrigger �Ǵ� NPC�� �����Ͽ� �Ÿ����
    ����Ʈ �����Լ�: EnemyKill(): �ش� Enemy(id�� �Ǻ�) Die()�Լ��� ����

    */

    //����Ʈ�Ŵ���  �߾Ӱ���
    //NCP(NPC���� QuestID �ο�) >> ��ȭ(Dialogueâ) >> �⺻���� ��ȭ �� ����Ʈor����or��Ÿ ��� List��� SetActive >> ����Ʈ ����

    //NPC����Ʈ(����ƮID, Level, ����)������ >> GiveQuest()�Լ��� QuestManager���� ���� >> ����Ʈ ����? ��:���� bool�� ���� ����Ʈ ����

    [Header("�ִ� ����Ʈ ��")]
    [SerializeField] private int maxQuestList;
    public int MaxQuestList => maxQuestList;

    [Header("������ ����Ʈ")]
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
                Debug.Log("����Ʈ �Ϸ�");
            }
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

        if(_Quest.PrerequisiteQuest != null)
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

        ClearQuests.Add(_Quest.Data.QuestId);
        ActiveQuests.Remove(_Quest);

        PlayerStat.Instance.AddExp(_Quest.Data.ExpReward);
        PlayerStat.Instance.Gold += _Quest.Data.GoldRewward;

        Debug.Log($"{_Quest.Data.QuestName} �Ϸ�! ����Ʈ ����: {_Quest.Data.GoldRewward}���, {_Quest.Data.ExpReward}����ġ");
    }
}
