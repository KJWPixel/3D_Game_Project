using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableData : ItemData
{
    [Header("소비효과")]
    [SerializeField] float RestoreHp;
    [SerializeField] float RestoreMp;
}
