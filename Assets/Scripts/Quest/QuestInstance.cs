[System.Serializable]
public class QuestInstance 
{   public QuestData Data { get; private set; }
    public QuestState State { get; private set; }
    public int CurrentAmount { get; private set; }

    public QuestInstance(QuestData _Data)
    {
        Data = _Data;
        State = QuestState.NotStart;
        CurrentAmount = 0;
    }

    public void AddProgress(int _Value)
    {
        CurrentAmount += _Value;
        if(CurrentAmount >= Data.Amount)
        {
            State = QuestState.Completed;
        }
    }
}
