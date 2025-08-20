using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    //스킬의 습득 조건 검사 및 습득여부 확인
    //스킬클릭 > 해당 스킬의 SP 차감 여부 확인, Level 확인 > 습득이 가능하면 SP 차감 > BackGround변화(습득연출)
    
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
        //습득 조건을 확인
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
