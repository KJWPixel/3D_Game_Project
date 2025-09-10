using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuffIcon : MonoBehaviour
{
    [SerializeField] Image BackGround;
    [SerializeField] Image Icon;
    [SerializeField] TMP_Text BuffTimeText;

    private BuffInstance LinkBuff;

    public void Setup(BuffInstance _Buff)
    {
         LinkBuff = _Buff;
         Icon.sprite = LinkBuff.SkillData.Icon;
    }

    void Update()
    {
        if(LinkBuff != null)
        {
            //버프시작시간 + 지속시간 - 흘러간 시간
            float Remaining = LinkBuff.StartTime + LinkBuff.Duration - Time.time;
            //float RemainingSeconds = LinkBuff.StartTime + (LinkBuff.Duration * 60) - Time.time;

            if (Remaining <= 0f)//흘러간 시간이 더 크다면 그로인해 Remaining 값이 음수이면
            {
                Destroy(gameObject);
                return;
            }

            //초단위 표시
            //BuffTimeText.text = Mathf.Ceil(Remaining).ToString();

            //분단위 표시
            int Minutes = Mathf.FloorToInt(Remaining / 60f);
            int Seconds = Mathf.FloorToInt(Remaining % 60f);
            BuffTimeText.text = $"{Minutes:D2}:{Seconds:D2}";
            


        }
    }
}
