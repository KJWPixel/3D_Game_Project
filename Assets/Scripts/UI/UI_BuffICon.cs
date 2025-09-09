using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuffIcon : MonoBehaviour
{
    [SerializeField] GameObject BackGround;
    [SerializeField] Image Icon;
    [SerializeField] TMP_Text DurationText;
    private BuffInstance Buff;

    public void Setup(BuffInstance _Buff)
    {
        Buff = _Buff;
        Icon.sprite = Buff.SkillData.Icon;
    }

    void Update()
    {

    }
}
