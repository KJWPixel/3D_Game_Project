using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class EnemyBossAI : AIBase
{
    private EnemyBoss boss;
    private Transform player;

    private void Awake()
    {
        
    }

    private void Start()
    {
        boss = GetComponent<EnemyBoss>();
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {

        // 보스가 죽거나 isActionRunning, 페이즈 전환하면 리턴
        if (boss.IsDie || boss.isActionRunning || boss.CurrentState == BossState.PHASE_TRANSITION) return;

        // 100m 밖 → IDLE 강제 유지
        if (!boss.isPlayerInRange)
        {
            boss.ChangeState(BossState.IDLE);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= boss.AttackRange)
        {
            // 플레이어가 공격범위에 들어오면 StartRandomAttack 실행
            StartCoroutine(boss.StartRandomAttack());
        }
        else
        {
            // 플레이어가 공격범위에 들어오지 않았다면 추적
            boss.ChangeState(BossState.CHASE);
            boss.Chase();
        }
    }

    public override void init()
    {
        
    }
}
