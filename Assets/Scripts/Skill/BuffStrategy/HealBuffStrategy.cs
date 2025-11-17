using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBuffStrategy : IBuffBehaviorStrategy
{
    public BuffTargetType TargetType => BuffTargetType.Stat;

    public void ApplyBuff(PlayerStat playerStat, SkillData skillData)
    {
        if (playerStat == null) return; 

        foreach (var Effect in skillData.Effects)
        {
            Debug.Log("Healing 코루틴 호출");

            playerStat.StartCoroutine(HealOverTime(Effect.EffectType, Effect.Power, Effect.HitCount, Effect.Duration));
        }     
        EffectManager.Instance.Spawn(skillData.CastEffectPrefab, playerStat.transform.position, skillData.CastPrefabDuration);
    }
    IEnumerator HealOverTime(SkillEffectType type, float power, int hitCount, float duration)
    {
        if (PlayerStat.Instance == null)
            yield break;
        if (power <= 0f)
        {
            Debug.LogWarning("HealOverTime: power <= 0, 중단");
            yield break;
        }
        if (hitCount <= 0f)
        {
            Debug.LogWarning("HealOverTime: hitCount <= 0, 중단");
            yield break;
        }
        if (duration <= 0f)
        {
            Debug.LogWarning("HealOverTime: duration <= 0, 중단");
            yield break;
        }

        //초당 hitCount번, duration초 동안 수행
        int hitsPerSecond = Mathf.Max(1, Mathf.RoundToInt(hitCount));
        float interval = 1f / hitsPerSecond;

        int totalTicks = Mathf.CeilToInt(duration * hitsPerSecond);

        bool debugLogEachTick = false;

        for (int tick = 0; tick < totalTicks; tick++)
        {
            // 플레이어 무효/사망 처리
            if (PlayerStat.Instance == null)
                yield break;

            
            switch (type)
            {
                case SkillEffectType.HealBuff:
                    PlayerStat.Instance.RecoveryStat(ConsumableType.ResotreHp, power);
                    if (debugLogEachTick) Debug.Log($"[Heal] +{power} HP (tick {tick + 1}/{totalTicks}) -> curHP:{PlayerStat.Instance.CurrentHp}/{PlayerStat.Instance.MaxHp}");
                    break;

                case SkillEffectType.MpBuff:
                    PlayerStat.Instance.RecoveryStat(ConsumableType.ResotreMp, power);
                    if (debugLogEachTick) Debug.Log($"[Heal] +{power} MP (tick {tick + 1}/{totalTicks}) -> curMP:{PlayerStat.Instance.CurrentMp}/{PlayerStat.Instance.MaxMp}");
                    break;

                default:
                    Debug.Log($"지정되지 않은 타입 {type}");
                    break;
            }

            // 마지막 tick이면 더 이상 대기하지 않음
            if (tick == totalTicks - 1)
                break;

            yield return new WaitForSeconds(interval);
        }
        yield break;

    }

    public void RemoveBuff(PlayerStat _PlayrStat, SkillData _SkillData)
    {
        //Remove필요없음
    }
}
