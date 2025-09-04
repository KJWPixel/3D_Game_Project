using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class LineAreaDamageStrategy : ISkillBehaviorStrategy
{
    public void Execute(PlayerController _Player, PlayerStat _PlayerStat, SkillData _SkillData, Transform _Target)
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

            List<Collider> EnemyList = new List<Collider>();

            foreach (var col in colliders)
            {
                //Overlap에서 감지된 colliders를 col로 담음
                if(col.CompareTag("Enemy"))
                {
                    EnemyList.Add(col);
                }           
            }
            //람다식을 이용한 비교(Player위치와 비교)
            EnemyList.Sort((a, b) => Vector3.Distance(_Player.transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(_Player.transform.position, b.transform.position)));

            int Count = Mathf.Min(Effect.MaxTarget, EnemyList.Count);

            if(Count >= 0)
            {
                Collider[] LimitTarget = EnemyList.GetRange(0, Count).ToArray();
                _Player.StartCoroutine(DealDamageOverTime(LimitTarget, Effect.Power, Effect.HitCount, Effect.DelayTime, _SkillData));
            }            
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
