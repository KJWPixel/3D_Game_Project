using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Quest/New Quest")]
public class QuestData : ScriptableObject
{
    [Header("Äù½ºÆ® Á¤º¸")]
    [SerializeField] private int id;
    [SerializeField] private string questName;
    [SerializeField] private string questDescription;
    [SerializeField] private int amount;
    [SerializeField] private QuestData prerequisiteQuest;
    [SerializeField] private QuestType questType;
    [SerializeField] private int goldReward;
    [SerializeField] private int expReward;
    [SerializeField] private ItemData itemReward;

    public int Id => id;
    public string QuestName => questName;
    public string QuestDescription => questDescription;
    public int Amount
    {
        get => amount;
        set => amount = value;
    }

    public QuestData PrerequisiteQuest => prerequisiteQuest;
    public QuestType QuestType => questType;
    public int GoldRewward => goldReward;
    public int ExpReward => expReward;
    public ItemData ItemReward => itemReward;
}
