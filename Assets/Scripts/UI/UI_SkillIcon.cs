using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //스킬창 아이콘에 부착하여 마우스 커서에 닿았을 시 스킬툴팁 활성화
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
