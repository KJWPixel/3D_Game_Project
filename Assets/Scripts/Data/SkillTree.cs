using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    //스킬의 습득 조건 검사 및 습득여부 확인
    //스킬클릭 > 해당 스킬의 SP 차감 여부 확인, Level 확인 > 습득이 가능하면 SP 차감 > BackGround변화(습득연출)

    [SerializeField] private PlayerStat PlayerStat;
    [SerializeField] private PlayerSkillBook PlayerSkillBook;

    private void OnDisable()
    {
        if(UI_Tooltip.Instance != null)
        {
            UI_Tooltip.Instance.HideTooltip();
        }
    }

    public bool LearnSkill(SkillData _Skill)
    {
        //이미 배운 레벨
        int CurrentLevel = PlayerSkillBook.GetSkillLevel(_Skill);

        //최대 레벨 체크
        if(CurrentLevel >= _Skill.MaxLevel)
        {
            Debug.Log($"{_Skill.SkillName} 이미 최대 레벨입니다.");
            return false;
        }

        //선행 스킬 체크
        foreach(var PreSkill in _Skill.PrerequisiteSkills)
        {
            if(!PlayerSkillBook.HasSkill(PreSkill))
            {
                Debug.Log($"{PreSkill.SkillName}를 먼저 배워야 합니다.");
                return false;
            }
        }

        //레벨 조건 체크
        if(PlayerStat.Level < _Skill.RequireLevel)
        {
            Debug.Log("플레이어 레벨이 부족합니다.");
            return false;
        }

        //SP 조건 체크
        if(!PlayerStat.ConsumeSp(_Skill.RequireSP))
        {
            Debug.Log("SP가 부족합니다.");
            return false;
        }

        //습득 처리
        PlayerSkillBook.LearnSkill(_Skill);
        Debug.Log($"{_Skill.SkillName} 습득완료! (현재 Lv.{PlayerSkillBook.GetSkillLevel(_Skill)})");
        return true;
    }
}
