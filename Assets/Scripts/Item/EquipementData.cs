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

    [Header("��� �ɷ�ġ")]
    [SerializeField] private List<EquipementItemStatus> EquipementStatus = new List<EquipementItemStatus>();

    [Header("��� �ɷ�ġ �ڵ�")]
    [SerializeField] private float[] stats = new float[System.Enum.GetValues(typeof(ItemStatus)).Length];


    public List<EquipementItemStatus> GetEquipStats()
    {
        return EquipementStatus;
    }
}
