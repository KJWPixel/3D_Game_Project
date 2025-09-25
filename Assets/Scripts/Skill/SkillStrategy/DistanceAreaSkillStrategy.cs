using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Animations.Rigging;
using UnityEngine;

public class DistanceAreaSkillStrategy : ISkillBehaviorStrategy
{
    List<Enemy> EnemyList = new List<Enemy>();
    public void Execute(PlayerController _Player, PlayerStat _PlayerStat, SkillData _SkillData, Transform _Target)
    {
        foreach(var Effect in _SkillData.Effects)
        {
            //스킬시작위치
            Vector3 Forward = _Player.transform.forward;
            Vector3 Origin = _Player.transform.position + Forward * Effect.Distance + Vector3.up;
            Quaternion Rotation = Quaternion.LookRotation(Forward);

            float Radius = Effect.Radius;
            float RadiusSqr = Radius * Radius;

            //스킬동작 - Effect 생성
            EffectManager.Instance.Spawn(_SkillData.CastEffectPrefab, Origin, Rotation, _SkillData.CastPrefabDuration);

            //Enemy를 넣을 배열, 스킬범위 안 EnemyList               
            GameObject[] EnemyGo = GameObject.FindGameObjectsWithTag("Enemy");
            Enemy[] Enemys = new Enemy[EnemyGo.Length];
 
            for (int i = 0; i < EnemyGo.Length; i++)
            {
                Enemys[i] = EnemyGo[i].GetComponent<Enemy>();
            }

            foreach(Enemy enemy in Enemys)
            {
                float TargetDis = Vector3.SqrMagnitude(enemy.transform.position - Origin);
                if(TargetDis < RadiusSqr)//스킬범위 안이라면
                {
                    if(EnemyList.Count < Effect.MaxTarget)//맥스타겟을 넘지 않았다면
                    {
                        EnemyList.Add(enemy);
                    }                  
                }
            }

            for(int i = 0; i < EnemyList.Count; i++)
            {
                //공격전달
                _Player.StartCoroutine(DamageAttack(Effect.Power, Effect.HitCount, Effect.DelayTime, _SkillData));
            }          
        }
    }

    IEnumerator DamageAttack(float _Power, int _HitCount, float _DelayTime, SkillData _SkillData)
    {
        for (int i = 0; i < _HitCount; i++)
        {
            foreach (Enemy enemy in EnemyList)
            {
                if(enemy == null) continue;
                enemy.TakeDamage(_Power);
                EffectManager.Instance.Spawn(_SkillData.HitEffectPrefab, enemy.transform.position, _SkillData.HitPrefabDuration);
            }

            yield return new WaitForSeconds(_DelayTime);
        }
    }
}
