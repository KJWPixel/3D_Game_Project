using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIStatusPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] StatTexts;

    public void UpdateStatusUI(PlayerStat stats)
    {
        StatTexts[1].text = stats.Level.ToString();
        StatTexts[2].text = $"{stats.CurrentHp:F0}/{stats.MaxHp:F0}";
        StatTexts[3].text = $"{stats.CurrentMp:F0}/{stats.MaxMp:F0}";
        StatTexts[4].text = stats.MaxStamina.ToString();
        StatTexts[5].text = stats.Atk.ToString();
        StatTexts[6].text = stats.Def.ToString();
        StatTexts[7].text = $"{(stats.Crit * 100):F0}%";
        StatTexts[8].text = $"{(stats.CritDmg * 100):F0}%";
    }
    
}
