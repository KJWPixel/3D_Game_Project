using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyAI : AIBase
{   //Enemy�� AI���� ��ȯ ���� ����
    //���¿� ���� �ൿ�� EnemyCharacter
    //9.12 ���, ���ͷ�����, Ŀ�Ǵ� ����, -ī�޶�(����ũ, �帲����), -���ӿϼ�(������ ��ä�ο�(AI����), �ִϸ�����ȿ��, ����Ʈ), ������ ���� �κ� ����
    //EnhancedScroller(?), -NGUI 1��, -Spine, -AssetBundle, -Picking(2D->3D), -UnityPackage(����), -Build, AutoBuild, -Market
    //9.16 AssetBundle

    private Enemy Enemy;
    [SerializeField] private Transform PlayerTrs;

    protected AI AI = AI.AI_CREATE;
    public AI CurrentAI => AI;

    [Header("IDLE ���� ����")]
    [SerializeField] float IdleStartTime = 0f;
    [SerializeField] float IdleDuration = 0f;

    [Header("CHASE ���� ����")]
    [SerializeField] float ChaseRange = 0f;
    [SerializeField] float ChaseStartTime = 0f;
    [SerializeField] float ChaseTime = 0f;

    [Header("ATTACK ���� ����")]
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
        //FSM2: �˻��� �̵�(Ÿ���� ã��), (�ڽ��� �̵�) ������ ó���� Character���� ó��
        //���� Ȯ�强 ������, EnemyAI�� �Լ������� ���Ե��� ���� 
        //AI�� ��ȹ������ �ٽ� Ȯ���ؾߵ�
        //IDLE���¿����� ������ �ؾ��ϴ��� ����� �����ߵ�
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
    } //AI ���¿� ���� �Լ� ����

    private AI GetAIState()//AI�� ���°� ����
    {
        Debug.Log($"EenyAI ���� ���: <b><color=orange>{CurrentAI}</color></b>");
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
        //Enemy�� ó�� ����(����)�Ǿ��� �� CREATE�����̸鼭 CREATE Animation ���
        //Enemy.AnimatoUpdate(AI _AI)�� _AI���¿� ���� AnimationUpdate��
        //���� Animation�� ��µǱ⿡ �ƹ��͵����� �ʰ� Animation�� ����ϰ� ����
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


