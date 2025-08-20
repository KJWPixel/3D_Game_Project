using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //��ųâ �����ܿ� �����Ͽ� ���콺 Ŀ���� ����� �� ��ų���� Ȱ��ȭ
    [SerializeField] public SkillData SkillData;
    [SerializeField] private Image IconImage;

    private void Start()
    {
        if (SkillData != null && IconImage != null)
        {
            IconImage.sprite = SkillData.Icon;
        }         
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
}
