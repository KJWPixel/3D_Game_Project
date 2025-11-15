using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffBehavoprStrategy
{
    BuffTargetType TargetType { get; }
    void ApplyBuff(PlayerStat playerStat, SkillData skillData);
    void RemoveBuff(PlayerStat playerStat, SkillData skillData);   
}
