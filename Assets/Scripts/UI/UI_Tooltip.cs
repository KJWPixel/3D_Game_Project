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
        TooltipPanel.SetActive(true);
        TooltipPanel.transform.position = _Position + new Vector3(0, 0, 0); 
        TooltipIcon.sprite = _Data.Icon;

        TooltipName.text = $"{_Data.SkillName}";
        TooltipDesc.text = GetDescriptionByType(_Data);

        string EffectsDesc = GetDescriptionByType(_Data);

        TooltipExtra.text = $"�䱸 ����:{_Data.RequireLevel}\n�䱸 ��ų����Ʈ  :{_Data.RequireSP}\n���� ���ð�  :{_Data.Cooldown}\n��ų������ :{EffectsDesc}";
    }

    public void HideTooltip()
    {
        TooltipPanel.SetActive(false);
    }

    private string GetDescriptionByType(SkillData _SkillData)
    {
        if(_SkillData == null || _SkillData.Effects.Count == 0)
        {
            return "�������";
        }

        string Description = "";

        foreach(var Effect in _SkillData.Effects)
        {
            switch (Effect.EffectType)
            {
                case SkillEffectType.Damage:
                    Description += $"����� �����Ͽ� {Effect.Power} �������� �����ϴ�.\n";
                    break;
                case SkillEffectType.Heal:
                    Description += $"����� ȸ���Ͽ� {Effect.Power} ü���� ȸ���մϴ�.\n";
                    break;
                case SkillEffectType.AtkBuff:
                    Description += $"�ɷ��� {Effect.Power} ��ŭ ���ݷ��� ��ȭ�ϴ� ������ �����մϴ�.\n";
                    break;
                case SkillEffectType.DefBuff:
                    Description += $"�ɷ��� {Effect.Power} ��ŭ ������ ��ȭ�ϴ� ������ �����մϴ�.\n";
                    break;
                case SkillEffectType.CriBuff:
                    Description += $"�ɷ��� {Effect.Power} ��ŭ ũ��Ƽ�� Ȯ���� ��ȭ�ϴ� ������ �����մϴ�.\n";
                    break;
                case SkillEffectType.TotalBuff:
                    Description += $"�ɷ��� {Effect.Power} ��ŭ ��ü���� ������ ��ȭ�ϴ� ������ �����մϴ�.\n";
                    break;
                case SkillEffectType.Debuff:
                    Description += $"������ {Effect.Power} ��ŭ ��ȭ ȿ���� {Effect.Duration}�� ���� �����մϴ�.\n";
                    break;
                case SkillEffectType.CC:
                    Description += $"������ �����̻� ȿ�� {Effect.Duration}�� ���� �����մϴ�.\n";
                    break;
                case SkillEffectType.Resource:
                    Description += $"�ڿ��� {Effect.Power}�� ��ŭ ȸ���մϴ�.\n";
                    break;
            }
        }

        return Description.TrimEnd();
    }
}
