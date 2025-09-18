using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //��ųâ �����ܿ� �����Ͽ� ���콺 Ŀ���� ����� �� ��ų���� Ȱ��ȭ
    [SerializeField] public SkillData SkillData;
    [SerializeField] private Image IconImage;
    [SerializeField] SkillTree SkillTree;
    [SerializeField] PlayerSkillBook PlayerSkillBook;
    [SerializeField] GameObject LearnSkillEffectImage;
    [SerializeField] TMP_Text SkillName;
    private Button Button;
    
    private void Awake()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClick);
        LearnSkillEffectImage.SetActive(false);
    }
    private void Start()
    {      
        if (SkillData != null && IconImage != null)
        {
            IconImage.sprite = SkillData.Icon;
        }    

        if(SkillData != null && SkillName != null)
        {
            SkillName.text = SkillData.SkillName;
        }
    }

    public void OnPointerEnter(PointerEventData _EventData)
    {
        if (SkillData != null)
        {
            UI_Tooltip.Instance.ShowTooltip(SkillData, transform.position);
        }
    }

    public void OnPointerExit(PointerEventData _EventData)
    {
        UI_Tooltip.Instance.HideTooltip();
    }

    public void OnPointerClick(PointerEventData _EventData)
    {
        //��Ŭ�� �� �ش� ��ų�� ���濩��Ȯ��(��/��) �� ��ų�� �ִٸ� UI_Manager�� ������ ����(List)�� ��� �ִٸ� �ش� ��ų�������� ����
        //��ų ��뿡 ���� ��Ÿ���� ����
        //��ų���Կ� �� ��ų�� �ٽ� ��Ŭ���ϸ� Remove��

        if(_EventData.button != PointerEventData.InputButton.Right)
        {
            return;//��Ŭ�� �ƴϸ� ����
        }

        if(!PlayerSkillBook.HasSkill(SkillData))
        {
            Debug.Log("��ų�� ����� �ʾҽ��ϴ�.");
            return;
        }

        if(UIManager.Instance.DuplicationSkillSlot(SkillData)) //�ߺ�üũ
        {
            //�ߺ��̸� True => �ش罺ų Remove
            UIManager.Instance.RemoveSkillSlot(SkillData);
            Debug.Log("��ų���Կ� ��ų�� �����մϴ�.");
        }
        else
        {
            //�ߺ������� Set
            UIManager.Instance.SetSkillSlot(SkillData);
            Debug.Log("��ų���Կ� ��ų�� ����մϴ�.");
        }
    }

    private void OnClick()
    {
        //Ŭ�� �� ��ų ����
        SkillTree.LearnSkill(SkillData);
        LearndSkillEffect();
    }

    private void LearndSkillEffect()
    {
        //��ų�� ������ �ִٸ� ��ų���� Effect Ȱ��ȭ
        if(PlayerSkillBook.LearnedSkills.Contains(SkillData))
        {
            LearnSkillEffectImage.SetActive(true);
        }
    } 
}
