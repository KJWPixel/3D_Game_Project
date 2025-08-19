using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Skill : MonoBehaviour
{
    [SerializeField] GameObject SkillWindow;
    [SerializeField] TextMeshProUGUI SkillPointText;

    bool IsActive = false;

    PlayerStat PlayerStat;

    private void Awake()
    {
        
    }
    void Start()
    {
        PlayerStat = FindAnyObjectByType<PlayerStat>();
        
    }

    
    void Update()
    {
        ActiveUI();
        SkillPointUpdate();
    }

    private void ActiveUI()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            IsActive = !IsActive;
            if(IsActive)
            {
                SkillWindow.SetActive(true);
            }
            else
            {
                SkillWindow.SetActive(false);
            }
        }
    }

    private void SkillPointUpdate()
    {
        SkillPointText.text = PlayerStat.SkillPoint.ToString();

    }
}
