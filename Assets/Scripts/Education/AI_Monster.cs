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
            case AI.AI_MOVE:
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
        //��ã��

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
            //��ã��

            if (dis < 0.1f)
            {
                if (TRPATH.Length - 1 > Index)
                    Index++;
                else
                    Index = 0;
            }
        }

        AI = AI.AI_MOVE;
        //��ã��
        //��Ȳ�ϱ�
    }
    protected virtual void Move()
    {
        if (!CharacterMove)
        {
            Monster.Move(TRPATH[Index].position);//��ǥ���� �̵�
        }
        else
        {
            Monster.Move(Character.transform.position);
        }

        AI = AI.AI_SEARCH;
    }
    protected virtual void Reset()
    {
        AI = AI.AI_CREATE;
    }
}
