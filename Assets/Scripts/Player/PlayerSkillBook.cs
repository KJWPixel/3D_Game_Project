using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾ ������ ��ų ����
public class PlayerSkillBook : MonoBehaviour
{
    public List<SkillData> LearnedSkills = new List<SkillData>();
    private Dictionary<SkillData, int> SkillLevels = new Dictionary<SkillData, int>();

    public bool HasSkill(SkillData _Skill)
    {
        return LearnedSkills.Contains(_Skill);
    }

    public int GetSkillLevel(SkillData _Skill)
    {
        if(SkillLevels.TryGetValue(_Skill, out int Level))
        {
            return Level;
        }

        return 0;
    }

    public void LearnSkill(SkillData _Skill)
    {
        if(!LearnedSkills.Contains(_Skill))
        {
            //�ش� ��ų�� ��ų�� �ƴ϶�� ��ų ����Ʈ�� �ְ� �ش� ��ų�� Level�� 1
            LearnedSkills.Add(_Skill);
            SkillLevels[_Skill] = 1;
        }
        else
        {
            //�ش� ��ų�� ��ų�̶�� ��ų���� ��
            SkillLevels[_Skill]++;
        }
    }
    
}
