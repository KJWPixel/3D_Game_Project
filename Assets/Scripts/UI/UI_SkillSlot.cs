using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour
{
    [SerializeField] public SkillData SkillData;

    private Image CurrentImage;

    private void Awake()
    {
        CurrentImage = GetComponent<Image>();
    }

    private void Update()
    {
        SetIcon();
    }

    private void SetIcon()
    {
        if (SkillData != null)
        {
            CurrentImage.sprite = SkillData.Icon;
        }     
    }
}
