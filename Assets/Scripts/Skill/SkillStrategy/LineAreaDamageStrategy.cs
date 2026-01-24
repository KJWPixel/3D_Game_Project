using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineAreaDamageStrategy : ISkillBehaviorStrategy
{
    public void Execute(PlayerController _Player, PlayerStat _PlayerStat, SkillData _SkillData, Transform _Target)
    {
        SoundManager.Instance.PlayOneShot(_SkillData.CastSFX, _Player.transform.position);

        SoundManager.Instance.PlayOneShot(_SkillData.CastSFX, _Player.transform.position);

        Vector3 playerPos = _Player.transform.position;
        Vector3 forward = _Player.transform.forward.normalized; // 정규화 필수

        foreach (var Effect in _SkillData.Effects)
        {
            Debug.Log("스킬 LineArea 실행");

            // 1. 스킬 시작 위치 (인디케이터와 동일하게!)
            Vector3 startPos = playerPos + forward * Effect.Distance + Vector3.up * 0.1f;

            // 2. Effect 생성 (인디케이터 위치와 정확히 일치)
            Quaternion rotation = Quaternion.LookRotation(forward);
            EffectManager.Instance.Spawn(_SkillData.CastEffectPrefab, startPos, rotation, _SkillData.CastPrefabDuration);

            // 3. 모든 적 찾기 (DistanceArea와 동일)
            GameObject[] enemyGos = GameObject.FindGameObjectsWithTag("Enemy");
            List<EnemyCharacter> hitEnemies = new List<EnemyCharacter>();

            foreach (var go in enemyGos)
            {
                EnemyCharacter enemy = go.GetComponent<EnemyCharacter>(); // 또는 GetComponentInParent
                if (enemy == null || enemy.IsDie) continue;

                // 4. 직선 범위 체크 (가장 정확한 방법)
                if (IsInLineArea(enemy.transform.position, startPos, forward, Effect.Length, Effect.Width))
                {
                    hitEnemies.Add(enemy);
                }
            }

            // 5. MaxTarget 제한 + 거리순 정렬 (DistanceArea와 동일 로직)
            var sorted = hitEnemies
                .OrderBy(e => Vector3.Distance(playerPos, e.transform.position))
                .Take(Effect.MaxTarget)
                .ToList();

            if (sorted.Count == 0) continue;

            Debug.Log($"스킬 LineArea 코루틴 실행 → 타겟 {sorted.Count}명");
            _Player.StartCoroutine(DealDamageOverTime(sorted.ToArray(), Effect.PowerMultiplier, Effect.HitCount, Effect.DelayTime, _SkillData));
        }
    }

    IEnumerator DealDamageOverTime(EnemyCharacter[] targets, float powerMultiplier, int hitCount, float delay, SkillData skillData)
    {
        for (int i = 0; i < hitCount; i++)
        {
            foreach (var enemy in targets)
            {
                if (enemy == null || enemy.IsDie) continue;

                var result = PlayerStat.Instance.CalculateFinalDamage(powerMultiplier, enemy.Def); // Final 오타 수정 추천
                Debug.Log("스킬 전략패턴 LineArea TakeDamage 호출");
                enemy.TakeDamage(result.damage, result.isCrit);

                // 히트 이펙트 위치 → enemy의 중심이나 대표 위치 사용
                // (보스처럼 크기가 크면 transform.position 대신 bounds.center 등을 고려 가능)
                Vector3 hitPosition = enemy.transform.position + Vector3.up * 1.2f; // 예시
                Quaternion hitRotation = Quaternion.identity; // 또는 방향 맞춰서

                EffectManager.Instance.Spawn(
                    skillData.HitEffectPrefab,
                    hitPosition,
                    hitRotation,
                    skillData.HitPrefabDuration  // duration이 float라면
                );
            }

            if (i < hitCount - 1)
                yield return new WaitForSeconds(delay);
        }
    }

    private bool IsInLineArea(Vector3 point, Vector3 start, Vector3 dir, float length, float width)
    {
        Vector3 toPoint = point - start;
        float proj = Vector3.Dot(toPoint, dir); // 선분 위 투영 길이

        if (proj < 0 || proj > length) return false; // 길이 범위 벗어남

        // 선분에 가장 가까운 점
        Vector3 closestOnLine = start + dir * proj;

        // 수직 거리 (Width/2 이내)
        float distToLine = Vector3.Distance(closestOnLine, point);
        return distToLine <= width * 0.5f;
    }

}
