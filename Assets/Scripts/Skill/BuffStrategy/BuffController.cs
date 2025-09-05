using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    //버프 인스턴스를 List로 선언
    private List<BuffInstance> ActiveBuffs = new List<BuffInstance>();
    [SerializeField] PlayerStat PlayerStat;

    private void Awake()
    {
        Debug.Log($"[BuffController] Awake on: {gameObject.name}, PlayerStat: {PlayerStat}");
    }

    public void AddBuff(IBuffBehavoprStrategy strategy, float power, float duration, SkillData skillData = null)
    {
        if (PlayerStat == null)
        {
            Debug.LogError("[BuffController] PlayerStat is null!");
        }
        else
        {
            Debug.Log("[BuffController] PlayerStat is OK: " + PlayerStat.gameObject.name);
        }

        // 같은 전략 + 스킬 데이터(같은 버프 스킬) 중복 검사
        BuffInstance existing = ActiveBuffs.FirstOrDefault(b => b.IBuff.GetType() == strategy.GetType()&& b.SkillData == skillData);

        if (existing != null)
        {
            existing.RefreshDuration(duration);
            return;
        }

        // 신규 버프 생성 및 적용
        BuffInstance buff = new BuffInstance(strategy, power, duration, skillData);
        ActiveBuffs.Add(buff);

        if (strategy.TargetType == BuffTargetType.Stat)
        {
            strategy.ApplyBuff(PlayerStat, skillData, power);
        }           
        else if (strategy.TargetType == BuffTargetType.Skill && skillData != null)
        {
            strategy.ApplyBuff(PlayerStat, skillData, power);
        }
            
    }

    private void Update()
    {
        for (int i = ActiveBuffs.Count - 1; i >= 0; i--)
        {
            if (ActiveBuffs[i].IsExpired)
            {
                var buff = ActiveBuffs[i];
                if (buff.IBuff.TargetType == BuffTargetType.Stat)
                    buff.IBuff.RemoveBuff(PlayerStat, buff.SkillData, buff.Power);
                else if (buff.IBuff.TargetType == BuffTargetType.Skill && buff.SkillData != null)
                    buff.IBuff.RemoveBuff(PlayerStat, buff.SkillData, buff.Power);

                ActiveBuffs.RemoveAt(i);
            }
        }
    }
}