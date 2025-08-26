using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class UI_SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //��ųâ �����ܿ� �����Ͽ� ���콺 Ŀ���� ����� �� ��ų���� Ȱ��ȭ
    [SerializeField] public SkillData SkillData;
    [SerializeField] private Image IconImage;
    [SerializeField] SkillTree SkillTree;
    [SerializeField] PlayerSkillBook PlayerSkillBook;
    [SerializeField] GameObject LearnSkillEffectImage;
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

        if(UI_Manager.Instance.DuplicationSkillSlot(SkillData)) //�ߺ�üũ
        {
            //�ߺ��̸� True => �ش罺ų Remove
            UI_Manager.Instance.RemoveSkillSlot(SkillData);
            Debug.Log("��ų���Կ� ��ų�� �����մϴ�.");
        }
        else
        {
            //�ߺ������� Set
            UI_Manager.Instance.SetSkillSlot(SkillData);
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
