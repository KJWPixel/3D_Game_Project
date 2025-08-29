using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAreaDamageStrategy : ISkillBehaviorStrategy
{
    public void Execute(PlayerController _Player, SkillData _SkillData, Transform _Target)
    {
        foreach (var Effect in _SkillData.Effects)
        {
            Vector3 Origin = _Player.transform.position + Vector3.up * 1.0f;
            Vector3 Dir = _Player.transform.forward;

            // ����׿� ���� (�� �信�� Ȯ�� ����)
            Debug.DrawRay(Origin, Dir * Effect.Distance, Color.red, 5f);
            Debug.Log($"{_SkillData.name} Ray Test On");
        }
    }
}
