using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UI_SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //스킬창 아이콘에 부착하여 마우스 커서에 닿았을 시 스킬툴팁 활성화
    [SerializeField] public SkillData SkillData;
    [SerializeField] private Image IconImage;
    [SerializeField] public SkillTree SkillTree;
    [SerializeField] public PlayerSkillBook PlayerSkillBook;
    [SerializeField] public GameObject LearnSkillEffectImage;
    [SerializeField] public TMP_Text SkillName;
    private Button Button;
    
    private void Awake()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClick);
        LearnSkillEffectImage.SetActive(false);
    }

    private void OnEnable()
    {
        // 1. 언어 설정이 바뀌었을 때 실행될 함수(OnLocaleChanged)를 등록합니다.
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

        // 2. 처음 켰을 때도 한 번은 업데이트 해줘야 합니다.
        UpdateSkillIcon();
    }

    private void OnDisable()
    {
        // 3. 오브젝트가 사라질 때는 등록을 해제해야 메모리 누수가 없습니다.
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(Locale locale)
    {
        UpdateSkillIcon();
    }
    private void Start()
    {      
        if (SkillData != null && IconImage != null)
        {
            IconImage.sprite = SkillData.Icon;
        }
       
        SkillName.text = LocalizationSettings.StringDatabase.GetLocalizedString("SKIl Table", SkillData.SkillKey);   
    }

    private void UpdateSkillIcon()
    {
        SkillName.text = LocalizationSettings.StringDatabase.GetLocalizedString("SKIl Table", SkillData.SkillKey);
    }

    public void OnPointerEnter(PointerEventData _EventData)
    {
        if (SkillData != null)
        {
            if (!SkillTree.gameObject.activeSelf) return;  
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

        if(!PlayerSkillBook.HasSkill(SkillData))
        {
            Debug.Log("스킬을 배우지 않았습니다.");
            return;
        }

        if(UIManager.Instance.DuplicationSkillSlot(SkillData)) //중복체크
        {
            //중복이면 True => 해당스킬 Remove
            UIManager.Instance.RemoveSkillSlot(SkillData);
            Debug.Log("스킬슬롯에 스킬을 해제합니다.");
        }
        else
        {
            //중복없으면 Set
            UIManager.Instance.SetSkillSlot(SkillData);
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
