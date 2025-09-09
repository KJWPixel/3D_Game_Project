using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    //���� �ν��Ͻ��� List�� ����
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
        // ���� ���� + ��ų ������(���� ���� ��ų) �ߺ� �˻�
        BuffInstance existing = ActiveBuffs.FirstOrDefault(Buffs => Buffs.IBuff.GetType() == strategy.GetType()&& Buffs.SkillData == skillData);
        //Buffs = ActiveBuffs.BuffInstance => GetType���� StrategyType && SkillData�� ������ Ȯ�� 
        //ActiveBuffs���� ���� �߰��Ϸ��� ����(strategy, skillData)�� ���� Ÿ�� + ���� ��ų�� �ɸ� ���� ������ �ִ��� ã��
        //������ �� ������ existing ������ ���� ������ null 
        //existing �ȿ� ���� ������ �����Ѵٸ� ���ӽð� ����
        //FirstOrDefault(����Ʈ�� ��ȸ�ϸ鼭 ���ǿ� �����ϸ� ù��° ��Ҹ� ��ȯ, �������̸� null)

        if (existing != null)
        {
            existing.RefreshDuration(duration);
            return;
        }

        // �ű� ���� ���� �� ����
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