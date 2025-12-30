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

    [Header("인디케이터")]
    [SerializeField] private GameObject Arrow;
    [SerializeField] private GameObject ArrowLine;
    [SerializeField] private GameObject Circle1;
    [SerializeField] private GameObject Circle2;
    [SerializeField] private GameObject Circle3;

    private float elapsedTime = 0f;
    private float castTime = 0f;
    private float distance = 0f;
    private float radius = 0f;  

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
    }

    public void ShowIndicator(SkillData data, Transform caster)
    {
        foreach(var dataeffect in data.Effects)
        {
            castTime = data.CastTime;
            distance = dataeffect.Distance;
            radius = dataeffect.Radius;

            if (dataeffect.EffectType == SkillEffectType.RayDamage)
            {
                Arrow?.SetActive (true);
                ArrowLine?.SetActive (true);

                transform.position = caster.position + Vector3.up * 0.05f;
                transform.rotation = caster.rotation;

                ScaleRoot.localScale = new Vector3(1f, 0.01f, 1f);
                Arrow.transform.localPosition = Vector3.zero;
            }
        }
    }

    public void UpdateCast(float ratio)
    {
        Debug.Log($"UpdateCast 호출됨 ratio = {ratio}");

        float length = Mathf.Lerp(0.01f, distance, ratio);

        ScaleRoot.localScale = new Vector3(1f, length, 1f);
        Arrow.transform.localPosition = new Vector3(0f, length, 0f);
    }
}
