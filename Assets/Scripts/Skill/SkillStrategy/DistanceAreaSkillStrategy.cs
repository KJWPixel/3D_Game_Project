using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations.Rigging;
using UnityEngine;

public class DistanceAreaSkillStrategy : ISkillBehaviorStrategy
{
    public void Execute(PlayerController _Player, PlayerStat _PlayerStat, SkillData _SkillData, Transform _Target)
    {
        foreach(var Effect in _SkillData.Effects)
        {
            
        }
    }
}
