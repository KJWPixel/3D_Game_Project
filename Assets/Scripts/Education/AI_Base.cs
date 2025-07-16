
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
        //����ȿ��: ������ �ð�, ��������
        AI = AI.AI_SEARCH;
    }
    protected virtual void Search()
    {
        //��ã��, ��ã��, ��Ȳ ���
        AI = AI.AI_MOVE;
    }
    protected virtual void Move()
    {
        //��ǥ���� �̵�
        //�����ϸ� ����
        AI = AI.AI_SEARCH;
    }
    protected virtual void Reset()
    {
        AI = AI.AI_CREATE;
    }
}




