using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 습득한 스킬 저장
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
            //해당 스킬이 배운스킬이 아니라면 배운스킬 리스트에 넣고 해당 스킬의 Level은 1
            LearnedSkills.Add(_Skill);
            SkillLevels[_Skill] = 1;
        }
        else
        {
            //해당 스킬이 배운스킬이라면 스킬레벨 업
            SkillLevels[_Skill]++;
        }
    }
    
}
