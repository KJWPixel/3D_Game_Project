using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackFrameStrategy : IBossAttack
{
    public int AttackIndex => (int)BossState.ATTACK_FRAME;
    private bool isEventTriggered = false; // 이벤트 발생 여부 체크

    private float attackAngle = 60f; 
    private float attackRange = 10f; // 사거리
    public IEnumerator Execute(EnemyBoss boss, Transform player, System.Action onComplete)
    {
        isEventTriggered = false;
        Debug.Log("AttackFream  공격 시작");

        // 1. 기즈모 범위 표시 등록
        boss.SetGizmoAction(() => {
            // 보스가 오른쪽을 보고 있으므로 boss.transform.right를 전달
            boss.DrawFanGizmo(boss.transform.position, boss.transform.right, 10f, 120f);
        });

        yield return new WaitUntil(() => isEventTriggered);

        // 3. 실제 타격 판정
        // 먼저 사거리 내에 있는 모든 콜라이더를 가져옵니다.
        Collider[] cols = Physics.OverlapSphere(boss.transform.position, attackRange, boss.PlayerLayer);

        foreach (var col in cols)
        {
            Vector3 dirToTarget = (col.transform.position - boss.transform.position).normalized;
            // 보스의 정면과 타겟 사이의 각도 계산
            float angle = Vector3.Angle(boss.transform.forward, dirToTarget);

            if (angle <= attackAngle)
            {
                // 부채꼴 범위 내 적중
                col.GetComponent<PlayerStat>()?.TakeDamage(boss.Atk * 1.5f);
                Debug.Log("부채꼴 공격 적중!");
            }
        }

        Quaternion effectRot = boss.transform.rotation * Quaternion.Euler(0, -90f, 0);

        // 4. 이펙트 생성 (보스 정면 위치)
        GameObject prefab = boss.GetFrameExplosion(); // ScreamAttackPrefab 참조
        if (prefab != null)
        {
            EffectManager.Instance.Spawn(prefab, boss.transform.forward * 4f, effectRot, Vector3.one, 2.0f);
        }

        yield return new WaitForSeconds(1.0f); // 후딜레이
        boss.ClearGizmoAction();
        onComplete?.Invoke();
    }

    public void OnEffectEvent() => isEventTriggered = true;

}
