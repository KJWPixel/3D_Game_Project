using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Character의 Status를 UI 확인
/// </summary>
public class UI_Status : MonoBehaviour
{
    [SerializeField] GameObject Character;
    [SerializeField] PlayerStat PlayerStat;

    private TextMeshPro TextLevel;
    [SerializeField] Image HpImage;
    [SerializeField] Image HpEffect;
    [SerializeField] Image MpImage;
    [SerializeField] Image MpEffect;
    [SerializeField] Image SteminaImage;

    private void Awake()
    {
        
    }
    void Start() 
    {
        
    }

    void Update()
    {
        UIupdate();
    }

    private void UIupdate()
    {
        //PlayerStat = GetComponent<PlayerStat>();
        //HpImage.fillAmount = PlayerStat.CurrentHp / PlayerStat.MaxHp;
        //MpImage.fillAmount = PlayerStat.CurrentMp / PlayerStat.MaxMp;
        //SteminaImage.fillAmount = PlayerStat.CurrentStamina / PlayerStat.MaxStamina;
    }
}
