
public class AI_Base
{
    protected AI AI = AI.AI_CREATE;

    protected Character Character;

    public void Init(Character _Character)
    {
        Character = _Character;
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
        //연출효과: 서버의 시간, 무적상태
        AI = AI.AI_SEARCH;
    }
    protected virtual void Search()
    {
        //길찾기, 적찾기, 방황 등등
        AI = AI.AI_MOVE;
    }
    protected virtual void Move()
    {
        //목표지점 이동
        //도착하면 공격
        AI = AI.AI_SEARCH;
    }
    protected virtual void Reset()
    {
        AI = AI.AI_CREATE;
    }
}




