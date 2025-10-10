using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/New Quest")]
public class QuestData : ScriptableObject
{
    [Header("Äù½ºÆ® Á¤º¸")]
    [SerializeField] private int questId;
    [SerializeField] private string questName;
    [SerializeField] private string questTip;
    [SerializeField] private string questDescription;
    [SerializeField] private int targetId;
    [SerializeField] private int amount;
    [SerializeField] private int questLevel;
    [SerializeField] private QuestData prerequisiteQuest;
    [SerializeField] private Transform targetArea;
    [SerializeField] private QuestClassification questClassification;
    [SerializeField] private QuestClass questClass;
    [SerializeField] private QuestCondition questCondition;

    [Header("Äù½ºÆ® º¸»ó")]
    [SerializeField] private int goldReward;
    [SerializeField] private int expReward;
    [SerializeField] private ItemData itemReward;

    public int QuestId => questId;
    public string QuestName => questName;
    public string QuestDescription => questDescription;
    public int TargetId => targetId;
    public int Amount => amount;
    public int QuestLevel => questLevel;
    public QuestData PrerequisiteQuest => prerequisiteQuest;
    public Transform TargetArea => targetArea;
    public QuestClass QuestClass => questClass;
    public QuestClassification QuestClassification => questClassification;
    public QuestCondition QuestCondition => questCondition;
    public int GoldRewward => goldReward;
    public int ExpReward => expReward;
    public ItemData ItemReward => itemReward;


    
    
}
