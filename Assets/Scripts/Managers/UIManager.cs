using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
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

    [Header("NPC ��ȭ UI")]
    [SerializeField] public GameObject DialoguePanel;
    [SerializeField] public TextMeshProUGUI NameText;
    [SerializeField] public TextMeshProUGUI DialogueText;
    [SerializeField] public GameObject ChoiceYes;
    [SerializeField] public GameObject ChoiceNo;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
    }

    private void Update()
    {
        CursorActive();
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

    //NPC ��ȭ ��� �б� ��ư
    public void OnClickYes()
    {
        Debug.Log("Yes ����");
        //���� �ļ����� �ʿ��� ��� �߰�
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
