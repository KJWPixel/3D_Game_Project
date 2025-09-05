using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    private List<BuffInstance> activeBuffs = new List<BuffInstance>();
    private PlayerStat playerStat;

    private void Awake()
    {
        playerStat = GetComponent<PlayerStat>();
    }

    public void AddBuff(IBuffBehavoprStrategy strategy, float power, float duration, SkillData skillData = null)
    {
        // 같은 전략 + 스킬 데이터(같은 버프 스킬) 중복 검사
        BuffInstance existing = activeBuffs
            .FirstOrDefault(b => b.IBuff.GetType() == strategy.GetType()
                              && b.SkillData == skillData);

        if (existing != null)
        {
            existing.RefreshDuration(duration);
            return;
        }

        // 신규 버프 생성 및 적용
        BuffInstance buff = new BuffInstance(strategy, power, duration, skillData);
        activeBuffs.Add(buff);

        if (strategy.TargetType == BuffTargetType.Stat)
            strategy.Apply(playerStat, skillData, power);
        else if (strategy.TargetType == BuffTargetType.Skill && skillData != null)
            strategy.Apply(playerStat, skillData, power);
    }

    private void Update()
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            if (activeBuffs[i].IsExpired)
            {
                var buff = activeBuffs[i];
                if (buff.IBuff.TargetType == BuffTargetType.Stat)
                    buff.IBuff.Remove(playerStat, buff.SkillData, buff.Power);
                else if (buff.IBuff.TargetType == BuffTargetType.Skill && buff.SkillData != null)
                    buff.IBuff.Remove(playerStat, buff.SkillData, buff.Power);

                activeBuffs.RemoveAt(i);
            }
        }
    }
}