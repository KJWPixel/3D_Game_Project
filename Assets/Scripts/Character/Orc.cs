using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : EnemyCharacter
{
    [Header("TRPATH")]
    [SerializeField] Transform[] TRPATH;
    [SerializeField] int CurrentPathIndex = 0;
    [SerializeField] float PatrolWaitTime = 0f;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        CurHp = MaxHp;
    }

    public override void Idle()
    {
        Debug.Log($"{Name} Idle");
    }

    public override void Search()
    {
        throw new System.NotImplementedException();
    }
    public override void Chase()
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
