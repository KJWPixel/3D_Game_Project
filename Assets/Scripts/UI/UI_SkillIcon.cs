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
    private UI_Manager UI_Manager;

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

        UI_Manager.Instance = GetComponent<UI_Manager>();   
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
        if (_EventData.button == PointerEventData.InputButton.Right)
        {
            if(PlayerSkillBook.LearnedSkills.Contains(SkillData))
            {
                Debug.Log("스킬이 슬롯에 들어갑니다.");
                //스킬을 배웠고 우클릭으로 슬롯에 들어가는 로직
                //우클릭 시 UI_Manager 호출, UI_Manager가 해당 슬롯을 관리하고 있으므로 5개 슬롯의 인덱스 순서에 따라 비어있다면 해당 스킬아이콘 이미지를 넣어주면서
                //인덱스에도 해당 스킬을 저장
            }
            else if(!PlayerSkillBook.LearnedSkills.Contains(SkillData))
            {
                Debug.Log("해당 스킬은 배우지 않았습니다.");
                //스킬을 배우지 않았다면
            }
            else
            {
                //스킬을 배웠고 다시한번 우클릭 시 슬롯에 해당 스킬을 뻄 Romove(SkillData)
            }

          
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
