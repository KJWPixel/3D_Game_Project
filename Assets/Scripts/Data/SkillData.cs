using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/New Skill")]
public class SkillData : ScriptableObject
{
    /*  스킬데이터 SkillData
     *  스킬의 정보(파라미터)만 담고, 실행로직 X
     *  스킬이름, 아이콘, 설명
     *  쿨타임
     *  소모자원
     *  사거리, 범위, 시전시간
     *  효과 타입(피해, 회복, 버프, 디버프 등)
     *  애니메이션/이펙트 프리팹 참조
    */

    public string SkillName;
    public Sprite Icon;
    public float Cooldown;
    public float Cost;
    public float Range;
    public float CastTime;
    public GameObject EffectPrefab;
    public SkillType type;
    public float Power;
}

public enum SkillType
{
    Damage,
    Heal,
    Buff,
    Debuff
}
