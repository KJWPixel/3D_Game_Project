using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour
{
    [SerializeField] public SkillData SkillData;
    [SerializeField] Image BackGround;
    [SerializeField] Image CurrentImage;

    private void Awake()
    {

    }

    private void Update()
    {
        IconCoolTimeUpdate();
    }

    public void SetIcon(SkillData _SkillData)
    {
        SkillData = _SkillData;
        if(SkillData != null)
        {
            CurrentImage.sprite = SkillData.Icon;
            BackGround.sprite = SkillData.Icon;
            BackGround.color = new Color(0.5f, 0.5f, 0.5f, 0.5f); 
        }
        else
        {
            if (CurrentImage != null)
            {
                CurrentImage.sprite = null;
            }
            if (BackGround != null)
            {
                BackGround.sprite = null;
            }            
        }
    }

    private void IconCoolTimeUpdate()
    {
        //��ų�Ŵ������� ��ų ��� �� Dictionary �ڷ����� ��� �ش� ��ų�� Cooldown�� �����ͼ� ��Ÿ���� ���ư��� Effect
        if(SkillData == null || CurrentImage == null)
        {
            return;
        }

        float CoolTime = 0f;
        if(SkillManager.Instance.SkillCoolDownTimers.ContainsKey(SkillData))
        {
            CoolTime = SkillManager.Instance.SkillCoolDownTimers[SkillData] - Time.time; //�ش� ��ų ��Ÿ�� ����
        }

        if(CoolTime >= 0)
        {
            CurrentImage.fillAmount = 1f - (CoolTime / SkillData.Cooldown);
        }
        else
        {
            CurrentImage.fillAmount = 1f;
        }
    }
}
