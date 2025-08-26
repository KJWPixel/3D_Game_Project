using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class UI_SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //스킬창 아이콘에 부착하여 마우스 커서에 닿았을 시 스킬툴팁 활성화
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
        //우클릭 시 해당 스킬을 습득여부확인(유/무) 후 스킬이 있다면 UI_Manager에 참조된 슬롯(List)에 비어 있다면 해당 스킬아이콘을 참조
        //스킬 사용에 따른 쿨타임을 연출
        //스킬슬롯에 들어간 스킬을 다시 우클릭하면 Remove함

        if(_EventData.button != PointerEventData.InputButton.Right)
        {
            return;//우클릭 아니면 리턴
        }

        if(UI_Manager.Instance.DuplicationSkillSlot(SkillData)) //중복체크
        {
            //중복이면 True => 해당스킬 Remove
            UI_Manager.Instance.RemoveSkillSlot(SkillData);
            Debug.Log("스킬슬롯에 스킬을 해제합니다.");
        }
        else
        {
            //중복없으면 Set
            UI_Manager.Instance.SetSkillSlot(SkillData);
            Debug.Log("스킬슬롯에 스킬을 등록합니다.");
        }
    }

    private void OnClick()
    {
        //클릭 시 스킬 습득
        SkillTree.LearnSkill(SkillData);
        LearndSkillEffect();
    }

    private void LearndSkillEffect()
    {
        //스킬을 가지고 있다면 스킬습득 Effect 활성화
        if(PlayerSkillBook.LearnedSkills.Contains(SkillData))
        {
            LearnSkillEffectImage.SetActive(true);
        }
    } 
}
