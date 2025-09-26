using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Material")]
public class MaterialItemData : ItemData
{
    public override int MaxStackAmount => 1;

    [Header("��� ����")]
    [SerializeField] string Category; //��ȭ���, �������

    public override ItemType Type => ItemType.Material;
}
