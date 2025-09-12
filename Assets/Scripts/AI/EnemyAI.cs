using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : AIBase
{   //Enemy의 AI상태 전환 조건 로직
    //상태에 따른 행동은 EnemyCharacter
    //어뎁터, 이터레이터, 커맨더 패턴, -카메라(세이크, 흐림필터), -게임완성(몬스터의 다채로움(AI증가), 애니메이터효과, 이펙트), 부족한 수업 부분 보충
    //EnhancedScroller(?), -NGUI 1달, -Spine, -AssetBundle, -Picking(2D->3D), -UnityPackage(묶기), -Build, AutoBuild, -Market
    private EnemyCharacter Enemy;
    [SerializeField] private Transform PlayerTrs;

    protected AI AI = AI.AI_CREATE;
    public AI CurrentAI => AI;

    [Header("상태 조건")]
    [SerializeField] float ChaseRange = 0f;
    [SerializeField] float ChaseStartTime = 0f;
    [SerializeField] float ChaseTime = 0f;
    [SerializeField] float AttackRange = 0f;


    private void Awake()
    {
        Enemy = GetComponent<EnemyCharacter>();
    }
    private void Update()
    {
        State();
        GetAIState();

        //상태 조건 함수
        IdleState();
        PatrolState();
        ChaseState();
        FleeState();
        AttackState();
    }

    public override void init()
    { 
        AI = AI.AI_CREATE;
    }

    private void State()
    {
        switch(AI)
        {
            case AI.AI_CREATE:
                break;
            case AI.AI_IDLE:
                break;
            case AI.AI_PATROL:
                break;
            case AI.AI_SEARCH:
                break;
            case AI.AI_CHASE:
                break;
            case AI.AI_FLEE:
                break;
            case AI.AI_ATTACK:
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
        Debug.Log($"<b><color=orange>{CurrentAI}</color></b>");
        return AI;
    }

    private void IdleState()
    {
        if(AI == AI.AI_CREATE)
        {
            AI = AI.AI_IDLE;
        }
    }

    private void PatrolState()
    {
        if (AI == AI.AI_IDLE)
        {
            AI = AI.AI_PATROL;
        }      
    }

    private void ChaseState()
    {
        if (PlayerTrs == null || AI == AI.AI_FLEE)
        {
            return; //플레이어 Trs가 Null이면 리턴, 추적시간 초과 시 리턴
        }   

        if (AI == AI.AI_PATROL)
        {
            float TargetDis = Vector3.Distance(transform.position, PlayerTrs.position);

            if (TargetDis <= ChaseRange)
            {
                if(AI != AI.AI_CHASE)
                {
                    AI = AI.AI_CHASE;
                    ChaseStartTime = Time.time;
                }
            }
        }
    }

    private void FleeState()
    {
        if(AI == AI.AI_CHASE)
        { 
            if(Time.time > ChaseStartTime + ChaseTime)
            {
                //추적시간 + 추적시작시간 < 게임진행시간 이라면 추적을 중지하고 원래 스폰 위치로 돌아감
                AI = AI.AI_FLEE;
            }
            else
            {
                AI = AI.AI_PATROL;
            }
        }
    }

    private void AttackState()
    {
        if(AI == AI.AI_CHASE)
        {
            float TargetDis = Vector3.Distance(transform.position, PlayerTrs.position);
            if (TargetDis <= AttackRange)
            {
                AI = AI.AI_ATTACK;
            }
            else
            {
                AI = AI.AI_CHASE;
            }
        }
    }


}
