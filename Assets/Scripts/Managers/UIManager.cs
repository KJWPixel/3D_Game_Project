using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("���콺Ŀ�� ���� üũ")]
    [SerializeField] public bool IsActiveCursor = false;

    [Header("�÷��̾� ��ų UI")]
    [SerializeField] SkillTree SkillTree;
    [SerializeField] PlayerSkillBook PlayerSkillBook;   
    [SerializeField] List<UI_SkillSlot> UI_SkillSlots;

    [Header("�κ��丮 UI")]
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private bool IsInventoryOpen = false;

    [Header("����Ʈ UI")]
    [SerializeField] private GameObject QuestPanel;
    [SerializeField] private GameObject QuestToolTipPanel;
    [SerializeField] private GameObject QuestGuidePanel;
    [SerializeField] private bool IsQuestOpen = false;

    [Header("NPC ��ȭ UI")]
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

    //���콺 Ŀ�� ����
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

    //NPC ��ȭ ��� �б� ��ư
    public void OnClickYes()
    {
        Debug.Log("Yes ����");

        var NPC = DialogueManager.Instance.CurrentNPC;
        if (NPC == null) return;

        //���� �ļ����� �ʿ��� ��� �߰�
        switch(NPC.interactionType)
        {
            case InteractionType.Shop:
                break;
            case InteractionType.Quest:
                if (CurrentQuestData != null)
                {
                    QuestManager.Instance.AddQuest(CurrentQuestData);                   
                    Debug.Log($"{CurrentQuestData.QuestName} ����");
                }
                break;
          
        }
       
        ChoiceYes.SetActive(false);
        ChoiceNo.SetActive(false);
        DialoguePanel.SetActive(false);
    }
    public void OnClickNo()
    {
        Debug.Log("No ���� ");

        DialogueManager.Instance.Index = 0;
        ChoiceYes.SetActive(false);
        ChoiceNo.SetActive(false);
        DialoguePanel.SetActive(false);
    }

    public void OnClickShop()
    {
        Debug.Log("���� ������ Ȱ��ȭ");

        ChoiceYes.SetActive(false);
        ChoiceNo.SetActive(false);
        DialoguePanel.SetActive(false);
    }

    public void SetupQuestButton(QuestData _Quest)
    {
        Debug.Log("��ư ȣ��");
        CurrentQuestData = _Quest;
        

        ChoiceYes.GetComponent<Button>().onClick.RemoveAllListeners();
        ChoiceYes.GetComponent<Button>().onClick.AddListener(OnClickYes);
        
        ChoiceNo.GetComponent<Button>().onClick.RemoveAllListeners();
        ChoiceNo.GetComponent<Button>().onClick.AddListener(OnClickNo);

        ChoiceYes.SetActive(true);
        ChoiceNo.SetActive(true);
    }


    //��ų ���� Set,Remove, �ߺ�Ȯ��
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
                return true;//�ߺ� Ȯ��
            }
        }
        return false;//�ߺ� ����
    }

    public SkillData GetSkillFromSlot(int _Index)
    {
        if (_Index >= 0 && _Index < UI_SkillSlots.Count)
        {
            return UI_SkillSlots[_Index].SkillData;
        }
        return null;
    }

    //Stack�� �̿��� Pop
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
