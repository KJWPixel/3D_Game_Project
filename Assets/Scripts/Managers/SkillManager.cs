using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;    

    PlayerStat PlayerStat;
    PlayerController PlayerController;
    PlayerAnimationController Anim;



    //스킬별 쿨타임 시간 
    private Dictionary<SkillData, float> SkillCoolDownTimers = new Dictionary<SkillData, float>();


    /* 스킬 동작 SkillManger
     * 스킬을 사용할 떄 실행되는 로직
     * 쿨타임 체크
     * 지원 소모 체크
     * 타겟 지정
     * 애니메이션 실행
     * 이펙트 생성
     * 데미지/효과 적용
    */

    private void Awake()
    {
        PlayerStat = GetComponent<PlayerStat>();      
        Anim = GetComponent<PlayerAnimationController>();
        PlayerController = GetComponent<PlayerController>();
    }

    public void UseSkill(SkillData _Skill, Transform _Target = null)
    {
        if (!CanUse(_Skill)) return;
        StartCoroutine(CastSkill(_Skill, _Target));
    }


    private bool CanUse(SkillData _Skill)
    {
        if (PlayerStat == null)
        {
            return false;
        }

        //MP 체크
        if(PlayerStat.CurrentMp < _Skill.Cost)
        {
            return false;
        }

        //쿨타임 체크
        if(SkillCoolDownTimers.ContainsKey(_Skill))
        {
            if(Time.time < SkillCoolDownTimers[_Skill])
            {
                return false;
            }
        }
        
        return true;
    }

    private IEnumerator CastSkill(SkillData _Skill, Transform _Target)
    {
        if(!PlayerStat.ConsumeMp(_Skill.Cost))
        {
            //MP부족 시 false 코루틴 중지
            yield break;
        }

        //이동 잠금 
        PlayerController.SetState(PlayerState.Casting);

        //애니메이션 재생 
        Anim.PlayerSkillAnimation(_Skill.type, true);
        
        //캐스팅 시간
        yield return new WaitForSeconds(_Skill.CastTime);

        //효과에 대한 처리 부분(데미지, 회복, 버프 적용)
        if(_Skill.type == SkillType.Damage)//스킬타입이 Damage면 
        {
            //Skill.Type Damage 처리 
        }

        if(_Skill.type == SkillType.Heal)//스킬타입이 Heal이면 
        {
            PlayerStat.Heal(_Skill.Power);
        }

        if(_Skill.type == SkillType.Buff)
        {
            //SKill.Type Buff 처리
        }

        //스킬이펙트 Prefab 생성 
        if(_Skill.EffectPrefab != null)
        {
            if(_Skill.type == SkillType.Heal)
            {
                GameObject EffectInstance = Instantiate(_Skill.EffectPrefab, PlayerStat.transform.position, Quaternion.identity);
                Destroy(EffectInstance, 1f);
            }
            else if(_Target != null)
            {
                GameObject EffectInstance = Instantiate(_Skill.EffectPrefab, _Target.transform.position, Quaternion.identity);
            }
        }

        //
        PlayerController.SetState(PlayerState.Idle);
        //애니메이션 종료
        Anim.PlayerSkillAnimation(_Skill.type, false);

        SkillCoolDownTimers[_Skill] = Time.time + _Skill.Cooldown;
    }


    /* 
     * 스킬 발동 흐름
     * 1. 플레이어 입력: 키보드, 마우스 > SkillManager 호출
     * 2. 사용 가능 여부 확인: 쿨타임, MP, 타겟 유효
     * 3. 시전(cast)과정: 애니메이션 생성, 이펙트 준비
     * 4. 효과적용: 타겟에게 데미지, 회복, 상태이상 부여
     * 5. 쿨타임 시작
     * 
     * 
     * 스킬 타입별 처리
     * 단일 타겟 스킬 > 한 대상만 지정해 적용
     * 광역스킬 > 범위 내 모든적 적용
     * 채널링스킬 > 일정 시간 동안 지속 발동
     * 버프/디퍼브 > 능력치 수정, 일정 시간 후 해제
     * 타입별 로직은 SkillType enum과 switch문 또는 상속/인터페이스를 활용해 분리합니다.
     * 
     * 스킬 데이터를 ScriptableObject로 관리 > 수정없이 밸런스 조정 가능
     * 쿨타임, 자원소모는 클라이언트 + 서버에서 검증
     * 애니메이션 이벤트로 타이밍 맞춰 데미지 적용
     * 범위 스킬은 Physics.OverlapSphere같은 함수를 사용
     * 효과(데미지,힐 버프)는 StatSystem이나 SatusEffectManager를 통해 적용
     * 
     * 
     * 예시
     * 플레이어가 "FireBall" 시전
     * SkillManager.UseSkill(FireBall, Target)
     * CanUse 체크 (쿨타임,MP)
     * 애니메이션 재생 & 캐스팅 시간 대기
     * FireBall Prefab생성
     * 충돌 시 데미지 계산 & 타겟 체력 감소 
     * 쿨타임 시작
    */
}
