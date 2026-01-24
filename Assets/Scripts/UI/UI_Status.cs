using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Character의 Status를 UI 확인
/// </summary>
public class UI_Status : MonoBehaviour
{
    PlayerStat PlayerStat;

    [Header("플레이어 UI")]
    [SerializeField] private TMP_Text TextLevel;
    [SerializeField] private Image HpImage;
    [SerializeField] private Image HpEffect;
    [SerializeField] private Image MpImage;
    [SerializeField] private Image MpEffect;
    [SerializeField] private Image SteminaImage;
    [SerializeField] private GameObject SteminaBackGround;
    [SerializeField] private float EffectTime = 0f;

    [Header("보스 UI")]
    [SerializeField] private TMP_Text BossNameText;
    [SerializeField] private Image BossHpEffectImage;
    [SerializeField] private Image BossHpImage;
    [SerializeField] private Image BossHpBackGround;
    [SerializeField] private TMP_Text BossHpText;
    [SerializeField] private GameObject BossUIPanel;
    [SerializeField] private GameObject BossUIDesign;
    [SerializeField] private float BossEffectTime = 0.5f; // 잔상이 줄어드는 속도 조절
    private Coroutine hpEffectCoroutine;

    private void Awake()
    {
        InitializedFillAmount();
    }
    void Start() 
    {
        BossUIPanel.SetActive(false);
        BossUIPanel.SetActive(false);
    }

    void Update()
    {
        UIStatsUpdate();
        HideStemina();
    
    }

    private void InitializedFillAmount()
    {
        HpImage.fillAmount = 1;
        HpEffect.fillAmount = 1;
        MpImage.fillAmount = 1;
        MpEffect.fillAmount = 1;
        SteminaImage.fillAmount = 1;
        BossHpEffectImage.fillAmount = 1;
        BossHpImage.fillAmount = 1;
        BossNameText.text = string.Empty;
    }

    public void SetStatus(PlayerStat Stats)
    {
        PlayerStat = Stats;
    }

    private void UIStatsUpdate()
    {
        TextLevel.text = PlayerStat.Level.ToString();
        float HpFill = PlayerStat.CurrentHp / PlayerStat.MaxHp;
        float MpFill = PlayerStat.CurrentMp / PlayerStat.MaxMp;
        float StaminaFill = PlayerStat.CurrentStamina / PlayerStat.MaxStamina;

        HpImage.fillAmount = HpFill;
        MpImage.fillAmount = MpFill;
        SteminaImage.fillAmount = StaminaFill;

        if (HpEffect.fillAmount > HpFill)
        {
            HpEffect.fillAmount = Mathf.Lerp(HpEffect.fillAmount, HpFill, Time.deltaTime * (1f / EffectTime));

            //Lerp를 사용하지 않는 방법
            //HpEffect.fillAmount -= (Time.deltaTime / EffectTime);
            //if(HpFill > HpEffect.fillAmount)
            //{
            //    HpEffect.fillAmount = HpFill;
            //}
        }
        else
        {
            HpEffect.fillAmount = HpFill;
        }

        if(MpEffect.fillAmount > MpFill)
        {
            MpEffect.fillAmount = Mathf.Lerp(MpEffect.fillAmount, MpFill, Time.deltaTime * (1f / EffectTime));
        }
        else
        {
            MpEffect.fillAmount = MpFill;
        }

        //스테미너 바

        //경험치 바 
    }

    private void HideStemina()
    {
        if(PlayerStat.CurrentStamina == PlayerStat.MaxStamina)
        {
            SteminaBackGround.SetActive(false);
        }
        else
        {
            SteminaBackGround.SetActive(true);
        }
    }


    // 보스 UI 초기화 및 켜기
    public void SetBossUI(string name, float current, float max)
    {
        BossUIPanel.SetActive(true);
        BossUIDesign.SetActive(true);
        BossNameText.text = name;
        UpdateBossHp(current, max);
    }

    // 보스 체력 실시간 업데이트
    public void UpdateBossHp(float current, float max)
    {
        float targetFill = current / max;

        // 1. 실제 체력 바(BossHpImage)는 즉시 변경
        BossHpImage.fillAmount = targetFill;
        BossHpText.text = $"{Mathf.Max(0, (int)current)} / {(int)max}";

        // 2. 잔상(BossHpEffectImage) 처리를 위한 코루틴 시작
        if (hpEffectCoroutine != null) StopCoroutine(hpEffectCoroutine);
        hpEffectCoroutine = StartCoroutine(SmoothHpEffect(targetFill));
    }

    public void HideBossUI() => BossUIDesign.SetActive(false);

    private IEnumerator SmoothHpEffect(float targetFill)
    {
        // 보스 체력이 깎인 뒤 아주 잠깐(예: 0.2초) 대기하면 타격감이 더 좋습니다. (선택사항)
        // yield return new WaitForSeconds(0.2f);

        // 잔상(EffectImage)이 목표(targetFill)보다 클 때만 실행
        while (BossHpEffectImage.fillAmount > targetFill)
        {
            // 요청하신 Lerp 로직 적용
            BossHpEffectImage.fillAmount = Mathf.Lerp(
                BossHpEffectImage.fillAmount,
                targetFill,
                Time.deltaTime * (1f / BossEffectTime)
            );

            // 매우 근접하면 루프 종료
            if (BossHpEffectImage.fillAmount - targetFill < 0.001f)
            {
                BossHpEffectImage.fillAmount = targetFill;
                break;
            }

            yield return null; // 다음 프레임까지 대기
        }

        // 만약 힐을 받아서 체력이 늘어난 경우라면 잔상도 즉시 맞춰줌
        if (BossHpEffectImage.fillAmount < targetFill)
        {
            BossHpEffectImage.fillAmount = targetFill;
        }
    }
}
