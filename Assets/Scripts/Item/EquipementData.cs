using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Equiment")]
public class EquipementData : ItemData
{
    [Header("아이템 최대 수량")]
    [SerializeField] private int maxStackAmount = 1;

    public override ItemType type => ItemType.Equipment;

    [System.Serializable]
    public class Status
    {
        public ItemStatus ItemStatus;
        public float Stat;
    }

    [Header("장비 능력치")]
    [SerializeField] private List<Status> EquipementStatus = new List<Status>();

    //[SerializeField] private float[] stats = new float[System.Enum.GetValues(typeof(ItemStatus)).Length];

    //public float GetStat(ItemStatus status)
    //{
    //    return stats[(int)status];
    //}
}
