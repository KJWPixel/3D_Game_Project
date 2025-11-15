using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBuffStrategy : IBuffBehavoprStrategy
{
    public BuffTargetType TargetType => BuffTargetType.Stat;

    public void ApplyBuff(PlayerStat playerStat, SkillData skillData)
    {
        if (playerStat == null) return; 

        foreach (var Effect in skillData.Effects)
        {
            Debug.Log("Healing ApplyBuff foreach 호출");

            playerStat.StartCoroutine(HealOverTime(Effect.EffectType, Effect.Power, Effect.HitCount, Effect.Duration));
        }

        IEnumerator HealOverTime(SkillEffectType type, float power, float hitCount, float duration)
        {
            for (int i = 0; i < duration; i++)
            {
                switch (type)
                {
                    case SkillEffectType.HealBuff:
                        playerStat.RecoveryStat(ConsumableType.ResotreHp, power);
                        break;
                    case SkillEffectType.MpBuff:
                        playerStat.RecoveryStat(ConsumableType.ResotreMp, power);
                        break;
                    default:
                        Debug.Log($"지정되지 않은 타입 {type}");
                        break;

                }
                yield return new WaitForSeconds(hitCount);//1초 마다
            }
        }

        EffectManager.Instance.Spawn(skillData.CastEffectPrefab, playerStat.transform.position, skillData.CastPrefabDuration);
    }


    public void RemoveBuff(PlayerStat _PlayrStat, SkillData _SkillData)
    {
        //Remove필요없음
    }
}
