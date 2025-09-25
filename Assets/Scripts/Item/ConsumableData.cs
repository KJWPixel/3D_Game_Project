using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableData : ItemData
{
    [Header("������ �ִ� ����")]
    [SerializeField] private int maxStackAmount = 999;

    [Header("�Һ�ȿ��")]
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
