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
                Debug.Log("��ų�� ���Կ� ���ϴ�.");
                //��ų�� ����� ��Ŭ������ ���Կ� ���� ����
                //��Ŭ�� �� UI_Manager ȣ��, UI_Manager�� �ش� ������ �����ϰ� �����Ƿ� 5�� ������ �ε��� ������ ���� ����ִٸ� �ش� ��ų������ �̹����� �־��ָ鼭
                //�ε������� �ش� ��ų�� ����
            }
            else if(!PlayerSkillBook.LearnedSkills.Contains(SkillData))
            {
                Debug.Log("�ش� ��ų�� ����� �ʾҽ��ϴ�.");
                //��ų�� ����� �ʾҴٸ�
            }
            else
            {
                //��ų�� ����� �ٽ��ѹ� ��Ŭ�� �� ���Կ� �ش� ��ų�� �M Romove(SkillData)
            }

          
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
