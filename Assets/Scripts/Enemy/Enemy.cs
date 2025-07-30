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

    Animator Animator;

    [Header("�̵��ӵ�")]
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

            if (Dir != Vector3.zero)//(Dir.sqrMagnitude > 0.0001f) dir �� Vector3.zero �� �� (��, ���̰� 0�� ��) Quaternion.LookRotation(dir)�� �ϸ� ��� ����
            {
                Quaternion TargetRotation = Quaternion.LookRotation(Dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, 5f * Time.deltaTime);
            }
        }
        else
        {
            Animator.SetBool("PlayerChase", false);
        }

        //magnitude: ������ ���� (����:Vector3(3,0,4) = 5) ��Ʈ �����Ͽ� 25���� 5��
        //sqrMagnitude: ������ ������ ���� (����:Vector3(3,0,4) = 25) �������Ͽ� 25 
    }
}
