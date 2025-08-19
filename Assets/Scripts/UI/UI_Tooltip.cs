using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Tooltip : MonoBehaviour
{
    [SerializeField] Image Icon;
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] GameObject SkillToopTip;

    private void Awake()
    {
        
    }

    private void Update()
    {
        
    }

    private void SkillToolTip(SkillData _Skill, Vector2 _Position)
    {
       
    }

    private void HideToolTip()
    {
        SkillToopTip.SetActive(false);
    }
}
