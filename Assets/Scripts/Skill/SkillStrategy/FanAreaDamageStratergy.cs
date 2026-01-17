using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class FanAreaDamageStratergy : ISkillBehaviorStrategy
{
    public void Execute(PlayerController player, PlayerStat stat, SkillData data, Transform target)
    {
        Vector3 origin = player.transform.position;
        Vector3 forward = player.transform.forward;
        Quaternion rotation = Quaternion.LookRotation(forward);

        foreach (var effect in data.Effects)
        {
            float radius = effect.Radius;
            float halfAndgle = effect.Angle * 0.5f;

            EffectManager.Instance.Spawn(data.CastEffectPrefab, origin, rotation, data.CastPrefabDuration);

            Collider[] hits = Physics.OverlapSphere(origin, radius);

            foreach(var col in hits)
            {
                if(!col.CompareTag("Enemy")) continue;

                Vector3 dir = col.transform.position - origin;
                float distance = dir.magnitude;

                if (distance > radius) continue;

                float angleToEnemy = Vector3.Angle(forward, dir);
                if(angleToEnemy > halfAndgle) continue;
                
                Enemy enemy = col.GetComponent<Enemy>();
                if(enemy != null)
                {
                    var result = PlayerStat.Instance.CalculateFianlDamage(effect.Power, enemy.Def);

                    // 적에게 최종 결과 전달
                    enemy.TakeDamage(result.damage, result.isCrit);
                }
            }
        }
    }
}
