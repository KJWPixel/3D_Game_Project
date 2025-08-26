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

    private string GetDescriptionByType(SkillData _SkillData)
    {
        if(_SkillData == null || _SkillData.Effects.Count == 0)
        {
            return "설명없음";
        }

        string Description = "";

        foreach(var Effect in _SkillData.Effects)
        {
            switch (Effect.EffectType)
            {
                case SkillEffectType.Damage:
                    Description += $"대상을 공격하여 {Effect.Power} 데미지를 입힙니다.\n";
                    break;
                case SkillEffectType.Heal:
                    Description += $"대상을 회복하여 {Effect.Power} 체력을 회복합니다.\n";
                    break;
                case SkillEffectType.Buff:
                    Description += $"능력을 {Effect.Power} 만큼 강화하는 버프를 적용합니다.\n";
                    break;
                case SkillEffectType.Debuff:
                    Description += $"적에게 {Effect.Power} 만큼 약화 효과를 {Effect.Duration}초 동안 적용합니다.\n";
                    break;
                case SkillEffectType.CC:
                    Description += $"적에게 상태이상 효과 {Effect.Duration}초 동안 적용합니다.\n";
                    break;
                case SkillEffectType.Resource:
                    Description += $"자원을 {Effect.Power}을 만큼 회복합니당.\n";
                    break;
            }

        }

        return Description.TrimEnd();
        //switch (_Data.Type)
        //{
        //    case SkillType.Damage: return "대상을 공격하는 스킬입니다.";
        //    case SkillType.Heal: return "체력을 회복하는 스킬입니다.";
        //    case SkillType.Buff: return "능력을 강화하는 버프 스킬입니다.";
        //    case SkillType.Debuff: return "적을 약화시키는 디버프 스킬입니다.";
        //    default: return "설명 없음";
        //}
    }
}
