using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefBuffStrategy : IBuffBehavoprStrategy
{
    public BuffTargetType TargetType => BuffTargetType.Stat;

    public void ApplyBuff(PlayerStat playerStat, SkillData skillData)
    {
        foreach (var Effect in skillData.Effects)
        {
            playerStat.Def += Effect.Power;
            Debug.Log($"공격력 버프 적용");
        }
        EffectManager.Instance.Spawn(skillData.CastEffectPrefab, playerStat.transform.position, skillData.CastPrefabDuration);
    }

    public void RemoveBuff(PlayerStat playerStat, SkillData skillData)
    {
        foreach(var Effect in skillData.Effects)
        {
            playerStat.Def -= Effect.Power;
            Debug.Log("방어력 버프 해제");
        }
    }
}
