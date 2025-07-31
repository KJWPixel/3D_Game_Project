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

    [Header("���ݷ�")]
    [SerializeField] float AttackDamage = 0f;

    [Header("�̵��ӵ�")]
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
