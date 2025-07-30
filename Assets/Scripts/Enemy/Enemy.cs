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

    Animator Animator;

    [Header("이동속도")]
    [SerializeField] float MoveSpeed = 0f;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        Chase();
    }

    private void Chase()
    {
        if (AI_Enemy.PlayerChese)
        {
            Animator.SetBool("PlayerChase", true);
            Vector3 Dir = Character.transform.position - transform.position;
            transform.position += Dir.normalized * MoveSpeed * Time.deltaTime;

            if (Dir != Vector3.zero)//(Dir.sqrMagnitude > 0.0001f) dir 이 Vector3.zero 일 때 (즉, 길이가 0일 때) Quaternion.LookRotation(dir)을 하면 경고가 나옴
            {
                Quaternion TargetRotation = Quaternion.LookRotation(Dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, 5f * Time.deltaTime);
            }
        }
        else
        {
            Animator.SetBool("PlayerChase", false);
        }

        //magnitude: 벡터의 길이 (예시:Vector3(3,0,4) = 5) 루트 제곱하여 25에서 5로
        //sqrMagnitude: 벡터의 길이의 제곱 (예시:Vector3(3,0,4) = 25) 제곱만하여 25 
    }
}
