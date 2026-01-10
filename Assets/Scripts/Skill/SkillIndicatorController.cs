using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillIndicatorController : MonoBehaviour
{
    [Header("인디케이트 루트")]
    [SerializeField] private Transform IndicatorRoot;

    [Header("인디케이터 스케일 루트")]
    [SerializeField] private Transform ScaleRoot;

    [Header("팬 인디케이터")]
    [SerializeField] private FanIndicator fanIndicator;

    [Header("인디케이터")]
    [SerializeField] private GameObject Arrow;
    [SerializeField] private GameObject ArrowLine;
    [SerializeField] private GameObject Circle1;
    [SerializeField] private GameObject Circle2;
    [SerializeField] private GameObject Circle3;
    [SerializeField] private GameObject Fan1;

    private IndicatorType currentIndicator = IndicatorType.None;
    private float castTime = 0f;
    private float distance = 0f;
    private float radius = 0f;
    private float length = 0f;
    private float width = 0f;
    private float targetRadius = 0f;
    private float angle = 0f;  

    private void Awake()
    {
        AllHide();
    }

    public void AllHide()
    {
        Arrow.SetActive(false);
        ArrowLine.SetActive(false);
        Circle1.SetActive(false);
        Circle2.SetActive(false);
        Circle3.SetActive(false);
        //Fan1.SetActive(false);
    }

    public void ShowIndicator(SkillData data, Transform caster)
    {
        foreach(var dataEffect in data.Effects)
        {
            castTime = data.CastTime;
            distance = dataEffect.Distance;
            radius = dataEffect.Radius;
            length = dataEffect.Length;
            width = dataEffect.Width;
            radius = dataEffect.Radius;
            angle = dataEffect.Angle;

            if (dataEffect.EffectType == SkillEffectType.RayDamage)// 문제 없음
            {
                currentIndicator = IndicatorType.Ray;

                Arrow?.SetActive (true);
                ArrowLine?.SetActive (true);

                transform.position = caster.position + Vector3.up * 0.05f;
                transform.rotation = caster.rotation;

                ScaleRoot.localScale = new Vector3(1f, 0.01f, 1f);
                Arrow.transform.localPosition = Vector3.zero;
            }
            else if(dataEffect.EffectType == SkillEffectType.LineAreaDamage)
            {
                currentIndicator = IndicatorType.LineArea;

                Circle1?.SetActive (true);

                Vector3 targetPos = caster.position + (caster.forward * distance) + (Vector3.up * 0.05f);
                IndicatorRoot.position = targetPos;

                IndicatorRoot.rotation = Quaternion.Euler(90f, caster.eulerAngles.y, 0f);

                // ▶ Scale 초기화 (아주 작게)
                ScaleRoot.localScale = Vector3.one * 0.01f;
            }
            else if(dataEffect.EffectType == SkillEffectType.DistanceAreaDamage)
            {
                currentIndicator = IndicatorType.DistanceArea;

                Circle1?.SetActive (true);

                //IndicatorRoot.position = caster.position + caster.forward * distance + Vector3.up * 0.05f;
                //IndicatorRoot.rotation = caster.rotation;

                Vector3 targetPos = caster.position + (caster.forward * distance) + (Vector3.up * 0.05f);
                IndicatorRoot.position = targetPos;

                IndicatorRoot.rotation = Quaternion.Euler(90f, caster.eulerAngles.y, 0f);

                // ▶ Scale 초기화 (아주 작게)
                //ScaleRoot.localScale = Vector3.one * 0.01f;
                ScaleRoot.localScale = new Vector3(0.01f, 0.01f, 1f);
            }
        }
    }

    public void UpdateCast(float ratio)
    {
        //Debug.Log($"UpdateCast 호출됨 ratio = {ratio}");

        switch (currentIndicator)
        {
            case IndicatorType.Ray:
                //RayDamageType
                float raySize = Mathf.Lerp(0.01f, distance, ratio);
                ScaleRoot.localScale = new Vector3(1f, raySize, 1f);
                Arrow.transform.localPosition = new Vector3(0f, raySize, 0f);
                break;

            case IndicatorType.LineArea:
                float wSize = Mathf.Lerp(0.01f, width, ratio);
                float lSize = Mathf.Lerp(0.01f, length, ratio);

                // 가로(width)는 X, 세로(length)는 Y
                ScaleRoot.localScale = new Vector3(wSize, lSize, 1f);
                break;  

            case IndicatorType.DistanceArea:
                //DistanceAreaType
                float size = Mathf.Lerp(0.01f, radius * 2f, ratio);
                // X, Y만 키운다 (바닥 원)
                ScaleRoot.localScale = new Vector3(size, size, 1f);
                break;
        } 
    }
}
