using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
public class Status
{
    public StatusType StatusType;
    public float Value;
}

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat Instance;
    [SerializeField] bool test = false;

    [Header("Status")]
    [SerializeField] private string userName;
    [SerializeField] private int level = 1;
    [SerializeField] private float maxExp = 1000f;
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

    [SerializeField] private int currentgold;
    [SerializeField] private PostProcessVolume postProcessVolume;
    [SerializeField] private Vignette vignette;

    [SerializeField] public float posX;
    [SerializeField] public float posY;
    [SerializeField] public float posZ;

    [Header("플레이어 상태")]
    [SerializeField] private bool isDie = false;

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

    public float CritDmg
    {
        get => critDmg;
        set => critDmg = value;
    }

    public int Gold
    {
        get => currentgold;
        set => currentgold = value;
    }


    List<float> ActiveBuffs = new List<float>();

    PlayerController playerController;
    PlayerAnimationController playerAniController;
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

        playerController = GetComponent<PlayerController>();
        playerAniController = GetComponent<PlayerAnimationController>();
        SkillManager = GetComponent<SkillManager>();


    }

    private void Start()
    {
        UI_Status Ui = FindAnyObjectByType<UI_Status>();
        if (Ui != null)
        {
            Ui.SetStatus(this);
        }
        StatInit();
        PostProcessInit();
    }
    void Update()
    {    
        NaturalRecovery();
        DamageEffect(); 
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

    private void PostProcessInit()
    {
        postProcessVolume.profile.TryGetSettings(out vignette);

        vignette.intensity.value = 0f;
        vignette.smoothness.value = 0f;
        vignette.roundness.value = 0f;  
    }

    private void NaturalRecovery()
    {
        //HP, MP, Stemina 자동회복
        if (CurrentMp < MaxMp)
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
                CurrentHp = Mathf.Min(CurrentHp + _Amount, MaxHp);
                Debug.Log($"플레이어의 Hp가 {_Amount}만큼 회복하였습니다. {CurrentHp}/{MaxHp}");
                break;
            case ConsumableType.ResotreMp:
                CurrentMp = Mathf.Min(CurrentMp + _Amount, MaxMp);
                Debug.Log($"플레이어의 Mp가 {_Amount}만큼 회복하였습니다. {CurrentMp}/{MaxMp}");
                break;
            case ConsumableType.ResotreStamina:
                CurrentStamina = Mathf.Min(CurrentStamina + _Amount, MaxStamina);
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
                CritDmg += _Value;  
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
                CritDmg -= _Value;
                break;
        }
    }

    public (float damage, bool isCrit) CalculateFinalDamage(float skillPower, float targetDef)
    {
        // 크리티컬 판정
        bool isCrit = Random.value <= this.crit;

        // 데미지 계산
        float baseDamage = this.atk * skillPower;

        // 크리티컬 데미지 계산
        if (isCrit) baseDamage *= this.critDmg;

        // 방어력 적용
        float finalDamage = Mathf.Max(1, baseDamage - targetDef);

        return (finalDamage, isCrit);
    }

    public void TakeDamage(float _Damage)
    {
        if (test) return;

        // 계산된 데미지가 1보다 작으면 1로 고정
        float finalDamage = Mathf.Max(1f, _Damage - Def);

        CurrentHp -= finalDamage;

        if (vignette != null) // Post-Processing Vignette 효과 
        {
            vignette.roundness.value = 0.85f;
        }

        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
            isDie = true;
            Die();
        }
    }

    private void DamageEffect() // 피격 Post-Processing 효과
    {
        if (vignette == null) return;

        float targetRoundness = 0f;

        if(CurrentHp <= MaxHp * 0.3f)
        {
            targetRoundness = 0.9f;
        }
        else
        {
            targetRoundness = 0f;
        }
        vignette.roundness.value = Mathf.Lerp(vignette.roundness.value, targetRoundness, Time.deltaTime * 5f);
    }

    public void Die()
    {
        playerAniController.PlayerDieAnimation(true);
        UIManager.Instance.ShowResurrectionPanel();
        playerController.OnPlayerDie();
    }

    public void Resurrect()
    {
        isDie = false;
        CurrentHp = MaxHp;
        playerAniController.PlayerDieAnimation(false);
    }

    public void AddGold(int gold)
    {
        currentgold += gold;
    }

    public void AddExp(float exp)
    {
        CurrentExp += exp;

        // if 대신 while을 사용하여 연속 레벨업 가능하게 수정
        while (CurrentExp >= MaxExp)
        {
            CurrentExp -= MaxExp; // 초과 경험치 보존
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;

        // 능력치 상승 로직
        MaxExp *= 1.15f;
        MaxHp *= 1.1f;
        MaxMp *= 1.4f;
        Atk *= 1.1f;
        Def++;

        // 레벨업 시 체력/마나 회복 (선택 사항)
        CurrentHp = MaxHp;
        CurrentMp = MaxMp;

        SoundManager.Instance.PlaySystemSFX(SFXType.LevelUp);

        Debug.Log($"레벨업! 현재 레벨: {Level}");
    }
}
