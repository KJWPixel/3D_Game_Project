using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Quest")]
public class QuestData : ItemData
{
    [Header("Äù½ºÆ® Á¤º¸")]
    public string QuestId;

    public override ItemType type => ItemType.Quest;
}
