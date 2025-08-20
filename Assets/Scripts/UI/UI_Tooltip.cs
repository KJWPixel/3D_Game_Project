using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tooltip : MonoBehaviour
{
    public static UI_Tooltip Instance;

    [SerializeField] private GameObject TooltipPanel;
    [SerializeField] private TMP_Text TooltipName;
    [SerializeField] private TMP_Text TooltipDesc;
    [SerializeField] private TMP_Text TooltipExtra; 
    [SerializeField] private Image TooltipIcon;

    private void Awake()
    {
        Instance = this;
        TooltipPanel.SetActive(false);
    }

    public void ShowTooltip(SkillData _Data, Vector3 _Position)
    {
        TooltipPanel.SetActive(true);
        TooltipPanel.transform.position = _Position + new Vector3(0, 0, 0); 
        TooltipIcon.sprite = _Data.Icon;

        TooltipName.text = $"{_Data.SkillName}";
        TooltipDesc.text = GetDescriptionByType(_Data);

        TooltipExtra.text = $"�䱸 ����:{_Data.RequireLevel}\n�䱸 ��ų����Ʈ  :{_Data.RequireSP}\n���� ���ð�  :{_Data.Cooldown}\n��ų������ :{_Data.Power}";

    }

    public void HideTooltip()
    {
        TooltipPanel.SetActive(false);
    }

    private string GetDescriptionByType(SkillData _Data)
    {
        switch (_Data.type)
        {
            case SkillType.Damage: return "����� �����ϴ� ��ų�Դϴ�.";
            case SkillType.Heal: return "ü���� ȸ���ϴ� ��ų�Դϴ�.";
            case SkillType.Buff: return "�ɷ��� ��ȭ�ϴ� ���� ��ų�Դϴ�.";
            case SkillType.Debuff: return "���� ��ȭ��Ű�� ����� ��ų�Դϴ�.";
            default: return "���� ����";
        }
    }
}
