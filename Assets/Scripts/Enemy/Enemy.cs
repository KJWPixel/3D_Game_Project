using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(AI_Enemy.PlayerChese)
        {
            Animator.SetBool("PlayerChase", true);
            Vector3 Dir = Character.transform.position - transform.position;          
            transform.position += Dir.normalized * MoveSpeed * Time.deltaTime;

            if(Dir != Vector3.zero)
            {
                Quaternion TargetRotation = Quaternion.LookRotation(Dir);
                Quaternion.Slerp(transform.rotation, TargetRotation, 5f * Time.deltaTime);
            }
        }
        else
        {
            Animator.SetBool("PlayerChase", false);
        }
    }
}
