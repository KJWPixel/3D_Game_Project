using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    NpcTolk,
    Kill,
    Collect
}

public enum QuestState
{
    NotStart,
    Precess,
    Completed,
    Failed,
}

public class QuestData : MonoBehaviour
{
    [Header("Äù½ºÆ® Á¤º¸")]
    [SerializeField] private int id;
    [SerializeField] private string questName;
    [SerializeField] private string questDescription;
    [SerializeField] private int Amount;
    [SerializeField] private QuestType QuestType;

}
