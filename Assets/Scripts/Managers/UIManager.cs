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

    [Header("메뉴")]
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] public bool isMenuPanel = false;

    [Header("옵션")]
    [SerializeField] private GameObject OptionPanel;
    [SerializeField] public bool isOptionPanel = false;

    [Header("게임 종료 패널")]
    [SerializeField] private GameObject ExitPanel;
    [SerializeField] private bool isExitPanel = false;

    [Header("플레이어 스킬 UI")]
    [SerializeField] private GameObject skillTree;
    [SerializeField] public bool isSkillTree = false;
    [SerializeField] private PlayerSkillBook PlayerSkillBook;   
    [SerializeField] private List<UI_SkillSlot> UI_SkillSlots;

    [Header("인벤토리 UI")]
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] public bool IsInventoryOpen = false;

    [Header("능력치 UI")]
    [SerializeField] private GameObject StatusPanel;
    [SerializeField] public bool IsStatusOpen = false;

    [Header("퀘스트 UI")]
    [SerializeField] private GameObject QuestPanel;
    [SerializeField] private GameObject QuestToolTipPanel;
    [SerializeField] private GameObject QuestGuidePanel;
    [SerializeField] public bool IsQuestOpen = false;

    [Header("NPC 대화 UI")]
    [SerializeField] public GameObject DialoguePanel;
    [SerializeField] public TextMeshProUGUI NameText;
    [SerializeField] public TextMeshProUGUI DialogueText;
    [SerializeField] public GameObject ChoiceYes;
    [SerializeField] public GameObject ChoiceNo;

    [Header("부활 UI")]
    [SerializeField] private GameObject ResurrectionPanel;

    [Header("조작법 UI")]
    [SerializeField] private GameObject UI_InfoPanel;

    [Header("알림 UI")]
    [SerializeField] private UI_Info uiInfo;

    [Header("UI 버튼그룹")]
    [SerializeField] private Button InventroyButton;
    [SerializeField] private Button SkillTreeButton;
    [SerializeField] private Button QuestButton;
    [SerializeField] private Button MenuButton;

    [Header("보스 UI")]
    [SerializeField] private GameObject BossUIPanel;
     
    private QuestData CurrentQuestData;
    private UI_Status uiStatus;
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

        InitializeUI();
        uiStatus = GetComponent<UI_Status>();
    }

    private void Start()
    {
        InputManager.Instance.OnToggleMenu += OnToggleMenu;
        InputManager.Instance.OnToggleOption += OnToggleOption;
        InputManager.Instance.OnToggleInventory += OnToggleInventory;
        InputManager.Instance.OnToggleStatus += OnToggleStatus;
        InputManager.Instance.OnToggleSkill += OnToggleSkill;
        InputManager.Instance.OnToggleQuest += OnToggleQuest;
        

        if (InventroyButton != null) InventroyButton.onClick.AddListener(OnToggleInventory);
        if (SkillTreeButton != null) SkillTreeButton.onClick.AddListener(OnToggleSkill);
        if (QuestButton != null) QuestButton.onClick.AddListener(OnToggleQuest);
        if (MenuButton != null) MenuButton.onClick.AddListener(OnToggleMenu);
    }

    private void OnToggleMenu()
    {
        isMenuPanel = !isMenuPanel;
        MenuPanel.SetActive(isMenuPanel);

        Time.timeScale = isMenuPanel ? 0f : 1f;

        RefreshCursor();
        SoundManager.Instance.PlaySFX(SFXType.OptionOpen);
    }

    public void OnToggleOption()
    {
        isOptionPanel = !isOptionPanel;
        OptionPanel.SetActive(isOptionPanel);
        RefreshCursor();
        SoundManager.Instance.PlaySFX(SFXType.OptionOpen);
    }

    private void OnToggleInventory()
    {
        IsInventoryOpen = !IsInventoryOpen;
        InventoryPanel.SetActive(IsInventoryOpen);
        RefreshCursor();

        if (IsInventoryOpen)
        {
            InventoryUI.Instance.RefreshUI();
            SoundManager.Instance.PlaySFX(SFXType.InventoryOpen);
        }
        else
        {
            SoundManager.Instance.PlaySFX(SFXType.InventoryClose);
        }
    }

    private void OnToggleStatus()
    {
        IsStatusOpen = !IsStatusOpen;
        StatusPanel.SetActive(IsStatusOpen);

        if(IsStatusOpen)
        {
            StatusPanel.GetComponent<UIStatusPanel>().UpdateStatusUI(PlayerStat.Instance);
            SoundManager.Instance.PlaySFX(SFXType.InventoryOpen);
        }
        else
        {
            SoundManager.Instance.PlaySFX(SFXType.InventoryClose);
        }
    }

    private void OnToggleSkill()
    {
        isSkillTree = !isSkillTree;
        skillTree.SetActive(isSkillTree);
        RefreshCursor();

        if (isSkillTree)
        {
            SoundManager.Instance.PlaySFX(SFXType.QuestOpen);
        }
        else
        {
            SoundManager.Instance.PlaySFX(SFXType.QuestClose);
        }
    }

    private void OnToggleQuest()
    {
        IsQuestOpen = !IsQuestOpen;
        QuestPanel.SetActive(IsQuestOpen);
        QuestToolTipPanel.SetActive(IsQuestOpen);
        RefreshCursor();

        if (IsQuestOpen)
        {
            SoundManager.Instance.PlaySFX(SFXType.QuestOpen);
        }
        else
        {
            SoundManager.Instance.PlaySFX(SFXType.QuestClose);
        }    
    }

    private void InitializeUI()
    {
        MenuPanel.SetActive(false);
        ExitPanel.SetActive(false);
        OptionPanel.SetActive(false);
        DialoguePanel.SetActive(false);
        InventoryPanel.SetActive(false);
        StatusPanel.SetActive(false);
        QuestPanel.SetActive(false);
        QuestToolTipPanel.SetActive(false);
        QuestGuidePanel.SetActive(false);
        ResurrectionPanel.SetActive(false);
        UI_InfoPanel.SetActive(false);
    }

    private void Update()
    {
        UpdateCursorState();
    }

    //마우스 커서 제어
    private void UpdateCursorState()
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
    public void RefreshCursor()
    {
        bool shouldShow = IsInventoryOpen || isSkillTree || IsQuestOpen || isMenuPanel || isOptionPanel || IsActiveCursor;

        if (shouldShow)
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

    public void OnClickExitPanel()
    {
        Debug.Log("게임 종료 패널 호출");
        isExitPanel = !isExitPanel;
        ExitPanel.SetActive(isExitPanel);
    }

    public void OnClickOpen(GameObject gameObject)
    {
        if(gameObject != null)
        {
            gameObject.SetActive(true);
        }
    }

    public void OnClickClose(GameObject gameObject)
    {
        if(gameObject != null)
        {
            gameObject.SetActive(false);
        }
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

    public void ShowResurrectionPanel()
    {
        ResurrectionPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None; // 커서 활성화
        Cursor.visible = true;
    }

    public void HideResurrectionPanel()
    {
        ResurrectionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // 커서 다시 잠금
        Cursor.visible = false;
    }

    public void OnClickResurrection()
    {
        // PlayerController를 찾아 부활 루틴 시작
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.StartResurrection();
            HideResurrectionPanel();    // 패널 닫기
        }
    }

    public void ShowBossHealth(string name, float current, float max)
    {
        uiStatus.SetBossUI(name, current, max);
    }

    public void UpdateBossHealth(float current, float max)
    {
        uiStatus.UpdateBossHp(current, max);
    }

    public void HideBossHealth()
    {
        uiStatus.HideBossUI();
    }

    public void ShowInfo(string key)
    {
        if(uiInfo)
        {
            uiInfo.showInfo(key);
        }
    }

}
