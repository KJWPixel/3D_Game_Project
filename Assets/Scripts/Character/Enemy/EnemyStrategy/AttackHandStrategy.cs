using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandStrategy : IBossAttack
{
    public int AttackIndex => (int)BossState.ATTACK_HAND;
    private bool isEventTriggered = false;

    public IEnumerator Execute(EnemyBoss boss, Transform player, System.Action onComplete)
    {
        Debug.Log("ATTACK FLY SCREAM 공격");

        isEventTriggered = false;

        boss.SetCollisionIgnore(true);
        // 1. 공격 전 플레이어 방향 고정
        //Vector3 lookPos = new Vector3(player.position.x, boss.transform.position.y, player.position.z);
        //boss.transform.LookAt(lookPos);
        Vector3 targetDir = (player.position - boss.transform.position).normalized;
        targetDir.y = 0;
        boss.transform.forward = targetDir;

        boss.SetGizmoAction(() => {
            Gizmos.color = Color.red;
            // 판정 중심점 계산
            Vector3 gizmoCenter = boss.transform.position + (boss.transform.forward * 7f);
            // 높이가 바닥이면 잘 안 보일 수 있으니 약간 띄워줌 (선택 사항)
            gizmoCenter.y += 1.0f;

            Gizmos.DrawWireSphere(gizmoCenter, 5f);
        });

        yield return new WaitUntil(() => isEventTriggered);

        AudioClip clip = boss.GetAttackHandClip();
        SoundManager.Instance.PlayBossSFX(clip);

        // 손 위치에 맞게 약간 옆으로 오프셋 판정
        Vector3 handPos = boss.transform.position + (boss.transform.forward * 7f);
        Collider[] cols = Physics.OverlapSphere(handPos, 5f, boss.PlayerLayer);
        foreach (var col in cols) col.GetComponent<PlayerStat>()?.TakeDamage(boss.Atk);

        boss.SetCollisionIgnore(false);
        boss.ClearGizmoAction();
        onComplete?.Invoke();
    }
    public void OnEffectEvent() => isEventTriggered = true;
}
