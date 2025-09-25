using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Equiment")]
public class EquipementData : ItemData
{
    [Header("������ �ִ� ����")]
    [SerializeField] private int maxStackAmount = 1;

    [System.Serializable]
    public class Status
    {
        public ItemStatus ItemStatus;
        public float Stat;
    }

    [Header("��� �ɷ�ġ")]
    [SerializeField] private List<Status> EquipementStatus = new List<Status>();

    
}
