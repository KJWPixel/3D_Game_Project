using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] public int Level = 0;
    [SerializeField] public float MaxHp = 0f;
    [SerializeField] public float CurrentHp = 0f;
    [SerializeField] public float MaxMp = 0f;
    [SerializeField] public float CurrentMp = 0f;
    [SerializeField] public float MaxStamina = 0f;
    [SerializeField] public float CurrentStamina = 0f;

    [SerializeField] public float Atk = 0f;

    void Update()
    {
        StatInit();
        NaturalRecovery();
    }

    private void StatInit()
    {
        if (CurrentHp > MaxHp)
        {
            CurrentHp = MaxHp;           
        }
        if(CurrentMp > MaxMp)
        {
            CurrentMp = MaxMp;
        }
        if (CurrentStamina > MaxStamina)
        {
            CurrentStamina = MaxStamina;
        }
    }

    private void NaturalRecovery()
    {
        if(CurrentMp < MaxHp)
        {
            CurrentMp += 0.1f * Time.deltaTime;
        }

        if (CurrentStamina < MaxStamina)
        {
            CurrentStamina += 0.1f * Time.deltaTime;
        }
    }

    

    public void TakeDamage(float _Damage)
    {
        CurrentHp -= _Damage;
    }
}
