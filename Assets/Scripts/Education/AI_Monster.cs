using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AI_Monster : MonoBehaviour
{
    protected AI AI = AI.AI_CREATE;

    public Monster Monster;

    public void Init(Monster _Character)
    {
        Monster = _Character;
    }

    public void Stat()
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
        AI = AI.AI_MOVE;
    }
    protected virtual void Move()
    {
        AI = AI.AI_SEARCH;
    }
    protected virtual void Reset()
    {
        AI = AI.AI_CREATE;
    }
}
