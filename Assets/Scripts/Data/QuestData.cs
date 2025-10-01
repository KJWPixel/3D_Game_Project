using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/New Quest")]
public class QuestData : ScriptableObject
{
    [Header("Äù½ºÆ® Á¤º¸")]
    [SerializeField] private int id;
    [SerializeField] private string questName;
    [SerializeField] private string questTip;
    [SerializeField] private string questDescription;
    [SerializeField] private int targetId;
    [SerializeField] private int amount;
    [SerializeField] private QuestData prerequisiteQuest;
    [SerializeField] private QuestType questType;
    [Header("Äù½ºÆ® º¸»ó")]
    [SerializeField] private int goldReward;
    [SerializeField] private int expReward;
    [SerializeField] private ItemData itemReward;

    public int Id => id;
    public string QuestName => questName;
    public string QuestDescription => questDescription;
    public int TargetId => targetId;
    public int Amount => amount;

    public QuestData PrerequisiteQuest => prerequisiteQuest;
    public QuestType QuestType => questType;
    public int GoldRewward => goldReward;
    public int ExpReward => expReward;
    public ItemData ItemReward => itemReward;
}
