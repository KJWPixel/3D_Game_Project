using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("마우스커서 제어 체크")]
    [SerializeField] public bool IsActiveCursor = false;

    [Header("플레이어 스킬 UI")]
    [SerializeField] SkillTree SkillTree;
    [SerializeField] PlayerSkillBook PlayerSkillBook;   
    [SerializeField] List<UI_SkillSlot> UI_SkillSlots;

    [Header("인벤토리 UI")]
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private bool IsInventoryOpen = false;

    [Header("퀘스트 UI")]
    [SerializeField] private GameObject QuestPanel;
    [SerializeField] private GameObject QuestToolTipPanel;
    [SerializeField] private GameObject QuestGuidePanel;
    [SerializeField] private bool IsQuestOpen = false;

    [Header("NPC 대화 UI")]
    [SerializeField] public GameObject DialoguePanel;
    [SerializeField] public TextMeshProUGUI NameText;
    [SerializeField] public TextMeshProUGUI DialogueText;
    [SerializeField] public GameObject ChoiceYes;
    [SerializeField] public GameObject ChoiceNo;

    private QuestData CurrentQuestData;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            GameObject root = gameObject.transform.root.gameObject;
            DontDestroyOnLoad(root);
        }
        else
        {
            Destroy(gameObject);
        }

        InitUI();
    }

    private void InitUI()
    {
        DialoguePanel.SetActive(false);
        InventoryPanel.SetActive(false);
        QuestPanel.SetActive(false);
        QuestToolTipPanel.SetActive(false);
        QuestGuidePanel.SetActive(false);
    }

    private void Update()
    {
        CursorActive();

        if(Input.GetKeyDown(KeyCode.I))
        {
            InventoryOpen();
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            QuestOpen();
        }
    }

    //마우스 커서 제어
    private void CursorActive()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            IsActiveCursor = !IsActiveCursor;
            
            if(IsActiveCursor)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false; 
            }
        }
    }

    private void InventoryOpen()
    {
        IsInventoryOpen = !IsInventoryOpen;
        InventoryPanel.SetActive(IsInventoryOpen);

        if(IsInventoryOpen)
        {
            InventoryUI.Instance.RefreshUI();
        }
    }

    private void QuestOpen()
    {
        IsQuestOpen = !IsQuestOpen;
        QuestPanel.SetActive(IsQuestOpen);
        QuestToolTipPanel.SetActive(IsQuestOpen);
    }

    //NPC 대화 대사 분기 버튼
    public void OnClickYes()
    {
        Debug.Log("Yes 선택");

        var NPC = DialogueManager.Instance.CurrentNPC;
        if (NPC == null) return;

        //추후 후속으로 필요한 기능 추가
        switch(NPC.interactionType)
        {
            case InteractionType.Shop:
                break;
            case InteractionType.Quest:
                if (CurrentQuestData != null)
                {
                    QuestManager.Instance.AddQuest(CurrentQuestData);                   
                    Debug.Log($"{CurrentQuestData.QuestName} 수락");
                }
                break;
          
        }
       
        ChoiceYes.SetActive(false);
        ChoiceNo.SetActive(false);
        DialoguePanel.SetActive(false);
    }
    public void OnClickNo()
    {
        Debug.Log("No 선택 ");

        DialogueManager.Instance.Index = 0;
        ChoiceYes.SetActive(false);
        ChoiceNo.SetActive(false);
        DialoguePanel.SetActive(false);
    }

    public void OnClickShop()
    {
        Debug.Log("상점 페이지 활성화");

        ChoiceYes.SetActive(false);
        ChoiceNo.SetActive(false);
        DialoguePanel.SetActive(false);
    }

    public void SetupQuestButton(QuestData _Quest)
    {
        Debug.Log("버튼 호출");
        CurrentQuestData = _Quest;
        

        ChoiceYes.GetComponent<Button>().onClick.RemoveAllListeners();
        ChoiceYes.GetComponent<Button>().onClick.AddListener(OnClickYes);
        
        ChoiceNo.GetComponent<Button>().onClick.RemoveAllListeners();
        ChoiceNo.GetComponent<Button>().onClick.AddListener(OnClickNo);

        ChoiceYes.SetActive(true);
        ChoiceNo.SetActive(true);
    }


    //스킬 슬롯 Set,Remove, 중복확인
    public void SetSkillSlot(SkillData _SkillData)
    {
        for(int Index = 0; Index < UI_SkillSlots.Count; Index++)
        {
            if (UI_SkillSlots[Index].SkillData == null)
            {
                UI_SkillSlots[Index].SetIcon(_SkillData);
                break;
            }
        }
    }
    public void RemoveSkillSlot(SkillData _SkillData)
    {
        for(int Index = 0; Index < UI_SkillSlots.Count; Index++)
        {
            if (UI_SkillSlots[Index].SkillData == _SkillData)
            {
                UI_SkillSlots[Index].SetIcon(null);
                break;
            }
        }
    }
    public bool DuplicationSkillSlot(SkillData _SkillData)
    {
        for(int Index = 0; Index < UI_SkillSlots.Count; Index++)
        {
            if (UI_SkillSlots[Index].SkillData == _SkillData)
            {
                return true;//중복 확인
            }
        }
        return false;//중복 없음
    }

    public SkillData GetSkillFromSlot(int _Index)
    {
        if (_Index >= 0 && _Index < UI_SkillSlots.Count)
        {
            return UI_SkillSlots[_Index].SkillData;
        }
        return null;
    }

    //Stack을 이용한 Pop
    #region
    //public void OpnePopup()
    //{
    //    GameObject Panels = Instantiate(Panel, Parent);
    //    panelStack.Push(Panels);
    //}

    //public void ClosePopup()
    //{
    //    if(Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        if(panelStack.Count == 0)
    //        {
    //            return;
    //        }
    //        GameObject ClosePanel = panelStack.Pop();
    //        Destroy(ClosePanel);
    //    }
    //}
    #endregion 

}
