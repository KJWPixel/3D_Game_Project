using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefBuffStrategy : IBuffBehavoprStrategy
{
    public BuffTargetType TargetType => BuffTargetType.Stat;

    public void ApplyBuff(PlayerStat _PlayerStat, SkillData _SkillData, float _Power)
    {

    }

    public void RemoveBuff(PlayerStat _PlayerStat, SkillData _SkillData, float _Power)
    {

    }
}
