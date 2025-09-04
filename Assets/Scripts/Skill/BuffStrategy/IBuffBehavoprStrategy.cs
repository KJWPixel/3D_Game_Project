using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffBehavoprStrategy
{
    void ApplyBuff(PlayerStat _PlayerStat, SkillData _SkillData, Transform _Target);
    void RemoveBuff(PlayerStat _PlayerStat, SkillData _SkillData, Transform _Target);
}
