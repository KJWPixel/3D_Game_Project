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
