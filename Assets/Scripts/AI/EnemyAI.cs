using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyAI : AIBase
{   //Enemy의 AI상태 전환 조건 로직
    //상태에 따른 행동은 EnemyCharacter
    //9.12 어뎁터, 이터레이터, 커맨더 패턴, -카메라(세이크, 흐림필터), -게임완성(몬스터의 다채로움(AI증가), 애니메이터효과, 이펙트), 부족한 수업 부분 보충
    //EnhancedScroller(?), -NGUI 1달, -Spine, -AssetBundle, -Picking(2D->3D), -UnityPackage(묶기), -Build, AutoBuild, -Market
    //9.16 AssetBundle

    private Enemy Enemy;
    [SerializeField] private Transform PlayerTrs;

    protected AI AI = AI.AI_CREATE;
    public AI CurrentAI => AI;

    [Header("IDLE 상태 조건")]
    [SerializeField] float IdleStartTime = 0f;
    [SerializeField] float IdleDuration = 0f;

    [Header("CHASE 상태 조건")]
    [SerializeField] float ChaseRange = 0f;
    [SerializeField] float ChaseStartTime = 0f;
    [SerializeField] float ChaseTime = 0f;

    [Header("ATTACK 상태 조건")]
    [SerializeField] float AttackRange = 0f;


    private void Awake()
    {
        Enemy = GetComponent<Enemy>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        if(playerObject != null)
        {
            PlayerTrs = playerObject.transform;
        }
        
    }
    private void Update()
    {
        State();
        GetAIState();
    }

    public override void init()
    {
        AI = AI.AI_CREATE;
    }

    private void State()
    {
        //FSM2: 검색과 이동(타겟을 찾기), (자신의 이동) 나머지 처리는 Character에서 처리
        //현재 확장성 떨어짐, EnemyAI의 함수끼리가 결함되지 않음 
        //AI를 계획적으로 다시 확인해야됨
        //IDLE상태에서는 무엇을 해야하는지 등등을 따져야됨
        switch (AI)
        {
            case AI.AI_CREATE:
                CreateState();
                break;
            case AI.AI_IDLE:
                IdleTransition();
                Enemy.Idle();
                break;
            case AI.AI_PATROL:
                PatrolTransition();
                Enemy.Patrol();
                break;
            case AI.AI_SEARCH:
                Enemy.Search();
                break;
            case AI.AI_CHASE:
                ChaseTransition();
                Enemy.Chase();
                break;
            case AI.AI_FLEE:
                FleeTransition();
                Enemy.Flee();
                break;
            case AI.AI_ATTACK:
                AttackTransition();
                break;
            case AI.AI_SKILL:
                break;
            case AI.AI_DEAD:
                DeadTransition();
                break;
            case AI.AI_RESET:
                break;
        }
    } //AI 상태에 따른 함수 동작

    private AI GetAIState()//AI의 상태값 리턴
    {
        Debug.Log($"EenyAI 상태 출력: <b><color=orange>{CurrentAI}</color></b>");
        return AI;
    }

    private bool IsPlayerInRange(float range)
    {
        if (PlayerTrs == null) return false;
        return Vector3.Distance(transform.position, PlayerTrs.position) <= range;
    }

    private bool HasIdleTimePassed()
    {
        return Time.time - IdleStartTime >= IdleDuration;
    }
    private bool HasChaseTimePassed()
    {
        return Time.time - ChaseStartTime >= ChaseTime;
    }

    private void CreateState()
    {
        //Enemy가 처음 생성(스폰)되었을 시 CREATE상태이면서 CREATE Animation 출력
        //Enemy.AnimatoUpdate(AI _AI)로 _AI상태에 따라 AnimationUpdate됨
        //스폰 Animation이 출력되기에 아무것도하지 않고 Animation만 재생하게 만듬
        if (AI == AI.AI_CREATE)
        {
            bool canPatrol = Enemy.TRPATH != null && Enemy.TRPATH.Length > 0 && Enemy.TRPATHCheck;

            if (canPatrol)
            {
                AI = AI.AI_PATROL;
            }
            else
            {
                AI = AI.AI_IDLE;
                // IdleStartTime을 여기서 초기화해야 IDLE 상태 진입 후 바로 PATROL로 넘어가지 않습니다.
                IdleStartTime = Time.time;
            }
        }
    }

    private void IdleTransition()
    {
        if (IsPlayerInRange(ChaseRange))
        {
            AI = AI.AI_CHASE;
            ChaseStartTime = Time.time;
        }
        else if (HasIdleTimePassed())
        {
            bool canPatrol = Enemy.TRPATH != null && Enemy.TRPATH.Length > 0 && Enemy.TRPATHCheck;

            if (canPatrol)
            {
                AI = AI.AI_PATROL; // 순찰 경로가 있고, 순찰이 활성화된 경우에만 PATROL로 전환
            }
            else
            {
                // 순찰 조건이 불충족되면 IDLE 상태 유지
                // IdleStartTime을 다시 현재 시간으로 리셋하여 IdleDuration만큼 다시 대기합니다.
                IdleStartTime = Time.time;
                AI = AI.AI_IDLE; // 명시적으로 IDLE 유지
            }
        }
    }

    private void PatrolTransition()
    {
        if (IsPlayerInRange(ChaseRange))
        {
            AI = AI.AI_CHASE;
            ChaseStartTime = Time.time;
        }
    }

    private void ChaseTransition()
    {
        if (IsPlayerInRange(AttackRange))
        {
            AI = AI.AI_ATTACK;
        }
        else if (HasChaseTimePassed())
        {
            AI = AI.AI_FLEE;
        }
    }

    private void FleeTransition()
    {
        Vector3 targetPosition;
        AI nextAIState;

        // 순찰 경로의 유효성 (null 아님, 길이 0 초과, 순찰 체크 활성화)
        bool hasValidPatrolPath = Enemy.TRPATH != null && Enemy.TRPATH.Length > 0 && Enemy.TRPATHCheck;

        if (hasValidPatrolPath)
        {
            // 1. PATROL 몬스터: 현재 순찰 지점으로 복귀 후 PATROL 상태로 전환
            if (Enemy.CurrentPathIndex >= 0 && Enemy.CurrentPathIndex < Enemy.TRPATH.Length && Enemy.TRPATH[Enemy.CurrentPathIndex] != null)
            {
                targetPosition = Enemy.TRPATH[Enemy.CurrentPathIndex].position;
                nextAIState = AI.AI_PATROL;
            }
            else
            {
                // 인덱스 문제 발생 시 최초 위치로 복귀 및 IDLE 전환 (안전 장치)
                targetPosition = Enemy.OriginalPosition;
                nextAIState = AI.AI_IDLE;
            }
        }
        else
        {
            // 2. IDLE 몬스터: 최초 스폰 위치로 복귀 후 IDLE 상태로 전환
            targetPosition = Enemy.OriginalPosition;
            nextAIState = AI.AI_IDLE;
        }

        // 복귀 목표와의 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // 목표에 충분히 가까워지면 상태 전환 (복귀 완료)
        if (distanceToTarget < 1.0f) // 1.0f는 복귀 완료로 간주하는 거리 임계값
        {
            AI = nextAIState;
            // 상태 전환 후 IDLE/PATROL 상태 진입 시간 초기화
            IdleStartTime = Time.time;
        }
    }

    private void AttackTransition()
    {
        if (!IsPlayerInRange(AttackRange))
        {
            AI = AI.AI_CHASE;
            ChaseStartTime = Time.time;
        }
    }

    public void OnDamageByPlayer()
    {
        if(AI == AI.AI_DEAD) return;

        AI = AI.AI_CHASE;
        ChaseStartTime += Time.time;
    }

    public void SetDeadState()
    {
        AI = AI.AI_DEAD;
    }

    private void DeadTransition()
    {

    }
}


