using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AI_Monster : MonoBehaviour
{
    public Transform[] TRPATH;

    protected AI AI = AI.AI_CREATE;

    public Monster Monster;
    public Character Character;

    bool CharacterMove = false;

    int Index = 0;
    public void Init(Monster _Character)
    {
        Monster = _Character;
    }

    public void State()
    {
        switch (AI)
        {
            case AI.AI_CREATE:
                Create();
                break;
            case AI.AI_SEARCH:
                Search();
                break;
            case AI.AI_CHASE:
                Move();
                break;
            case AI.AI_RESET:
                Reset();
                break;
        }
    }

    protected virtual void Create()
    {
        AI = AI.AI_SEARCH;
    }
    protected virtual void Search()
    {
        float dis = Vector3.Distance(Monster.transform.position, TRPATH[Index].position);
        //길찾기

        if (dis > 3f)
        {
            CharacterMove = true;
        }
        else
        {
            CharacterMove = false;
        }

        if (!CharacterMove)
        {
            dis = Vector3.Distance(Monster.transform.position, TRPATH[Index].position);
            //길찾기

            if (dis < 0.1f)
            {
                if (TRPATH.Length - 1 > Index)
                    Index++;
                else
                    Index = 0;
            }
        }

        AI = AI.AI_CHASE;
        //적찾기
        //방황하기
    }
    protected virtual void Move()
    {
        if (!CharacterMove)
        {
            transform.LookAt(TRPATH[Index].position);

            Monster.Move(TRPATH[Index].position);//목표지점 이동
        }
        else
        {
            transform.LookAt(Character.transform.position);

            float dis = Vector3.Distance(Monster.transform.position, Character.transform.position);

            if(dis < 3f)
            {
                Monster.Move(Character.transform.position);
            }
            else
            {
                Attack();
            }       
        }

        AI = AI.AI_SEARCH;
    }

    void Attack()
    {
        //공격처리 
        //1. 애니메이션 충돌 콜리전을 이용한 직접충돌 방법
        //2. 범위는 거리로 체크, 캐릭터가 이미 타켓이므로 애니메이션 시(공격거리 안에 있을 떄) hp 감소
        //(이벤투 추가, 캐릭터 가져와서 감소)

        Monster.SetAnimation("Attack");
    }


    protected virtual void Reset()
    {
        AI = AI.AI_CREATE;
    }
}
