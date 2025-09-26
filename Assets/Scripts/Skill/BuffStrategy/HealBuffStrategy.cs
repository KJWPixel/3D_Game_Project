using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBuffStrategy : IBuffBehavoprStrategy
{
    public BuffTargetType TargetType => BuffTargetType.Stat;

    public void ApplyBuff(PlayerStat _PlayerStat, SkillData _SkillData, float _Power)
    {
        foreach (var Effect in _SkillData.Effects)
        {
            Debug.Log("Healing ApplyBuff foreach ȣ��");
            if (_PlayerStat != null)
            {
                _PlayerStat.StartCoroutine(HealOverTime(Effect.Power, Effect.HitCount, Effect.Duration));
            }
        }

        IEnumerator HealOverTime(float _Power, float _HitCount, float _Duration)
        {
            //���� ����, �����, ȸ��
            for (int i = 0; i < _Duration; i++)
            {
                _PlayerStat.RecoveryStat(ConsumableType.ResotreHp, _Power);
                yield return new WaitForSeconds(_HitCount);//1�� ����
            }          
        }

        EffectManager.Instance.Spawn(_SkillData.CastEffectPrefab, _PlayerStat.transform.position, _SkillData.CastPrefabDuration);
    }

    public void RemoveBuff(PlayerStat _PlayrStat, SkillData _SkillData, float _Power)
    {
        //Remove�ʿ����
    }
}
