using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] float MaxHp = 0f;
    [SerializeField] float CurrentHp = 0f;

    void Update()
    {
        
    }

    public void TakeDamage(float _Damage)
    {
        CurrentHp -= _Damage;
    }
}
