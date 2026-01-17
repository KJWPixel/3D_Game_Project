using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAreaDamageStrategy : ISkillBehaviorStrategy
{
    public void Execute(PlayerController _Player, PlayerStat _PlayerStat, SkillData _SkillData, Transform _Target)
    {      
        SoundManager.Instance.PlayOneShot(_SkillData.CastSFX, _Player.transform.position);

        foreach (var Effect in _SkillData.Effects)
        {
            Vector3 forward = _Player.transform.forward;
            Vector3 center = _Player.transform.position + 
            forward *  (Effect.Distance + Effect.Length * 0.5f) + Vector3.up * 0.05f;
            Vector3 halfExtents = new Vector3(Effect.Width * 0.5f, 2f, Effect.Length * 0.5f);

            Quaternion rotation = Quaternion.LookRotation(forward);

            EffectManager.Instance.Spawn(_SkillData.CastEffectPrefab, center, rotation, _SkillData.CastPrefabDuration);

            // OverlapSphera(center, radius) 사용하려면, 가벼운 판정을 위해 Box사용
            Collider[] hits = Physics.OverlapBox(center, halfExtents, rotation);

            List<Collider> EnemyList = new List<Collider>();

            foreach (var col in hits)
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

            int count = Mathf.Min(Effect.MaxTarget, EnemyList.Count);

            if(count > 0)
            {
                //Collider[] LimitTarget = EnemyList.GetRange(0, count).ToArray();
                _Player.StartCoroutine(DealDamageOverTime(EnemyList.GetRange(0, count).ToArray(), Effect.PowerMultiplier, Effect.HitCount, Effect.DelayTime, _SkillData));
            }            
        }

        IEnumerator DealDamageOverTime(Collider[] _Coliiders, float power, int _HitCount, float _Delay, SkillData _SkillData)
        {
            for(int i = 0; i < _HitCount; i++)
            {
                foreach (var col in _Coliiders)
                {
                    if(col == null) continue;

                    if(col.CompareTag("Enemy"))
                    {
                        Enemy enemy = col.GetComponent<Enemy>();
                        if(enemy != null)
                        {
                            // 플레이어에서 계산한 결과르 받아오기
                            var result = PlayerStat.Instance.CalculateFianlDamage(power, enemy.Def);

                            // 적에게 최종 결과 전달
                            enemy.TakeDamage(result.damage, result.isCrit);

                            EffectManager.Instance.Spawn(_SkillData.HitEffectPrefab, enemy.transform.position, _SkillData.HitPrefabDuration);
                        }
                    }
                }
                yield return new WaitForSeconds(_Delay);    
            }
        }
    }
}
