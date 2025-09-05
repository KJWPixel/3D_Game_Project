using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffBehavoprStrategy
{
    BuffTargetType TargetType { get; }
    void ApplyBuff(PlayerStat _PlayerStat, SkillData _SkillData, float _Power);
    void RemoveBuff(PlayerStat _PlayerStat, SkillData _SkillData, float _Power);   
}
