using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using UnityEngine;

public class Orc : EnemyCharacter
{
    [Header("AI")]
    [SerializeField] EnemyAI EnemyAI;

    [Header("TRPATH")]
    [SerializeField] public Transform[] TRPATH;
    [SerializeField] int CurrentPathIndex = 0;
    [SerializeField] float PatrolWaitTime = 0f;
    [SerializeField] float PatrolWaitStartTime= 0f;

    [Header("프리팹")]
    [SerializeField] GameObject DamageTextPrefab;
    [SerializeField] GameObject DynamicObject;

    [Header("Player")]
    [SerializeField] GameObject Player;
    [SerializeField] PlayerStat PlayerStat;
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
        if(Player == null)
        {
            Player = GameObject.FindWithTag("Player");
        }

        PlayerStat = Player.GetComponent<PlayerStat>();
    }

    private void Update()
    {
        AnimatorUpdate(EnemyAI.CurrentAI);
    }

    private void AnimatorUpdate(AI _AI)
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
        if(TRPATH == null) return;

        Transform PathPoint = TRPATH[CurrentPathIndex];
        Vector3 Dir = PathPoint.position - transform.position;//방향벡터
        float Distance = Dir.magnitude;//벡터의 길이

        if(Distance > 0.2f)
        {
            transform.position += Dir.normalized * WalkSpeed * Time.deltaTime;

            if (Dir != Vector3.zero)
            {
                //방향벡터로 정면 방향으로 회전
                Quaternion PathRotation = Quaternion.LookRotation(Dir);

                transform.rotation = Quaternion.Slerp(transform.rotation, PathRotation, 5f * Time.deltaTime);
            }

            PatrolWaitStartTime = 0f;
        }
        else
        {
            PatrolWaitStartTime += Time.deltaTime;
            if (PatrolWaitStartTime >= PatrolWaitTime)
            {
                CurrentPathIndex = (CurrentPathIndex + 1) % TRPATH.Length;
                PatrolWaitStartTime = 0f;
            }
        }

    }
    public override void Chase()
    {
        if(Player == null) return;

        Vector3 Dir = Player.transform.position - transform.position;
        transform.position += Dir.normalized * RunningSpeed * Time.deltaTime;

        if(Dir != Vector3.zero)
        {
            Quaternion TargetRotation = Quaternion.LookRotation(Dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, 5f * Time.deltaTime);
        }
    }

    public override void Flee()
    {
        Vector3 Dir = TRPATH[CurrentPathIndex].position - transform.position;
        transform.position += Dir.normalized * RunningSpeed * Time.deltaTime;
    }
    public override void Attack()
    {
        if (Player == null) return;

        float Distance = Vector3.Distance(transform.position, Player.transform.position);
        if(Distance <= 3f)
        {
            PlayerStat.TakeDamage(Atk);
        }
    }

    public override void TakeDamage(float _Damage)
    {
        float FinalDamage = 0f;

        if (CurHp > 0)
        {
            if(Def >= _Damage)
            {
                FinalDamage = 1f;
                CurHp -= FinalDamage;
            }
            else
            {
                FinalDamage = _Damage - Def;
                CurHp -= FinalDamage;
            }
        }
        else
        {
            IsDie = true;
            Die();
        }

            ShowDamageText(FinalDamage);
    }

    public override void ShowDamageText(float _Damage)
    {
        Vector3 spawnPosition = transform.position + Vector3.up * 2f;

        GameObject DamageTextInstance = Instantiate(DamageTextPrefab, spawnPosition, Quaternion.identity, DynamicObject.transform);

        DamageText DamageText = DamageTextInstance.GetComponent<DamageText>();

        if (DamageText != null)
        {
            DamageText.SetDamageText(_Damage, false);
        }
    }

    private void Die()
    {
        IsDie = true;
        PlayerStat.AddExp(GainExp);
        Destroy(gameObject);
    }
}
