using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBuffStrategy : IBuffBehavoprStrategy
{

    public void ApplyBuff(PlayerStat _PlayerStat, SkillData _SkillData, Transform _Target)
    {
        foreach (var Effect in _SkillData.Effects)
        {
            if (_PlayerStat != null)
            {
                _PlayerStat.StartCoroutine(HealOverTime(Effect.Power, Effect.HitCount, Effect.Duration));
            }
        }

        IEnumerator HealOverTime(float _Power, float _HitCount, float _Duration)
        {
            //몇초 마다, 몇번을, 회복
            for (int i = 0; i < _Duration; i++)
            {
                _PlayerStat.Heal(_Power);
                yield return new WaitForSeconds(_HitCount);//1초 마다
            }          
        }
    }

    public void RemoveBuff(PlayerStat _PlayrStat, SkillData _SkillData, Transform _Target)
    {
    }

    public BuffTargetType TargetType => throw new System.NotImplementedException();
}
