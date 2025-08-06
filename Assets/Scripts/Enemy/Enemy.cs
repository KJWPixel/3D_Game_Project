using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 데이터 및 행동: HP, 공격력, 애니메이션 트리거 등
/// 플레이어나 외부 호출 이벤트: 피격, 죽음, 스폰 등 
/// 
/// AI_Enemy.cs를 멤버로 가지고 있음
/// Enemy의 HP, 애니메이션, 공격 처리 같은 "실제 행동"을 담당
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

    [Header("체력")]
    [SerializeField] public float MaxHp = 0f;
    [SerializeField] public float CurHp = 0f;

    [Header("공격력")]
    [SerializeField] float AttackDamage = 0f;

    [Header("이동속도")]
    [SerializeField] float WalkSpeed = 0f;
    [SerializeField] float ChaseSpeed = 0f;

    [Header("상태 체크")]
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
            Debug.Log("Player 객체 Null");
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
                //Search함수에서 애니메이터 동작
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
        Vector3 Dir = PathPoint.position - transform.position;//목표지점 - 현재위치: 목표로 향하는 방향벡터

        //이동
        float Distance = Dir.magnitude;//벡터의 길이를 의미

        if(Distance > 0.2f)
        {
            transform.position += Dir.normalized * WalkSpeed * Time.deltaTime;

            if(Dir != Vector3.zero)
            {
                //방향 벡터Dir를 정면 방향으로 하는 회전을 만든다.
                Quaternion PathRotation = Quaternion.LookRotation(Dir);

                //구면 선형 보간(Slerp)(현재 회전, 목표 회전, 보간 속도)
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

            if (Dir != Vector3.zero)//(Dir.sqrMagnitude > 0.0001f) dir 이 Vector3.zero 일 때 (즉, 길이가 0일 때) Quaternion.LookRotation(dir)을 하면 경고가 나옴
            {
                Quaternion TargetRotation = Quaternion.LookRotation(Dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, 5f * Time.deltaTime);
            }
        }
        //Magnitude, sqrMagnitude에 대한 설명
        #region
        //magnitude: 벡터의 길이 (예시:Vector3(3,0,4) = 5) 루트 제곱하여 25에서 5로
        //sqrMagnitude: 벡터의 길이의 제곱 (예시:Vector3(3,0,4) = 25) 제곱만하여 25
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
