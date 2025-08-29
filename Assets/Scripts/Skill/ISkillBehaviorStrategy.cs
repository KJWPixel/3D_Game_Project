using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillBehaviorStrategy
{
    void Execute(PlayerController _Player, SkillData _SkillData, Transform _Target);
}
