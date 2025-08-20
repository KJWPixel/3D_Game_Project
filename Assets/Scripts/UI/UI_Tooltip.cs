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

        TooltipExtra.text = $"요구 레벨:{_Data.RequireLevel}\n요구 스킬포인트  :{_Data.RequireSP}\n재사용 대기시간  :{_Data.Cooldown}\n스킬데미지 :{_Data.Power}";

    }

    public void HideTooltip()
    {
        TooltipPanel.SetActive(false);
    }

    private string GetDescriptionByType(SkillData _Data)
    {
        switch (_Data.type)
        {
            case SkillType.Damage: return "대상을 공격하는 스킬입니다.";
            case SkillType.Heal: return "체력을 회복하는 스킬입니다.";
            case SkillType.Buff: return "능력을 강화하는 버프 스킬입니다.";
            case SkillType.Debuff: return "적을 약화시키는 디버프 스킬입니다.";
            default: return "설명 없음";
        }
    }
}
