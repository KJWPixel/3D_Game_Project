using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillBehavior
{
    void Execute(PlayerController _Player, SkillData _SkillData);
}
