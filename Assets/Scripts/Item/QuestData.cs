using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Quest")]
public class QuestData : ItemData
{
    public override int MaxStackAmount => 1;

    [Header("����Ʈ ����")]
    public string QuestId;

    public override ItemType Type => ItemType.Quest;
}
