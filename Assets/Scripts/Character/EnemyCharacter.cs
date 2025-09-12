using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCharacter : CharacterBase
{
    //Enemy상속에서 필요한 기본적인 정보 및 함수
    [Header("기본 정보")]
    [SerializeField] protected string Name;
    [SerializeField] protected float MaxHp;
    [SerializeField] protected float CurHp;
    [SerializeField] protected float WalkSpeed;
    [SerializeField] protected float RunningSpeed;
    [SerializeField] protected float Atk;
    [SerializeField] protected float Def;
    [SerializeField] protected float AtackRange;
    [SerializeField] protected float GainExp;

    public abstract void Init();
    public abstract void Idle();
    public abstract void Search();
    public abstract void Chase();
    public abstract void Attack();
    public abstract void TakeDamage(float _Damage);
    public abstract void ShowDamageText(float _Damage);
    
}
