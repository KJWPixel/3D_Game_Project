using System;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    protected Animator ANIMATOR;
    [SerializeField]
    protected SpriteRenderer SR;

    public AI_Monster AI;

    string CurAni = "";

    void Start()
    {
        //float hp = Def - Character.Instance.Atk;
    }

    private void FixedUpdate()
    {
        //AI.State();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(Vector3.up);

            SetAnimation("Run");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector3.down);

            SetAnimation("Run");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector3.left);

            SR.flipX = false;

            SetAnimation("Run");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector3.right);

            SR.flipX = true;

            SetAnimation("Run");
        }
    }
    public void Move(Vector3 _Move)
    {

        //transform.position += _Move;
        transform.position = Vector3.MoveTowards(transform.position, _Move, 0.1f);
    }
    public void SetAnimation(string _Ani, SKILL _Skill = SKILL.END)
    {
        if (CurAni == _Ani) 
            return;
        ANIMATOR.SetTrigger(_Ani);
    }

    public void OnAnimationStart(string _Ani)
    {
        Debug.Log("OnAnimationStart : " + _Ani);
    }

    public void OnAnimationIng(string _Ani)
    {
        Debug.Log("OnAnimationIng : " + _Ani);
    }

    public void OnAnimationEnd(string _Ani)
    {
        Debug.Log("OnAnimationEnd : " + _Ani);
    }
}

