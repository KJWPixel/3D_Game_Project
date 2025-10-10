using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCharacter : CharacterBase
{
    //Enemy상속에서 필요한 기본적인 정보 및 함수
    [Header("기본 정보")]
    [SerializeField] public string name;
    [SerializeField] public int id;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float curHp;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runningSpeed;
    [SerializeField] protected float atk;
    [SerializeField] protected float def;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float gainExp;

    [Header("행동 체크")]
    [SerializeField] protected bool IsIdle;
    [SerializeField] protected bool IsWalk;
    [SerializeField] protected bool IsRunning;
    [SerializeField] protected bool IsAttack;
    [SerializeField] public bool IsDie;

    public string Name
    {
        get => name;
        set => name = value;
    }

    public float MaxHp
    {
        get => maxHp;
        set => maxHp = value;
    }

    public float CurHp
    {
        get => curHp;
        set => curHp = Mathf.Clamp(value, 0, maxHp);
    }

    public float WalkSpeed
    {
        get => walkSpeed;
        set => walkSpeed = value;
    }

    public float RunningSpeed
    {
        get => runningSpeed;
        set => runningSpeed = value;
    }

    public float Atk
    {
        get => atk;
        set => atk = value;
    }

    public float Def
    {
        get => def;
        set => def = value;
    }

    public float AttackRange
    {
        get => attackRange;
        set => attackRange = value;
    }

    public float GainExp
    {
        get => gainExp;
        set => gainExp = value;
    }

    public abstract void Init();
    public abstract void Idle();
    public abstract void Search();
    public abstract void Patrol();
    public abstract void Chase();
    public abstract void Attack();
    public abstract void Flee();
    public abstract void TakeDamage(float _Damage);
    public abstract void ShowDamageText(float _Damage);
    
}
