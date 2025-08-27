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


    void Start()
    {
        
    }
  
    void Update()
    {
        
    }

    public void AnimationUpdate(float _x, float _z, float _VerticalVelocity)
    {
        //if(PlayerController.CanMove == false)
        //{
        //    Debug.Log("Animation ÇÔ¼ö Return");
        //    Animator.SetBool("IsWalk", false);
        //    Animator.SetFloat("xDir", 0f);
        //    Animator.SetFloat("yDir", 0f);
        //    Animator.SetFloat("zDir", 0f);
        //    return;
        //}

        bool IsWaking = _x != 0 || _z != 0;
        Animator.SetBool("IsWalk", IsWaking);
        Animator.SetFloat("xDir", _x);
        Animator.SetFloat("zDir", _z);
        Animator.SetFloat("yDir", _VerticalVelocity);
    }

    public void SetDash(bool _IsDash)
    {
        Animator.SetBool("IsDash", _IsDash);
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
                case SkillEffectType.Damage:
                    Animator.SetBool("IsDamage", _IsPlayering);
                    break;

                case SkillEffectType.Heal:
                    Animator.SetBool("IsHeal", _IsPlayering);
                    break;

                case SkillEffectType.AtkBuff:
                case SkillEffectType.DefBuff:
                case SkillEffectType.CriBuff:
                case SkillEffectType.TotalBuff:
                    Animator.SetBool("IsBuff", _IsPlayering);
                    break;

                case SkillEffectType.Debuff:
                    Animator.SetBool("IsDebuff", _IsPlayering);
                    break;

                case SkillEffectType.CC:
                    Animator.SetBool("IsCC", _IsPlayering);
                    break;

                case SkillEffectType.MoveMent:
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
