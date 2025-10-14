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

    [SerializeField] private TMP_Text TextLevel;
    [SerializeField] private Image HpImage;
    [SerializeField] private Image HpEffect;
    [SerializeField] private Image MpImage;
    [SerializeField] private Image MpEffect;
    [SerializeField] private Image SteminaImage;
    [SerializeField] private GameObject SteminaBackGround;
    [SerializeField] private float EffectTime = 0f;

    private void Awake()
    {
        InitializedFillAmount();
    }
    void Start() 
    {

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


}
