using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Quest")]
public class QuestData : ItemData
{
    [Header("퀘스트 정보")]
    public string QuestId;
}
