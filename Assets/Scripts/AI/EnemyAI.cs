using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : AIBase
{   //Enemy의 AI상태 전환 조건 로직
    //상태에 따른 행동은 EnemyCharacter
    //9.12 어뎁터, 이터레이터, 커맨더 패턴, -카메라(세이크, 흐림필터), -게임완성(몬스터의 다채로움(AI증가), 애니메이터효과, 이펙트), 부족한 수업 부분 보충
    //EnhancedScroller(?), -NGUI 1달, -Spine, -AssetBundle, -Picking(2D->3D), -UnityPackage(묶기), -Build, AutoBuild, -Market
    //9.16 AssetBundle

    private EnemyCharacter Enemy;
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
        Enemy = GetComponent<EnemyCharacter>();
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
                IdleState();
                Enemy.Idle();
                break;
            case AI.AI_PATROL:
                PatrolState();
                Enemy.Patrol();
                break;
            case AI.AI_SEARCH:
                Enemy.Search();
                break;
            case AI.AI_CHASE:
                ChaseState();
                Enemy.Chase();
                break;
            case AI.AI_FLEE:
                Enemy.Flee();
                break;
            case AI.AI_ATTACK:
                Enemy.Attack();
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

    private void IdleState()
    {
        //IDLE 상태에서 상태전환의 여러 조건을 추가
        //전환할 상태 - PATROL, CHASE, ATTACK 등등

        //PATROL 전환:IDLE 시작 시간에서 일정 시간이 지나면 PATROL 상태로 전환
        if (AI == AI.AI_IDLE && IdleStartTime == 0f)
        {
            IdleStartTime = Time.time;

            if (Time.time - IdleStartTime >= IdleDuration)
            {
                AI = AI.AI_PATROL;
                IdleStartTime = 0f;        
            }
        }

        //CHASE 전환: IDLE상태에서 일정 거리에 다가서면 CHASE상태로 전환
        ChaseState();
    }

    private void PatrolState()
    {
        //PATROL 상태
        //PATROL상태라고는 하나 행동은 Enemy에서 TRPATH로 지정된 Trs위치로 계속 이동
        //EnemyAI에서 다른 동작 추가할게 없음

        ChaseState();
    }

    private void ChaseState()
    {
        //CHASE 상태
        //플레이어가 일정 추적거리에 들어오면 Chase 상태로 전환
        //플레이어가 추적거리 안쪽 AttackRange까지 들어오면 바로 ATTACK 상태로 전환
        //CHASE상태에 일정 시간이 지나면 FLEE 상태로 전환

        if (PlayerTrs == null) return;

        float TargetDis = Vector3.Distance(transform.position, PlayerTrs.position);

        if(TargetDis <= AttackRange)
        {
            AI = AI.AI_ATTACK;
            return;
        }

        if (AI != AI.AI_CHASE && TargetDis <= ChaseRange)
        {
            AI = AI.AI_CHASE;
            ChaseStartTime = Time.time;
        }

        if (Time.time > ChaseStartTime + ChaseTime)
        {
            AI = AI.AI_FLEE;
        }
    }
}
