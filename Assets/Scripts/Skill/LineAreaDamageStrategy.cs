using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LineAreaDamageStrategy : ISkillBehaviorStrategy
{
    public void Execute(PlayerController _Player, SkillData _SkillData, Transform _Target)
    {
        Collider[] colliders;

        foreach (var Effect in _SkillData.Effects)
        {
            Vector3 Forward = Vector3.forward * 3f;
            Vector3 Origin = _Player.transform.position + Forward + Vector3.up;
            Vector3 Size = Vector3.one * Effect.Radius;

            Quaternion Rotation = Quaternion.LookRotation(Forward);

            EffectManager.Instance.Spawn(_SkillData.CastEffectPrefab, Origin, Rotation);

            colliders = Physics.OverlapBox(Forward, Size, Rotation);
            foreach(var col in colliders)
            {
                if(col.CompareTag("Enemy"))
                {
                    Enemy enemy = col.GetComponent<Enemy>();
                    enemy.TakeDamage(Effect.Power);

                    EffectManager.Instance.Spawn(_SkillData.HitEffectPrefab, enemy.transform.position);
                }
            }
            
        }
    }



    

    
}
