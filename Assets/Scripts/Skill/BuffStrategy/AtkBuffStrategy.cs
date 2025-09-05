using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkBuffStrategy : IBuffBehavoprStrategy
{
    public BuffTargetType TargetType => BuffTargetType.Stat;
    public void ApplyBuff(PlayerStat _PlayrStat, SkillData _SkillData, float _Power)
    {
        foreach (var Effect in _SkillData.Effects)
        {
            _PlayrStat.Atk += Effect.Power;
            Debug.Log($"���ݷ� ���� ����");
        }
        EffectManager.Instance.Spawn(_SkillData.CastEffectPrefab, _PlayrStat.transform.position, _SkillData.PrefabDuration);
    }

    public void RemoveBuff(PlayerStat _PlayrStat, SkillData _SkillData, float _Power)
    {
        foreach (var Effect in _SkillData.Effects)
        {
            _PlayrStat.Atk -= Effect.Power;
            Debug.Log($"���ݷ� ���� ����");
        }  
    }

 
}
