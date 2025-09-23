using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableData : ItemData
{
    [Header("�Һ�ȿ��")]
    [SerializeField] float RestoreHp;
    [SerializeField] float RestoreMp;
}
