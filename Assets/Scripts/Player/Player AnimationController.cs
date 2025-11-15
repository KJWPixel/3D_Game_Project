using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator Animator;
    PlayerController PlayerController;
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        PlayerController = GetComponent<PlayerController>();
    }

    public void AnimationUpdate(float _x, float _z, float _VerticalVelocity)
    {
        bool IsWaking = _x != 0 || _z != 0;
        bool IsRunning = PlayerController.IsRunning;
        Animator.SetBool("IsWalk", IsWaking);
        Animator.SetBool("IsRunning", IsRunning);
        Animator.SetFloat("xDir", _x);
        Animator.SetFloat("zDir", _z);
        Animator.SetFloat("yDir", _VerticalVelocity);
    }

    public void SetAttack(bool _IsAttack)
    {
        Animator.SetBool("IsAttack", _IsAttack);
    }

    public void PlayerSkillAnimation(List<SkillEffect> _Effects, bool _IsPlayering)
    {
        foreach (var Effect in _Effects)
        {
            switch (Effect.EffectType)
            {
                case SkillEffectType.RayDamage:
                    Animator.SetBool("IsRayDamageSkill", _IsPlayering);
                    break;
                case SkillEffectType.LineAreaDamage:
                    Animator.SetBool("IsLineAreaDamageSkill", _IsPlayering);
                    break;
                case SkillEffectType.TargetAreaDamage:
                    Animator.SetBool("IsTargetAreaDamageSkill", _IsPlayering);
                    break;
                case SkillEffectType.DistanceAreaDamage:
                    Animator.SetBool("IsDistanceDamageSkill", _IsPlayering);
                    break;

                case SkillEffectType.Heal:
                    Animator.SetBool("IsHeal", _IsPlayering);
                    break;

                case SkillEffectType.AtkBuff:
                case SkillEffectType.DefBuff:
                case SkillEffectType.CriBuff:
                case SkillEffectType.TotalBuff:
                case SkillEffectType.HealBuff:
                case SkillEffectType.MpBuff:
                    Animator.SetBool("IsBuff", _IsPlayering);
                    break;

                case SkillEffectType.Debuff:
                    Animator.SetBool("IsDebuff", _IsPlayering);
                    break;

                case SkillEffectType.CC:
                    Animator.SetBool("IsCC", _IsPlayering);
                    break;

                case SkillEffectType.Movement:
                    Animator.SetBool("IsMovement", _IsPlayering);
                    break;
                case SkillEffectType.Teleport:
                    Animator.SetBool("IsTeleport", _IsPlayering);
                    break;
                default:
                    Debug.LogWarning("Unknown SkillEffectType: " + Effect.EffectType);
                    break;
            }
        }
    }

    public void PlayerSkillAnimation(string _AnimationName, bool _IsPlayering)
    {
        Animator.SetBool(_AnimationName, _IsPlayering);
    }
}
