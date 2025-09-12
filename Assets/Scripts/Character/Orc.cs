using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : EnemyCharacter
{
    [Header("AI")]
    [SerializeField] EnemyAI EnemyAI;

    [Header("TRPATH")]
    [SerializeField] public Transform[] TRPATH;
    [SerializeField] int CurrentPathIndex = 0;
    [SerializeField] float PatrolWaitTime = 0f;

    Animator Animator;

    private void Awake()
    {
        Init();
        EnemyAI = GetComponent<EnemyAI>();  
    }

    public override void Init()
    {
        CurHp = MaxHp;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        AnimatoUpdate(EnemyAI.CurrentAI);
    }

    private void AnimatoUpdate(AI _AI)
    {
        if (Animator == null) return;

        IsIdle =_AI == AI.AI_IDLE;
        IsWalk = _AI == AI.AI_PATROL;
        IsRunning = _AI == AI.AI_CHASE;
        IsRunning = _AI == AI.AI_FLEE;
        IsAttack = _AI == AI.AI_ATTACK;

        
        Animator.SetBool("Idle", IsIdle);
        Animator.SetBool("Walking", IsWalk);
        Animator.SetBool("Running", IsRunning);
        Animator.SetBool("Attack", IsAttack);
    }

    public override void Idle()
    {
        Debug.Log($"<b><color=orange>{Name}: Idle</color></b>");
    }

    public override void Search()
    {
        
    }
    public override void Patrol()
    {
        throw new System.NotImplementedException();
    }
    public override void Chase()
    {
        throw new System.NotImplementedException();
    }
    public override void Flee()
    {
        throw new System.NotImplementedException();
    }
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage(float _Damage)
    {
        throw new System.NotImplementedException();
    }

    public override void ShowDamageText(float _Damage)
    {
        throw new System.NotImplementedException();
    }
}
