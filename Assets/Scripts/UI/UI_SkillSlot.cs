using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour
{
    [SerializeField] SkillData SkillData;
    private Image CurrentImage;

    private void Awake()
    {
        CurrentImage = GetComponent<Image>();
    }


}
