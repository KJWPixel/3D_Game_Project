using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Pipeline;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UI_Tooltip : MonoBehaviour
{
    public static UI_Tooltip Instance;

    private const string TableName = "SKILL Table";

    [SerializeField] private GameObject SkillTree;
    [SerializeField] private GameObject TooltipPanel;
    [SerializeField] private Image TooltipIcon;           //스킬이미지
    [SerializeField] private TMP_Text TooltipSkillName;   //스킬이름
    [SerializeField] private TMP_Text TooltipSkillDesc;   //스킬설명
    [SerializeField] private TMP_Text TooltipSkillRequirements;  //스킬요구치


    private void Awake()
    {
        Instance = this;
        TooltipPanel.SetActive(false);
    }

    public void ShowTooltip(SkillData data, Vector3 position)
    {
        if (TooltipPanel == null)
        {
            Debug.Log($"UI_Tooltip NULL {gameObject.name}");
            return;
        }

        // 스킬아이콘에 마우스커서가 들어오면 호출
        TooltipPanel.SetActive(true);
        // 기존 Text 초기화
        TooltipSkillName.text = "";
        TooltipSkillDesc.text = "";
        TooltipSkillRequirements.text = "";

        TooltipIcon.sprite = data.Icon;

        TooltipPanel.transform.position = position;

        object[] args = GetFormattedArgs(data);

        // UI_Tooltip 로컬라이제션: LocalizationTable에서 Table,Key값을 가져와 호출
        TooltipSkillName.text = LocalizationSettings.StringDatabase.GetLocalizedString(TableName, data.SkillKey); // 스킬이름

        TooltipSkillDesc.text = LocalizationSettings.StringDatabase.GetLocalizedString(TableName, data.SkillDescriptionKey, args);

        TooltipSkillRequirements.text = LocalizationSettings.StringDatabase.GetLocalizedString(TableName, data.SkillRequirementsKey, data.GetRequirementParams()); // 스킬요구치
    }

    private object[] GetFormattedArgs(SkillData data)
    {
        if (data.Effects == null || data.Effects.Count == 0) return null;
        var e = data.Effects[0];

        switch (data.SkillDescriptionKey)
        {
            case "Skill_SingleHit_Desc": // 단타
            case "Skill_Heal_Desc":      // 1회 회복기
                return new object[] { e.Power };

            case "Skill_MultiHit_Desc":
                return new object[] { e.Power, e.HitCount };

            case "Skill_HealBuff_Desc": //회복 버프
            case "Skill_MpBuff_Desc":
                return new object[] { e.Duration, e.HitCount, e.Power};

            case "Skill_AtkBuff_Desc":
            case "Skill_DefBuff_Desc":
            case "Skill_MovementBuff_Desc":
                return new object[] { e.Duration, e.Power };

            case "Skill_Teleport_Desc":
                return new object[] { e.Distance };

            default:
                return new object[] { e.Power };
        }
    }

    public void HideTooltip()
    {
        TooltipPanel.SetActive(false);
    }


}
