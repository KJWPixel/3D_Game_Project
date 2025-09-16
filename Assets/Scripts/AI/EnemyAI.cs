using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : AIBase
{   //Enemy�� AI���� ��ȯ ���� ����
    //���¿� ���� �ൿ�� EnemyCharacter
    //9.12 ���, ���ͷ�����, Ŀ�Ǵ� ����, -ī�޶�(����ũ, �帲����), -���ӿϼ�(������ ��ä�ο�(AI����), �ִϸ�����ȿ��, ����Ʈ), ������ ���� �κ� ����
    //EnhancedScroller(?), -NGUI 1��, -Spine, -AssetBundle, -Picking(2D->3D), -UnityPackage(����), -Build, AutoBuild, -Market
    //9.16 AssetBundle

    private EnemyCharacter Enemy;
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
    } //AI ���¿� ���� �Լ� ����

    private AI GetAIState()//AI�� ���°� ����
    {
        Debug.Log($"EenyAI ���� ���: <b><color=orange>{CurrentAI}</color></b>");
        return AI;
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

    private void IdleState()
    {
        //IDLE ���¿��� ������ȯ�� ���� ������ �߰�
        //��ȯ�� ���� - PATROL, CHASE, ATTACK ���

        //PATROL ��ȯ:IDLE ���� �ð����� ���� �ð��� ������ PATROL ���·� ��ȯ
        if (AI == AI.AI_IDLE && IdleStartTime == 0f)
        {
            IdleStartTime = Time.time;

            if (Time.time - IdleStartTime >= IdleDuration)
            {
                AI = AI.AI_PATROL;
                IdleStartTime = 0f;        
            }
        }

        //CHASE ��ȯ: IDLE���¿��� ���� �Ÿ��� �ٰ����� CHASE���·� ��ȯ
        ChaseState();
    }

    private void PatrolState()
    {
        //PATROL ����
        //PATROL���¶��� �ϳ� �ൿ�� Enemy���� TRPATH�� ������ Trs��ġ�� ��� �̵�
        //EnemyAI���� �ٸ� ���� �߰��Ұ� ����

        ChaseState();
    }

    private void ChaseState()
    {
        //CHASE ����
        //�÷��̾ ���� �����Ÿ��� ������ Chase ���·� ��ȯ
        //�÷��̾ �����Ÿ� ���� AttackRange���� ������ �ٷ� ATTACK ���·� ��ȯ
        //CHASE���¿� ���� �ð��� ������ FLEE ���·� ��ȯ

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
