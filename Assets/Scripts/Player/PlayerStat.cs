using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;


[System.Serializable]
public class Status
{
    public StatusType StatusType;
    public float Value;
}

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat Instance;

    [Header("Status")]
    [SerializeField] private string userName;
    [SerializeField] private int level = 1;
    [SerializeField] private float maxExp = 0f;
    [SerializeField] private float currentExp = 0f;
    [SerializeField] private int skillPoint = 0; 

    [SerializeField] private float maxHp = 0f;
    [SerializeField] private float currentHp = 0f;
    [SerializeField] private float maxMp = 0f;
    [SerializeField] private float currentMp = 0f;
    [SerializeField] private float maxStamina = 0f;
    [SerializeField] private float currentStamina = 0f;

    [SerializeField] private float atk = 0f;
    [SerializeField] private float def = 0f;
    [SerializeField] private float crit = 0f;
    [SerializeField] private float critDmg = 0f;

    [SerializeField] private int gold;

    //public float GetStatus(StatusType _Type)
    //{
    //    return PlayerStatus[_Type];
    //}

    //public float SetStatus(StatusType _Type, float _Value)
    //{
    //    return PlayerStatus[_Type] = _Value;
    //}

    public string UserName
    {
        get => userName;
        set => userName = value;
    }

    public int Level
    {
        get => level;
        set => level = value;
    }

    public float MaxExp
    {
        get => maxExp;
        set => maxExp = value;
    }

    public float CurrentExp
    {
        get => currentExp;
        set => currentExp = Mathf.Clamp(value, 0, maxExp);
    }

    public int SkillPoint
    {
        get => skillPoint;
        set => skillPoint = value;
    }

    public float MaxHp
    {
        get => maxHp;
        set => maxHp = value;
    }

    public float CurrentHp
    {
        get => currentHp;
        set => currentHp = Mathf.Clamp(value, 0, maxHp);
    }

    public float MaxMp
    { 
        get => maxMp;
        set => maxMp = value;
    }

    public float CurrentMp
    {
        get => currentMp;
        set => currentMp = Mathf.Clamp(value, 0, maxMp);
    }

    public float MaxStamina
    {
        get => maxStamina;
        set => maxStamina = value;
    }

    public float CurrentStamina
    {
        get => currentStamina;
        set => currentStamina = Mathf.Clamp(value, 0, maxStamina);
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

    public float Crit
    {
        get => crit;
        set => crit = value;
    }

    public float CirtDmg
    {
        get => critDmg;
        set => critDmg = value;
    }

    public int Gold
    {
        get => gold;
        set => gold = value;
    }


    List<float> ActiveBuffs = new List<float>();

    PlayerController PlayerController;
    SkillManager SkillManager;
    UI_Status status;

    private string FilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

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
        if (CurrentMp > MaxMp)
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
        if (CurrentMp < MaxHp)
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
        //스테미너 감소
        CurrentStamina -= _Amount;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);
    }

    public bool ConsumeMp(float _Amount)
    {
        //MP감소
        if (CurrentMp < _Amount) return false;
        CurrentMp -= _Amount;
        return true;
    }

    public bool ConsumeSp(int _Amount)
    {
        //SP 감소
        if (SkillPoint < _Amount) return false;
        SkillPoint -= _Amount;
        return true;
    }

    public void RecoveryStat(ConsumableType _Type, float _Amount)
    {
        switch (_Type)
        {
            case ConsumableType.ResotreHp:
                CurrentHp += Mathf.Min(CurrentHp + _Amount, MaxHp);
                Debug.Log($"플레이어의 Hp가 {_Amount}만큼 회복하였습니다. {CurrentHp}/{MaxHp}");
                break;
            case ConsumableType.ResotreMp:
                CurrentMp += Mathf.Min(CurrentMp + _Amount, MaxMp);
                Debug.Log($"플레이어의 Mp가 {_Amount}만큼 회복하였습니다. {CurrentMp}/{MaxMp}");
                break;
            case ConsumableType.ResotreStamina:
                CurrentStamina += Mathf.Min(CurrentStamina + _Amount, MaxStamina);
                Debug.Log($"플레이어의 Stamina가 {_Amount}만큼 회복하였습니다. {CurrentStamina}/{MaxStamina}");
                break;
        }
    }

    public void ApplyStat(ItemStatus _ItemStatus, float _Value)
    {
        switch (_ItemStatus)
        {
            case ItemStatus.Atk:
                Atk += _Value;
                break;
            case ItemStatus.Def:
                Def += _Value;
                break;
            case ItemStatus.Crit:
                Crit += _Value;
                break;
            case ItemStatus.CritDmg:
                CirtDmg += _Value;  
                break;
        }
    }

    public void RemoveStat(ItemStatus _ItemStatus, float _Value)
    {
        switch (_ItemStatus)
        {
            case ItemStatus.Atk:
                Atk -= _Value;
                break;
            case ItemStatus.Def:
                Def -= _Value;
                break;
            case ItemStatus.Crit:
                Crit -= _Value;
                break;
            case ItemStatus.CritDmg:
                CirtDmg -= _Value;
                break;
        }
    }

    public void TakeDamage(float _Damage)
    {
        if (_Damage <= Def)
        {
            return;
        }

        CurrentHp -= (_Damage - Def);
    }

    public void AddExp(float _Exp)
    {
        CurrentExp += _Exp;

        if (CurrentExp > MaxExp)
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
