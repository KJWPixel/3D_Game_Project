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
            AI = AI.AI_IDLE;
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
            AI = AI.AI_PATROL;
            IdleStartTime = Time.time;
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
        float SpawnDistance = Vector3.Distance(transform.position, Enemy.TRPATH[Enemy.CurrentPathIndex].transform.position);

        if(SpawnDistance > 2)
        {
            AI = AI.AI_PATROL;
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
}


