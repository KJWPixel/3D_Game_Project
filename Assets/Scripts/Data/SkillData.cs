using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/New Skill")]
public class SkillData : ScriptableObject
{
    #region
    /*  스킬데이터 SkillData
     *  스킬의 정보(파라미터)만 담고, 실행로직 X
     *  스킬이름, 아이콘, 설명
     *  쿨타임, 소모자원, 사거리, 범위, 시전시간
     *  효과 타입(피해, 회복, 버프, 디버프 등)
     *  애니메이션/이펙트 프리팹 참조
    */
    #endregion

    [Header("기본 정보")]
    public int SkillID;
    public string SkillKey;
    public string SkillDescriptionKey;
    public string SkillRequirementsKey;
    public string SkillName; 
    public Sprite Icon;
    public float Cost;
    public float CastTime;
    public float Cooldown;
    public float CastPrefabDuration;
    public float HitPrefabDuration;
    public GameObject CastEffectPrefab;
    public GameObject HitEffectPrefab;

    [Header("사운드 효과")]
    public AudioClip CastSFX; // 스킬 시전/발동 시 
    public AudioClip HitSFX;  // 스킬 적중 시 

    [Header("효과들 (여러 개 가능)")]
    public List<SkillEffect> Effects = new List<SkillEffect>();
    //List 추가해서 Type을 2개 이상

    [Header("습득 조건")]
    public int RequireLevel;
    public int RequireSP;
    public List<SkillData> PrerequisiteSkills;

    [Header("스킬 레벨")]
    public int MaxLevel = 1; //최대 레벨

    [Header("애니메이션")]
    public string AnimationName;

    public object[] GetDescriptionParams()
    {
        // 효과가 없다면 빈 배열 반환
        if(Effects == null || Effects.Count == 0) return new object[0];
        var main = Effects[0];

        return new object[] { main.Power, main.HitCount, main.Duration, main.Distance, };
    }

    public object[] GetRequirementParams()
    {
        float power = Effects.Count > 0 ? Effects[0].Power : 0;
        return new object[] { RequireLevel, RequireSP, Cooldown, power };
    }
}

[System.Serializable]
public class SkillEffect
{
    public SkillEffectType EffectType;
    public float Power;    //데미지, 회복 수치
    // 연산 최적화를 위한 런타임 전용 프로퍼티 (곱하기 정뇽)
    public float PowerMultiplier => Power * 0.01f;
    public int HitCount;
    public float Duration; //버프, 디버프, CC기 지속시간
    public float Distance; //거리
    //DistanceAraeType
    public float Radius;   //범위 공격 반경
    //LineAreaType
    public float Length;
    public float Width;
    //FanArea
    public float Angle;
    public float DelayTime;
    public int MaxTarget;   
}

