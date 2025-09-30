using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipementItemStatus
{
    public ItemStatus ItemStatus;
    public float Stat;
}

[CreateAssetMenu(menuName = "Item/Equiment")]
public class EquipementData : ItemData
{
    public override int MaxStackAmount => 1;
    public override ItemType Type => ItemType.Equipment;

    [Header("장비 능력치")]
    [SerializeField] private List<EquipementItemStatus> EquipementStatus = new List<EquipementItemStatus>();

    [Header("장비 능력치 자동")]
    [SerializeField] private float[] stats = new float[System.Enum.GetValues(typeof(ItemStatus)).Length];


    public List<EquipementItemStatus> GetEquipStats()
    {
        return EquipementStatus;
    }
}
