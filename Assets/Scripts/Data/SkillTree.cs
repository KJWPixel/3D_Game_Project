using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    //��ų�� ���� ���� �˻� �� ���濩�� Ȯ��
    //��ųŬ�� > �ش� ��ų�� SP ���� ���� Ȯ��, Level Ȯ�� > ������ �����ϸ� SP ���� > BackGround��ȭ(���濬��)

    [SerializeField] PlayerStat PlayerStat;
    [SerializeField] PlayerSkillBook PlayerSkillBook; 

    public bool LearnSkill(SkillData _Skill)
    {
        //�̹� ��� ����
        int CurrentLevel = PlayerSkillBook.GetSkillLevel(_Skill);

        //�ִ� ���� üũ
        if(CurrentLevel >= _Skill.MaxLevel)
        {
            Debug.Log($"{_Skill.SkillName} �̹� �ִ� �����Դϴ�.");
            return false;
        }

        //���� ��ų üũ
        foreach(var PreSkill in _Skill.PrerequisiteSkills)
        {
            if(!PlayerSkillBook.HasSkill(PreSkill))
            {
                Debug.Log($"{PreSkill.SkillName}�� ���� ����� �մϴ�.");
                return false;
            }
        }

        //���� ���� üũ
        if(PlayerStat.Level < _Skill.RequireLevel)
        {
            Debug.Log("�÷��̾� ������ �����մϴ�.");
        }

        //SP ���� üũ
        if(!PlayerStat.ConsumeSp(_Skill.RequireSP))
        {
            Debug.Log("SP�� �����մϴ�.");
            return false;
        }

        //���� ó��
        PlayerSkillBook.LearnSkill(_Skill);
        Debug.Log($"{_Skill.SkillName} ����Ϸ�! (���� Lv.{PlayerSkillBook.GetSkillLevel(_Skill)})");
        return true;
    }
}
