[System.Serializable]
public class QuestInstance 
{   public QuestData Data { get; private set; }
    public QuestCondition State { get; private set; }
    public int CurrentAmount { get; private set; }

    public QuestInstance(QuestData _Data)
    {
        Data = _Data;
        State = QuestCondition.NotStart;
        CurrentAmount = 0;
    }

    public void AddProgress(int _Value)
    {
        CurrentAmount += _Value;
        if(CurrentAmount >= Data.Amount)
        {
            State = QuestCondition.Completed;
        }
    }
}
