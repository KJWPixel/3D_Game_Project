using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tooltip : MonoBehaviour
{
    public static UI_Tooltip Instance;

    [SerializeField] GameObject SkillTree;
    [SerializeField] GameObject TooltipPanel;
    [SerializeField] TMP_Text TooltipName;
    [SerializeField] TMP_Text TooltipDesc;
    [SerializeField] TMP_Text TooltipExtra; 
    [SerializeField] Image TooltipIcon;

    private void Awake()
    {
        Instance = this;
        TooltipPanel.SetActive(false);
    }

    public void ShowTooltip(SkillData _Data, Vector3 _Position)
    {
        if (TooltipPanel == null)
        {
            Debug.Log("TooltipPanel NULL");
            return;
        }

        TooltipPanel.SetActive(true);
        TooltipPanel.transform.position = _Position + new Vector3(0, 0, 0);
        TooltipIcon.sprite = _Data.Icon;

        TooltipName.text = $"{_Data.SkillName}";
        TooltipDesc.text = GetDescriptionByType(_Data);

        string effectDesc = "";
        foreach (var effect in _Data.Effects)
        {
            //string EffectsDesc = GetDescriptionByType(_Data);
            effectDesc += effect.Power.ToString();

            switch (effect.EffectType)
            {
                case SkillEffectType.RayDamage:
                case SkillEffectType.LineAreaDamage:
                case SkillEffectType.TargetAreaDamage:
                case SkillEffectType.DistanceAreaDamage:
                    TooltipExtra.text = $"요구 레벨:{_Data.RequireLevel}\n요구 스킬포인트  :{_Data.RequireSP}\n재사용 대기시간  :{_Data.Cooldown}\n스킬데미지 :{effectDesc}";
                    break;
                case SkillEffectType.Heal:
                case SkillEffectType.HealBuff:
                    TooltipExtra.text = $"요구 레벨:{_Data.RequireLevel}\n요구 스킬포인트  :{_Data.RequireSP}\n재사용 대기시간  :{_Data.Cooldown}\n회복량 :{effectDesc}";
                    break;
                case SkillEffectType.AtkBuff:
                case SkillEffectType.DefBuff:
                case SkillEffectType.CriBuff:
                case SkillEffectType.TotalBuff:
                    TooltipExtra.text = $"요구 레벨:{_Data.RequireLevel}\n요구 스킬포인트  :{_Data.RequireSP}\n재사용 대기시간  :{_Data.Cooldown}\n스탯 증가량 :{effectDesc}";
                    break;
                case SkillEffectType.Debuff:
                    TooltipExtra.text = $"요구 레벨:{_Data.RequireLevel}\n요구 스킬포인트  :{_Data.RequireSP}\n재사용 대기시간  :{_Data.Cooldown}\n스탯 감소량 :{effectDesc}";
                    break;
            }
        }
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
                case SkillEffectType.RayDamage:
                case SkillEffectType.DistanceAreaDamage:
                case SkillEffectType.LineAreaDamage:
                case SkillEffectType.TargetAreaDamage:
                    Description += $"대상을 공격하여 {Effect.Power} 데미지를 입힙니다.\n";
                    break;
                case SkillEffectType.Heal:
                    Description += $"대상을 회복하여 {Effect.Power} 체력을 회복합니다.\n";
                    break;
                case SkillEffectType.AtkBuff:
                    Description += $"능력을 {Effect.Power} 만큼 공격력을 강화하는 버프를 적용합니다.\n";
                    break;
                case SkillEffectType.DefBuff:
                    Description += $"능력을 {Effect.Power} 만큼 방어력을 강화하는 버프를 적용합니다.\n";
                    break;
                case SkillEffectType.CriBuff:
                    Description += $"능력을 {Effect.Power} 만큼 크리티컬 확률을 강화하는 버프를 적용합니다.\n";
                    break;
                case SkillEffectType.TotalBuff:
                    Description += $"능력을 {Effect.Power} 만큼 전체적인 스탯을 강화하는 버프를 적용합니다.\n";
                    break;
                case SkillEffectType.Debuff:
                    Description += $"적에게 {Effect.Power} 만큼 약화 효과를 {Effect.Duration}초 동안 적용합니다.\n";
                    break;
                case SkillEffectType.CC:
                    Description += $"적에게 상태이상 효과 {Effect.Duration}초 동안 적용합니다.\n";
                    break;
                case SkillEffectType.Resource:
                    Description += $"자원을 {Effect.Power}을 만큼 회복합니다.\n";
                    break;
                case SkillEffectType.Teleport:
                    Description += $"{Effect.Distance}만큼 거리를 이동합니다.\n";
                    break;
            }
        }

        return Description.TrimEnd();
    }
}
