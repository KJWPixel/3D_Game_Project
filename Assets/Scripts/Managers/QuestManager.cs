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



    [SerializeField] private int[] QuestCount = new int[30];

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
}
