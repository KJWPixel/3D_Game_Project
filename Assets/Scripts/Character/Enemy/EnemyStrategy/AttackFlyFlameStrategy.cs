using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFlyFlameStrategy : IBossAttack
{
    public int AttackIndex => (int)BossState.ATTACK_FLYFRAME;
    private bool isEventTriggered = false;

    private float attackRadius = 50f; // 반지름 50 (지름 100)
    private float indicatorDuration = 3.0f;

    // 알려주신 월드 좌표 고정
    private readonly Vector3 targetWorldPos = new Vector3(376.9f, 30.1f, 92f);

    public IEnumerator Execute(EnemyBoss boss, Transform player, System.Action onComplete)
    {
        Debug.Log("ATTACK FLY FRAME(범위 공격)");
        isEventTriggered = false;

        // 1. 타격 위치 설정 (알려주신 좌표 사용)
        // y값은 바닥에 딱 붙이기 위해 0.1f 정도만 살짝 띄워줍니다.
        Vector3 spawnPos = new Vector3(targetWorldPos.x, targetWorldPos.y + 0.1f, targetWorldPos.z);

        // 2. 인디케이터(장판) 표시 - 사이즈 100
        Quaternion indicatorRot = Quaternion.Euler(90f, 0f, 0f);
        Vector3 indicatorScale = new Vector3(100f, 100f, 1f);

        // 지정된 월드 좌표에 장판 생성
        EffectManager.Instance.Spawn(boss.GetCircleIndicator(), spawnPos, indicatorRot, indicatorScale, indicatorDuration);

        // 3. 인디케이터 대기
        yield return new WaitForSeconds(indicatorDuration);

        // 4. 실제 타격 효과 생성 (알려주신 월드 좌표 위치)
        GameObject flamePrefab = boss.GetFlyFreamExplosion();
        if (flamePrefab != null)
        {
            // 이펙트 역시 해당 좌표에 생성
            EffectManager.Instance.Spawn(flamePrefab, spawnPos, Quaternion.identity, Vector3.one, 2.0f);
        }

        AudioClip clip = boss.GetAttackFlyFrameClip();
        SoundManager.Instance.PlayBossSFX(clip);

        // 5. 타격 판정 (해당 좌표 중심 반지름 50 범위 내 플레이어)
        Collider[] cols = Physics.OverlapSphere(spawnPos, attackRadius, boss.PlayerLayer);
        foreach (var col in cols)
        {
            // 플레이어에게 데미지 전달
            col.GetComponent<PlayerStat>()?.TakeDamage(boss.Atk * 2.5f);
            Debug.Log($"고정 좌표 {spawnPos}에서 광역 공격 적중!");
        }

        // 6. 애니메이션 마무리 대기
        yield return new WaitForSeconds(1.5f);

        Debug.Log("ATTACK FLY FRAME 완료");
        onComplete?.Invoke();
    }

    public void OnEffectEvent() => isEventTriggered = true;
}

