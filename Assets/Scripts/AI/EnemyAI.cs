using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : AIBase
{   //Enemy�� AI���� ��ȯ ���� ����
    //���¿� ���� �ൿ�� EnemyCharacter
    //���, ���ͷ�����, Ŀ�Ǵ� ����, -ī�޶�(����ũ, �帲����), -���ӿϼ�(������ ��ä�ο�(AI����), �ִϸ�����ȿ��, ����Ʈ), ������ ���� �κ� ����
    //EnhancedScroller(?), -NGUI 1��, -Spine, -AssetBundle, -Picking(2D->3D), -UnityPackage(����), -Build, AutoBuild, -Market
    private EnemyCharacter Enemy;
    [SerializeField] private Transform PlayerTrs;

    protected AI AI = AI.AI_CREATE;
    public AI CurrentAI => AI;

    [Header("���� ����")]
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

        //���� ���� �Լ�
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
    } //AI ���¿� ���� �Լ� ����

    private AI GetAIState()//AI�� ���°� ����
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
            return; //�÷��̾� Trs�� Null�̸� ����, �����ð� �ʰ� �� ����
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
                //�����ð� + �������۽ð� < ��������ð� �̶�� ������ �����ϰ� ���� ���� ��ġ�� ���ư�
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
