using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMouthStrategy : IBossAttack
{
    public int AttackIndex => (int)BossState.ATTACK_MOUTH;
    private bool isEventTriggered = false;

    public IEnumerator Execute(EnemyBoss boss, Transform player, Action onComplete)
    {
        Debug.Log("ATTACK MOUTH 공격");

        isEventTriggered = false;

        //Vector3 lookPos = new Vector3(player.position.x, boss.transform.position.y, player.position.z);
        //boss.transform.LookAt(lookPos);
        Vector3 targetDir = (player.position - boss.transform.position).normalized;
        targetDir.y = 0;
        boss.transform.forward = targetDir;

        // 근접 공격은 인디케이터 없이 빠른 반응 속도 
        yield return new WaitUntil(() => isEventTriggered);

        AudioClip clip = boss.GetAttackMouthClip();
        SoundManager.Instance.PlayBossSFX(clip);

        //Gizmo 
        Vector3 hitPos = boss.transform.position + boss.transform.forward * 2f;
        float hitRadius = 7.0f;

        // 기즈모 등록
        boss.SetGizmoAction(() => {
            Gizmos.DrawWireSphere(hitPos, hitRadius);
        });

        Collider[] cols = Physics.OverlapSphere(boss.transform.position + boss.transform.forward * 2f, 7f, boss.PlayerLayer);
        foreach (var col in cols) col.GetComponent<PlayerStat>()?.TakeDamage(boss.Atk);

        boss.ClearGizmoAction();
        onComplete?.Invoke();
    }

    public void OnEffectEvent() => isEventTriggered = true;
}
