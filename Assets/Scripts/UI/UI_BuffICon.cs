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
            //�������۽ð� + ���ӽð� - �귯�� �ð�
            float Remaining = LinkBuff.StartTime + LinkBuff.Duration - Time.time;
            //float RemainingSeconds = LinkBuff.StartTime + (LinkBuff.Duration * 60) - Time.time;

            if (Remaining <= 0f)//�귯�� �ð��� �� ũ�ٸ� �׷����� Remaining ���� �����̸�
            {
                Destroy(gameObject);
                return;
            }

            //�ʴ��� ǥ��
            //BuffTimeText.text = Mathf.Ceil(Remaining).ToString();

            //�д��� ǥ��
            int Minutes = Mathf.FloorToInt(Remaining / 60f);
            int Seconds = Mathf.FloorToInt(Remaining % 60f);
            BuffTimeText.text = $"{Minutes:D2}:{Seconds:D2}";
            


        }
    }
}
