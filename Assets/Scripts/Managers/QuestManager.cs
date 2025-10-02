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

    [Header("�ִ� ����Ʈ ��")]
    [SerializeField] private int MaxQuestList;

    [Header("������ ����Ʈ")]
    [SerializeField] private List<QuestInstance> ActiveQuests = new List<QuestInstance>();
    private HashSet<int> ClearQuests = new HashSet<int>();

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

    public void UpdateQuestPrecess(QuestType _Type, int _id, int _Amount = 1)
    {
        foreach (var quest in ActiveQuests)
        {
            if (quest.Data.QuestType == QuestType.NpcTolk && quest.Data.Id == _id)
            {
                quest.AddProgress(_Amount);

            }
            if (quest.Data.QuestType == QuestType.Kill && quest.Data.Id == _id)
            {

                quest.AddProgress(_Amount);

            }
            if (quest.Data.QuestType == QuestType.Collect && quest.Data.Id == _id)
            {

                quest.AddProgress(_Amount);

            }

            if (quest.State == QuestState.Completed)
            {
                Debug.Log("����Ʈ �Ϸ�");
            }
        }
    }

    public void AddQuest(QuestData _Quest)
    {
        if (_Quest == null) return;
        if (ActiveQuests.Count >= MaxQuestList) return;


        QuestInstance NewQuest = new QuestInstance(_Quest);
        ActiveQuests.Add(NewQuest);
    }

    private void RemoveQuest(QuestInstance _Quest)
    {
        if (_Quest == null) return;

        ActiveQuests.Remove(_Quest);
    }

    private void CompletedQuest(QuestInstance _Quest)
    {
        if (_Quest == null) return;

        ClearQuests.Add(_Quest.Data.Id);
        ActiveQuests.Remove(_Quest);

        PlayerStat.Instance.AddExp(_Quest.Data.ExpReward);
        PlayerStat.Instance.Gold += _Quest.Data.GoldRewward;

        Debug.Log($"{_Quest.Data.QuestName} �Ϸ�! ����Ʈ ����: {_Quest.Data.GoldRewward}���, {_Quest.Data.ExpReward}����ġ");
    }
}
