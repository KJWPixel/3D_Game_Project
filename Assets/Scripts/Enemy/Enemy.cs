using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �� �ൿ: HP, ���ݷ�, �ִϸ��̼� Ʈ���� ��
/// �÷��̾ �ܺ� ȣ�� �̺�Ʈ: �ǰ�, ����, ���� �� 
/// 
/// AI_Enemy.cs�� ����� ������ ����
/// Enemy�� HP, �ִϸ��̼�, ���� ó�� ���� "���� �ൿ"�� ���
/// </summary>

public class Enemy : BaseEnemy
{
    [SerializeField] AI_Enemy AI_Enemy;
    [SerializeField] Character Character;
    [SerializeField] GameObject Player;

    [SerializeField] Transform[] TRPATH;
    [SerializeField] int CurrentPathIndex = 0;
    [SerializeField] float PatrolWaitTime = 0f;
    float WaitTime = 0f;

    [Header("ü��")]
    [SerializeField] public float MaxHp = 0f;
    [SerializeField] public float CurHp = 0f;

    [Header("���ݷ�")]
    [SerializeField] float AttackDamage = 0f;

    [Header("�̵��ӵ�")]
    [SerializeField] float WalkSpeed = 0f;
    [SerializeField] float ChaseSpeed = 0f;

    [Header("���� üũ")]
    [SerializeField] bool IsDie = false;    

    Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    void Start()
    {
        Player = GameObject.FindWithTag("Player");

        if (Player == null)
        {
            Debug.Log("Player ��ü Null");
        }
    }


    void Update()
    {
        AnimationControll();
    }

    private void AnimationControll()
    {
        if (AI_Enemy != null)
        {
            if (AI_Enemy.CurrentAI == AI.AI_CREATE)
            {
                Animator.SetBool("Search", false);
                Animator.SetBool("Chase", false);
                Animator.SetBool("Attack", false);
            }
            else if (AI_Enemy.CurrentAI == AI.AI_SEARCH)
            {
                //Search�Լ����� �ִϸ����� ����
            }
            else if (AI_Enemy.CurrentAI == AI.AI_CHASE)
            {
                Animator.SetBool("Search", false);
                Animator.SetBool("Chase", true);
                Animator.SetBool("Attack", false);
            }
            else if (AI_Enemy.CurrentAI == AI.AI_ATTACK)
            {
                Animator.SetBool("Search", false);
                Animator.SetBool("Chase", false);
                Animator.SetBool("Attack", true);
            }
        }
    }

    public void Idle()
    {
        Debug.Log("Enemy Idle");
    }

    public void Search()
    {
        if (AI_Enemy.CurrentAI != AI.AI_SEARCH || TRPATH.Length == 0)
        {
            //AI=Search Not or TRPATH Index 0
            return;
        }

        Debug.Log("Enemy Search");

        Transform PathPoint = TRPATH[CurrentPathIndex];
        Vector3 Dir = PathPoint.position - transform.position;//��ǥ���� - ������ġ: ��ǥ�� ���ϴ� ���⺤��

        //�̵�
        float Distance = Dir.magnitude;//������ ���̸� �ǹ�

        if(Distance > 0.2f)
        {
            transform.position += Dir.normalized * WalkSpeed * Time.deltaTime;

            if(Dir != Vector3.zero)
            {
                //���� ����Dir�� ���� �������� �ϴ� ȸ���� �����.
                Quaternion PathRotation = Quaternion.LookRotation(Dir);

                //���� ���� ����(Slerp)(���� ȸ��, ��ǥ ȸ��, ���� �ӵ�)
                transform.rotation = Quaternion.Slerp(transform.rotation, PathRotation, 5f * Time.deltaTime);
            }

            WaitTime = 0f;
            Animator.SetBool("Search", true);
        }
        else
        {
            WaitTime += Time.deltaTime;
            Animator.SetBool("Search", false);
            if(WaitTime >= PatrolWaitTime)
            {
                CurrentPathIndex = (CurrentPathIndex + 1) % TRPATH.Length;
                WaitTime = 0f;
            }
        }

    }

    public void Chase()
    {
        if (AI_Enemy.CurrentAI == AI.AI_ATTACK)
        {
            return;
        }

        if (AI_Enemy.CurrentAI == AI.AI_CHASE)
        {
            Debug.Log("Enemy Chase");
            Vector3 Dir = Character.transform.position - transform.position;
            transform.position += Dir.normalized * ChaseSpeed * Time.deltaTime;

            if (Dir != Vector3.zero)//(Dir.sqrMagnitude > 0.0001f) dir �� Vector3.zero �� �� (��, ���̰� 0�� ��) Quaternion.LookRotation(dir)�� �ϸ� ��� ����
            {
                Quaternion TargetRotation = Quaternion.LookRotation(Dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, 5f * Time.deltaTime);
            }
        }
        //Magnitude, sqrMagnitude�� ���� ����
        #region
        //magnitude: ������ ���� (����:Vector3(3,0,4) = 5) ��Ʈ �����Ͽ� 25���� 5��
        //sqrMagnitude: ������ ������ ���� (����:Vector3(3,0,4) = 25) �������Ͽ� 25
        #endregion//
    }

    public void Attack()
    {
        Debug.Log("Enemy Attack Start");
        float Distance = Vector3.Distance(transform.position, Character.transform.position);

        if (Distance <= 3)
        {
            PlayerStat PlayerStat = Player.GetComponent<PlayerStat>();
            if (PlayerStat != null)
            {
                PlayerStat.TakeDamage(AttackDamage);
            }
        }
    }

    private void AttackDistanceGizmo()
    {
        Gizmos.DrawWireSphere(transform.position, 3);
    }

    public void TakeDamage(float _Damage)
    {
        if(CurHp > 0)
        {
            CurHp -= _Damage;
        }
        else
        {
            IsDie = true;
        }
    }

    public void Reset()
    {

    }
}
