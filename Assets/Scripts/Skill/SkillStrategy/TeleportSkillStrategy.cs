using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSkillStrategy : ISkillBehaviorStrategy
{
    public void Execute(PlayerController _Player, PlayerStat _PlayerStat, SkillData _SkillData, Transform _Target)
    {
        var controller = _Player.GetComponent<CharacterController>();

        foreach (var Effect in _SkillData.Effects)
        {
            Vector3 Dir = _Player.transform.forward;
            Vector3 MoveDistance = _Player.transform.position + Dir * Effect.Distance;

            if (controller != null) controller.enabled = false;
            _Player.transform.position = MoveDistance;
            if (controller != null) controller.enabled = true;

            EffectManager.Instance.Spawn(_SkillData.CastEffectPrefab, _Player.transform.position, _SkillData.PrefabDuration);

            Debug.Log($"[Teleport] Player Move to {MoveDistance}");
        }
    }
}
