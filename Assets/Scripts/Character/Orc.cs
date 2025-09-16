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
        Animator = GetComponent<Animator>();
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

        int State = _AI switch
        {
            AI.AI_CREATE => 0,
            AI.AI_IDLE => 1,
            AI.AI_SEARCH => 2,
            AI.AI_PATROL => 3,
            AI.AI_CHASE => 4,
            AI.AI_FLEE => 5,
            AI.AI_ATTACK => 6,
            AI.AI_DEAD => 7,
        };

        Animator.SetInteger("State", State);
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
