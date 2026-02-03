using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScreamStrategy : IBossAttack
{
    public int AttackIndex => (int)BossState.ATTACK_SCREAM;
    private bool isEventTriggered = false;

    private int attackCount = 3;
    private float attackRadius = 6.0f;
    private float indicatorDuration = 2f;

    // 스크림 공격 (랜덤 원형 범위)
    public IEnumerator Execute(EnemyBoss boss, Transform player, System.Action onComplete)
    {
        Debug.Log("ATTACK SCREAM 공격");
        isEventTriggered = false;

        //페이즈별 설정 분기
        bool isSecondPhase = boss.CurHp < boss.MaxHp * 0.5f;
        int count = isSecondPhase ? 10 : 5;
        float maxDist = isSecondPhase ? 20f : 15f; // 2페이즈 땐 더 멀리까지 장판 생성

        // 1. 랜덤 위치 선정 (플레이어 주변 3곳)
        List<Vector3> targetPositions = new List<Vector3>();
        for (int i = 0; i < count; i++)
        {
            // 방법 1: 방향(Vector)을 먼저 정하고 길이를 3~8로 설정 (추천)
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            // 거리 결정 (3m ~ 8m)
            float randomDist = Random.Range(3f, maxDist);
            // 상대적 위치 계산
            Vector2 offset = randomDir * randomDist;
            // [중요] 플레이어 위치에 오프셋을 더해 최종 월드 좌표 생성
            // Vector2의 x, y를 Vector3의 x, z로 대입합니다.
            Vector3 spawnPos = new Vector3(
                player.position.x + offset.x,
                boss.transform.position.y + 0.1f, // 보스 발바닥 높이 (바닥에 붙임)
                player.position.z + offset.y
            );
            targetPositions.Add(spawnPos);
        }

        // 2. 인디케이터(장판) 표시 (크기 조절 포함)
        // 반지름이 3이면 지름은 6이므로 Scale을 6으로 설정 (프리팹이 1Unit 기준일 때)
        Quaternion indicatorRot = Quaternion.Euler(90f, 0f, 0f);
        Vector3 indicatorScale = new Vector3(attackRadius * 2, attackRadius * 2, 1);
        Vector3 effectScale = Vector3.one;
        foreach (var pos in targetPositions)
        {
            EffectManager.Instance.Spawn(boss.GetCircleIndicator(), pos, indicatorRot, indicatorScale, indicatorDuration);
        }

        // 3. [기즈모 개선] 리스트 전체를 그리도록 단 한 번만 등록
        boss.SetGizmoAction(() => {
            Gizmos.color = new Color(1, 0, 0, 0.4f);
            foreach (var pos in targetPositions)
            {
                Gizmos.DrawWireSphere(pos, attackRadius); // 실제 판정 크기인 6m와 일치시킴
            }
        });

        // 4. 애니메이션 이벤트 대
        yield return new WaitUntil(() => isEventTriggered);

        AudioClip clip = boss.GetAttackScreamClip();
        SoundManager.Instance.PlayBossSFX(clip);

        // 5. 폭발 및 실제 타격 판정
        foreach (var pos in targetPositions)
        {
            // 폭발 이펙트 생성
            if (boss.GetScreamExplosion() != null)
            {
                EffectManager.Instance.Spawn(boss.GetScreamExplosion(), pos, Quaternion.identity, effectScale, 2.0f);
            }

            Collider[] cols = Physics.OverlapSphere(pos, 3f, boss.PlayerLayer);
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
