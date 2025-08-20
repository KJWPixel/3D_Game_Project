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
    [SerializeField] private TMP_Text TooltipExtra; // ��Ÿ��, �Ҹ��ڿ� ��
    [SerializeField] private Image TooltipIcon;

    private void Awake()
    {
        Instance = this;
        TooltipPanel.SetActive(false);
    }

    public void ShowTooltip(SkillData _Data, Vector3 _Position)
    {
        TooltipPanel.SetActive(true);
        TooltipPanel.transform.position = _Position + new Vector3(0, -350, 0); // ������ ������
        TooltipIcon.sprite = _Data.Icon;

        TooltipName.text = $"{_Data.SkillName}";
        TooltipDesc.text = GetDescriptionByType(_Data);

        TooltipExtra.text =
            $"��Ÿ��: {_Data.Cooldown:F1}s\n" +
            $"�Ҹ�: {_Data.Cost}\n" +
            $"��Ÿ�: {_Data.Range}\n" +
            $"�����ð�: {_Data.CastTime}s\n" +
            $"����: {_Data.Power}";
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
