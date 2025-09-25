using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableData : ItemData
{
    [Header("아이템 최대 수량")]
    [SerializeField] private int maxStackAmount = 999;

    [Header("소비효과")]
    [SerializeField] private float restoreHp;
    [SerializeField] private float restoreMp;

    public float RestoreHp
    {
        get => restoreHp;
        private set => restoreHp = value;
    }

    public float RestoreMp
    {
        get => restoreMp;
        private set => restoreMp = value;
    }
}
