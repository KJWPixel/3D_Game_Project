using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkBuffStrategy : IBuffBehavoprStrategy
{
    public BuffTargetType TargetType => BuffTargetType.Stat;
    public void ApplyBuff(PlayerStat playrStat, SkillData skillData)
    {
        foreach (var Effect in skillData.Effects)
        {
            playrStat.Atk += Effect.Power;
            Debug.Log($"공격력 버프 적용");
        }
        EffectManager.Instance.Spawn(skillData.CastEffectPrefab, playrStat.transform.position, skillData.CastPrefabDuration);
    }

    public void RemoveBuff(PlayerStat playrStat, SkillData skillData)
    {
        foreach (var Effect in skillData.Effects)
        {
            playrStat.Atk -= Effect.Power;
            Debug.Log($"공격력 버프 해제");
        }  
    }

 
}
