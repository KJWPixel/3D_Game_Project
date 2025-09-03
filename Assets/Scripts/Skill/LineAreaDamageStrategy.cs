using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineAreaDamageStrategy : ISkillBehaviorStrategy
{
    public void Execute(PlayerController _Player, SkillData _SkillData, Transform _Target)
    {
        Collider[] colliders;

        foreach (var Effect in _SkillData.Effects)
        {
            Vector3 Forward = _Player.transform.forward * 3f;
            Vector3 Origin = _Player.transform.position + Forward + Vector3.up;
            Vector3 Size = Vector3.one * Effect.Radius;

            Quaternion Rotation = Quaternion.LookRotation(Forward);

            EffectManager.Instance.Spawn(_SkillData.CastEffectPrefab, Origin, Rotation);

            colliders = Physics.OverlapBox(Origin, Size, Rotation);

            //딜레이타임을 넣고 코루틴 시작 
            //foreach (var col in colliders)
            //{
            //    if (col.CompareTag("Enemy"))
            //    {
            //        Enemy enemy = col.GetComponent<Enemy>();
            //        for (int i = 0; i < 5; i++)
            //        {
            //            enemy.TakeDamage(Effect.Power);
            //        }

            //        EffectManager.Instance.Spawn(_SkillData.HitEffectPrefab, enemy.transform.position);
            //    }
            //}

            _Player.StartCoroutine(DealDamageOverTime(colliders, Effect.Power, Effect.HitCount, Effect.DelayTime, _SkillData));
        }

        IEnumerator DealDamageOverTime(Collider[] _Coliiders, float _Power, int _HitCount, float _Delay, SkillData _SkillData)
        {
            for(int i = 0; i < _HitCount; i++)
            {
                foreach (var col in _Coliiders)
                {
                    if(col.CompareTag("Enemy"))
                    {
                        Enemy enemy = col.GetComponent<Enemy>();
                        if(enemy != null)
                        {
                            enemy.TakeDamage(_Power);
                            EffectManager.Instance.Spawn(_SkillData.HitEffectPrefab, enemy.transform.position);
                        }
                    }
                }
                yield return new WaitForSeconds(_Delay);    
            }
        }
    }
}
