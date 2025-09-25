using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Equiment")]
public class EquipementData : ItemData
{
    [Header("아이템 최대 수량")]
    [SerializeField] private int maxStackAmount = 1;

    [System.Serializable]
    public class Status
    {
        public ItemStatus ItemStatus;
        public float Stat;
    }

    [Header("장비 능력치")]
    [SerializeField] private List<Status> EquipementStatus = new List<Status>();

    
}
