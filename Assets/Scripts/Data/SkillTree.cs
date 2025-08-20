using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    //��ų�� ���� ���� �˻� �� ���濩�� Ȯ��
    //��ųŬ�� > �ش� ��ų�� SP ���� ���� Ȯ��, Level Ȯ�� > ������ �����ϸ� SP ���� > BackGround��ȭ(���濬��)
    
    [SerializeField] List<SkillData> AllSkillData;
    private Dictionary<SkillData, string> SkillName = new Dictionary<SkillData, string>();
    private Dictionary<SkillData, int> RequireSkill = new Dictionary<SkillData, int>();

    PlayerStat PlayerStat;
    UI_Skill UI_Skill;

    private void Start()
    {
        PlayerStat = FindAnyObjectByType<PlayerStat>();
    }


    public void CanLearnSkill(SkillData _SkillData)
    {
        //���� ������ Ȯ��
        if (PlayerStat.Level < _SkillData.RequireLevel)
        {
            return;
        }       
        if (PlayerStat.SkillPoint < _SkillData.RequireSP)
        {
            return;
        }

        return;
    }


}
