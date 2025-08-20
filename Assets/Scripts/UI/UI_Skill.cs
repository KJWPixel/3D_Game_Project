using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skill : MonoBehaviour
{
    [SerializeField] GameObject SkillWindow;
    [SerializeField] TextMeshProUGUI SkillPointText;
    [SerializeField] GameObject AcquisitionImage;

    bool IsActive = false;

    PlayerStat PlayerStat;

    private void Awake()
    {
        InitSkillWindow();
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
    private void InitSkillWindow()
    {
        SkillWindow.SetActive(false);
        AcquisitionImage.SetActive(false);
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

    public void SkillButton()
    {
        Debug.Log("��ų ����");
        AcquisitionImage.SetActive(true);
    }
}
