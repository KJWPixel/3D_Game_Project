using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStat : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] public int Level = 1;
    [SerializeField] public float MaxExp = 0f;
    [SerializeField] public float CurrentExp = 0f;
    [SerializeField] public int SkillPoint = 0;

    [SerializeField] public float MaxHp = 0f;
    [SerializeField] public float CurrentHp = 0f;
    [SerializeField] public float MaxMp = 0f;
    [SerializeField] public float CurrentMp = 0f;
    [SerializeField] public float MaxStamina = 0f;
    [SerializeField] public float CurrentStamina = 0f;

    [SerializeField] public float Atk = 0f;
    [SerializeField] public float Def = 0f;
    [SerializeField] public float Critical = 0f;

    PlayerController PlayerController;
    SkillManager SkillManager;
    UI_Status status;

    private void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
        SkillManager = GetComponent<SkillManager>();
    }

    private void Start()
    {
        UI_Status Ui = FindAnyObjectByType<UI_Status>();
        if (Ui != null)
        {
            Ui.SetStatus(this);
        }
    }

    void Update()
    {
        StatInit();
        NaturalRecovery();
    }

    private void StatInit()
    {
        //현재 스탯이 Max스탯보다 크면 Max스탯으로 변경
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
        //HP, MP, Stemina 자동회복
        if(CurrentMp < MaxHp)
        {
            CurrentMp += 0.1f * Time.deltaTime;
        }

        if (CurrentStamina < MaxStamina)
        {
            CurrentStamina += 2 * Time.deltaTime;
        }
    }

    public void ReduceStamina(float _Amount)
    {
        CurrentStamina -= _Amount;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);
    }

    public bool ConsumeMp(float _Amount)
    {
        if(CurrentMp < _Amount) return false;
        CurrentMp -= _Amount;
        return true;
    }

    public void Heal(float _Amount)
    {
        CurrentHp = Mathf.Min(CurrentHp + _Amount, MaxHp);
    }


    public void TakeDamage(float _Damage)
    {
        if(_Damage <= Def)
        {
            return;
        }

        CurrentHp -= (_Damage - Def);
    }



    public void AddExp(float _Exp)
    {
        CurrentExp += _Exp;

        if(CurrentExp > MaxExp)
        {
            float overExp = CurrentHp - MaxExp;
            CurrentHp = overExp;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;
        MaxExp *= 1.2f;
        MaxHp++;
        Atk++;
        Def++;
    }
}
