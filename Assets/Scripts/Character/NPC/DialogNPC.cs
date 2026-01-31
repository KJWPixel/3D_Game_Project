using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DialogNPC : NPCCharacter
{
    [Header("NPC 대화 옵션")]
    public InteractionType interactionType;
    public List<QuestData> questList = new List<QuestData>();
    public Transform TargetTrs;

    [Header("퀘스트마커")]
    [SerializeField] public GameObject MainQuestMarker;
    [SerializeField] public GameObject SideQuestMarker;
    [SerializeField] public GameObject RepeatQuestMarker;
    [SerializeField] public GameObject EventQuestMarker;
    [SerializeField] private GameObject currentQuestMarker;

    [Header("상점 아이템 목록")]
    [SerializeField] public List<ItemData> itemDatas = new List<ItemData>();

    private Camera MainCamera;

    private bool isShowingMarker = false;
    private float sqrNameDistance;        // Awake()에서 미리 제곱

    private void Awake()
    {
        sqrNameDistance = NameDistance * NameDistance; 
    }

    private void Start()
    {
        MainCamera = Camera.main;

        if (NPCText == null && NPCMark != null)
        {
            NPCText = NPCMark.gameObject.GetComponent<TextMeshPro>();
        }

        if (Player == null)
        {
            Player = GameManager.Instance.Player;
        }


        NPCSetup();
    }
    private void Update()
    {
        Interact();
        UpdateShowMarker();
    }

    private void LateUpdate()
    {
        //NPCNameOn();
        //UpdateShowMarker();
    }

    private void NPCSetup()
    {
        if(MainQuestMarker != null)
        {
            MainQuestMarker.SetActive(false);
        }
        if(SideQuestMarker != null)
        {
            SideQuestMarker.SetActive(false);
        }
        if(RepeatQuestMarker != null)
        {
            RepeatQuestMarker.SetActive(false);
        }
        if(EventQuestMarker != null)
        {
            EventQuestMarker.SetActive(false);
        }
        
        if (NPCText != null)
        {
            NPCText.text = NpcName;
        }
        
        if(NPCMark != null)
        {
            NPCMark.gameObject.SetActive(false);
        }

        QuestData priorityQuest = GetPriorityQuest();

        if (priorityQuest != null)
        {
            switch (priorityQuest.QuestClass)
            {
                case QuestClass.Main: currentQuestMarker = MainQuestMarker; break;
                case QuestClass.Sub: currentQuestMarker = SideQuestMarker; break;
                case QuestClass.Repeat: currentQuestMarker = RepeatQuestMarker; break;
                case QuestClass.Event: currentQuestMarker = EventQuestMarker; break;
            }
        }

        if (currentQuestMarker != null)
        {
            currentQuestMarker.SetActive(true);
            currentQuestMarker.transform.localPosition = new Vector3(0, 3, 0);
        }
    }

    private void NPCNameOn()
    {
        Playerdistance = Vector3.Distance(transform.position, Player.transform.position);
        if (Playerdistance > NameDistance || Player == null)
        {
            NPCMark.gameObject.SetActive(false);
            return;
        }
        else if(Playerdistance < NameDistance)
        {
            NPCMark.gameObject.SetActive(true);
            Quaternion targetRotation = MainCamera.transform.rotation;
            NPCMark.transform.rotation = targetRotation;
        }
    }

    private void UpdateShowMarker()
    {
        if(Player == null)
        {
            if(isShowingMarker)
            {
                isShowingMarker = false;
            }
            return;
        }

        float sqrDist = (transform.position - Player.transform.position).sqrMagnitude;
        bool shouldShow = sqrDist < sqrNameDistance;

        if(shouldShow != isShowingMarker)
        {
            SetMarkerActive(shouldShow);
            isShowingMarker = shouldShow; 
        }   

        // 회전은 네임, 마커가 보일 떄만
        if(shouldShow)
        {
            NPCMark.transform.LookAt(MainCamera.transform);
            NPCMark.transform.Rotate(0, 180, 0); //앞면으로 필요 시

            if (currentQuestMarker != null)
            {
                currentQuestMarker.transform.LookAt(MainCamera.transform);
                currentQuestMarker.transform.Rotate(0, 180, 0);
            }           
        }
    }

    private void SetMarkerActive(bool active)
    {
        NPCMark.SetActive(active);

        if(currentQuestMarker == null)
        {
            return;
        }

        currentQuestMarker.SetActive(active);
    }

    public QuestData GetPriorityQuest()
    {
        foreach (var quest in questList)
        {
            // 이미 완료한 퀘스트가 아니고, 현재 진행 중인 퀘스트도 아닐 때
            if (!QuestManager.Instance.ClearQuests.Contains(quest.QuestId) &&
                !QuestManager.Instance.ActiveQuests.Exists(q => q.Data.QuestId == quest.QuestId))
            {
                return quest; // 우선순위대로 첫 번째 퀘스트 반환
            }
        }
        return null;
    }

    public QuestData GetAvailableQuest()
    {
        foreach (var quest in questList)
        {
            // 아직 클리어하지 않았고, 현재 진행 중도 아닌 퀘스트 반환
            bool isCleared = QuestManager.Instance.ClearQuests.Contains(quest.QuestId);
            bool isActive = QuestManager.Instance.ActiveQuests.Exists(q => q.Data.QuestId == quest.QuestId);

            if (!isCleared && !isActive) return quest;
        }
        return null;
    }

    private void Interact()
    {
        Playerdistance = Vector3.Distance(transform.position, Player.transform.position);

        // 대화 중인데 멀어지면 종료
        if (isTolk && Playerdistance > InsteractionRange)
        {
            isTolk = false;
            DialogueManager.Instance.CloseDialogue(false); // EndDialogue()
            ShopManager.Instance.CloseShop();
            return;
        }

        if (ShopManager.Instance != null && ShopManager.Instance.IsShopOpen())  // IsShopOpen() 함수 아래에 추가할게요
        {
            // 상점 열려 있으면 E키 무시 (또는 필요 시 안내 메시지)
            if (Input.GetKeyDown(KeyCode.E))
            {
                // 선택적: Debug.Log("상점 이용 중에는 대화를 시작할 수 없습니다.");
                return;
            }
        }

        // E키 입력 처리
        if (Playerdistance < InsteractionRange && Input.GetKeyDown(KeyCode.E))
        {

            if(!DialogueManager.Instance.IsDialogueActive)
            {
                //대화시작
                isTolk = true;
                DialogueManager.Instance.StartDialogueWithLocalization(this, interactionType);

            }
            else
            {
                DialogueManager.Instance.ContinueDialogue();
            }
            //isTolk = true;
            //Debug.Log("StartDialogue");

            //DialogueManager.Instance.StartDialogue(this, Name, DialogueLines, interactionType);
        }
    }
}
