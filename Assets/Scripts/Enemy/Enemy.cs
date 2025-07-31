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

    [Header("공격력")]
    [SerializeField] float AttackDamage = 0f;

    [Header("이동속도")]
    [SerializeField] float MoveSpeed = 0f;

    Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    void Start()
    {
        Player = GameObject.FindWithTag("Player");

        if(Player == null)
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
                Animator.SetBool("Search", true);
                Animator.SetBool("Chase", false);
                Animator.SetBool("Attack", false);
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
        Debug.Log("Enemy Search");
    }

    public void Chase()
    {
        if (AI_Enemy.CurrentAI == AI.AI_ATTACK)
        {
            return;
        }

        if (AI_Enemy.PlayerChese)
        {
            Debug.Log("Enemy Chase");
            Vector3 Dir = Character.transform.position - transform.position;
            transform.position += Dir.normalized * MoveSpeed * Time.deltaTime;

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
        Debug.Log("Enemy Attack");

        PlayerStat PlayerStat = Player.GetComponent<PlayerStat>();
        if(PlayerStat != null)
        {
            PlayerStat.TakeDamage(AttackDamage);
        }
    }

    public void Reset()
    {

    }
}
