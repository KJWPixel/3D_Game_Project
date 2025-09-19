using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance; //�̱���

    public int MaxSlot = 100;//�κ��丮 ���� ������

    private List<InventoryItem> Items = new List<InventoryItem>();//�κ��丮������ ����Ʈ

    private void Awake()
    {
        Instance = this;
    }
    private void AddItme(ItemData _itemData, int _Amount = 1)
    {
        //InventoryItem���� ������ ���� ID�� ������ �ִ��� Find
        InventoryItem existingItem = Items.Find(i => i.itemdata.ID == _itemData.ID);

        //������ �������� �����Ѵٸ� +1
        if(existingItem != null)
        {
            existingItem.Quantity = _Amount;
        }

        //������ �������� �������� �ʴ´ٸ�
        if(existingItem == null)
        {
            Items.Add(existingItem);
        }
    }

    private void RemoveItem(ItemData _id, int _Amount = 1)
    {
        //�ش� �������� �������� �ʴ´ٸ� Return

        //�ش� �������� �����Ѵٸ� -1
    }

    public ItemData GetItem(ItemData _Item)
    {
        return _Item;
    }
}
