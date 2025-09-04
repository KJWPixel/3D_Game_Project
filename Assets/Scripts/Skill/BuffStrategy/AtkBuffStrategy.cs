using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkBuffStrategy : IBuffBehavoprStrategy
{
    public void ApplyBuff(PlayerStat _PlayrStat, SkillData _SkillData, Transform _Target)
    {
        foreach (var Effect in _SkillData.Effects)
        {
            _PlayrStat.Atk += Effect.Power;
            Debug.Log($"���ݷ� ���� ����");
        }
    }

    public void RemoveBuff(PlayerStat _PlayrStat, SkillData _SkillData, Transform _Target)
    {
        foreach (var Effect in _SkillData.Effects)
        {
            _PlayrStat.Atk -= Effect.Power;
            Debug.Log($"���ݷ� ���� ����");
        }  
    }
}
