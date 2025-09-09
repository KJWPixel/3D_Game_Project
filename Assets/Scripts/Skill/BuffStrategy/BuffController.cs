using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    //버프 인스턴스를 List로 선언
    private List<BuffInstance> ActiveBuffs = new List<BuffInstance>();

    [SerializeField] PlayerStat PlayerStat;
    [SerializeField] UI_BuffIcon BuffIcon;
    [SerializeField] GameObject BuffIconPrefab;
    [SerializeField] Transform BuffIconParent;


    private void Awake()
    {
        
    }

    public void AddBuff(IBuffBehavoprStrategy strategy, float power, float duration, SkillData skillData = null)
    {
        // 같은 전략 + 스킬 데이터(같은 버프 스킬) 중복 검사
        BuffInstance existing = ActiveBuffs.FirstOrDefault(Buffs => Buffs.IBuff.GetType() == strategy.GetType()&& Buffs.SkillData == skillData);
        //Buffs = ActiveBuffs.BuffInstance => GetType으로 StrategyType && SkillData가 같은지 확인 
        //ActiveBuffs에서 지금 추가하려는 버프(strategy, skillData)와 같은 타입 + 같은 스킬로 걸린 기존 버프가 있는지 찾음
        //있으면 그 버프를 existing 변수에 저장 없으면 null 
        //existing 안에 같은 버프가 존재한다면 지속시간 갱신
        //FirstOrDefault(리스트를 순회하면서 조건에 만족하면 첫번째 요소를 변환, 불조건이면 null)

        if (existing != null)
        {
            existing.RefreshDuration(duration);
            return;
        }

        // 신규 버프 생성 및 적용
        BuffInstance buff = new BuffInstance(strategy, power, duration, skillData);
        ActiveBuffs.Add(buff);

        GameObject go = Instantiate(BuffIconPrefab, BuffIconParent);
        UI_BuffIcon BuffIcon = go.GetComponent<UI_BuffIcon>();
        BuffIcon.Setup(buff);




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