using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryStrategy : ISkillBehaviorStrategy
{
    public void Execute(PlayerController _Player, PlayerStat _PlayerStat, SkillData _SkillData, Transform _Target)
    {
        foreach(var Effect in _SkillData.Effects)
        {
            PlayerStat PlayerStat = _Player.GetComponent<PlayerStat>();
            PlayerStat.CurrentHp = Mathf.Min(PlayerStat.CurrentHp + Effect.Power, PlayerStat.MaxHp);
        }
    }
}
