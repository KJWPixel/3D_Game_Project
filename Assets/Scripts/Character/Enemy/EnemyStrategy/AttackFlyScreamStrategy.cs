using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; // Random 중복 방지

public class AttackFlyScreamStrategy : IBossAttack
{
    public int AttackIndex => (int)BossState.ATTACK_FLYSCREAM;

    private bool isEventTriggered = false;
    private float attackRadius = 6.0f;
    private float indicatorDuration = 2f;

    public IEnumerator Execute(EnemyBoss boss, Transform player, Action onComplete)
    {
        Debug.Log("ATTACK FLY SCREAM 공격");
        isEventTriggered = false;

        // 2페이즈(공중)이므로 항상 강화된 수치 적용
        int count = 10;
        float maxDist = 20f;

        // 1. 랜덤 위치 선정
        List<Vector3> targetPositions = new List<Vector3>();
        for (int i = 0; i < count; i++)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            float randomDist = Random.Range(3f, maxDist);
            Vector2 offset = randomDir * randomDist;

            // 보스가 하늘에 있으므로 player.position.y를 기준으로 바닥 좌표 설정
            Vector3 spawnPos = new Vector3(
                player.position.x + offset.x,
                player.position.y + 0.1f,
                player.position.z + offset.y
            );
            targetPositions.Add(spawnPos);
        }

        // 2. 인디케이터(장판) 표시
        Quaternion indicatorRot = Quaternion.Euler(90f, 0f, 0f);
        Vector3 indicatorScale = new Vector3(attackRadius * 2, attackRadius * 2, 1);

        foreach (var pos in targetPositions)
        {
            EffectManager.Instance.Spawn(boss.GetCircleIndicator(), pos, indicatorRot, indicatorScale, indicatorDuration);
        }

        // 3. 기즈모 등록
        boss.SetGizmoAction(() => {
            Gizmos.color = new Color(1, 0, 0, 0.4f);
            foreach (var pos in targetPositions)
            {
                Gizmos.DrawWireSphere(pos, attackRadius);
            }
        });

        // 4. 애니메이션 이벤트 대기 (공중에서 포효하는 애니메이션 타이밍)
        yield return new WaitForSeconds(indicatorDuration);

        // 5. 실제 타격 판정
        foreach (var pos in targetPositions)
        {
            if (boss.GetScreamExplosion() != null)
            {
                EffectManager.Instance.Spawn(boss.GetScreamExplosion(), pos, Quaternion.identity, Vector3.one, 2.0f);
            }

            Collider[] cols = Physics.OverlapSphere(pos, attackRadius, boss.PlayerLayer);
            foreach (var col in cols)
            {
                col.GetComponent<PlayerStat>()?.TakeDamage(boss.Atk * 1.5f);
            }
        }

        yield return new WaitForSeconds(0.5f);
        boss.ClearGizmoAction();
        onComplete?.Invoke();
    }

    public void OnEffectEvent() => isEventTriggered = true;
}
