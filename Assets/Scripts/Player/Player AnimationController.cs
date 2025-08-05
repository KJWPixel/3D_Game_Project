using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }


    void Start()
    {
        
    }
  
    void Update()
    {
        
    }

    public void AnimationUpdate(float _x, float _z, float _VerticalVelocity)
    {
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
}
