using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InventoryItem
{
    public ItemData ItemData;
    public int Quantity;
    public bool IsEquipped;

    public InventoryItem(ItemData _data, int _Quantity = 1)
    {
        ItemData = _data;
        Quantity = _Quantity;
        IsEquipped = false;
    }
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance; //�̱���

    [SerializeField] private int MaxSlot = 100;//�κ��丮 ���� ������
    private List<InventoryItem> Items = new List<InventoryItem>();//�κ��丮������ ����Ʈ
    

    private void Awake()
    {
        Instance = this;
    }
    public void AddItem(ItemData _ItemData, int _Amount = 1)
    {
        //InventoryItem���� ������ ���� ID�� ������ �ִ��� Find
        InventoryItem existingItem = Items.Find(i => i.ItemData.ID == _ItemData.ID);

        //������ �������� �����Ѵٸ� +1
        if(existingItem != null)
        {
            existingItem.Quantity += _Amount;
        }
        else
        {
            if(Items.Count >= MaxSlot)
            {
                Debug.Log("�κ��丮 ���� ����");
                return;
            }

            Items.Add(new InventoryItem(_ItemData, _Amount));
        }    
    }

    public void RemoveItem(ItemData _ItemData, int _Amount = 1)
    {
        InventoryItem exitingItem = Items.Find(i => i.ItemData.ID == _ItemData.ID);

        if (exitingItem == null) return;

        exitingItem .Quantity -= _Amount;

        if(exitingItem.Quantity <= 0)
        {
            Items.Remove(exitingItem);
        }
    }

    public InventoryItem GetItem(ItemData _ItemData)
    {
        return Items.Find(i => i.ItemData.ID == _ItemData.ID);
    }

    public List<InventoryItem> GetAllItems()
    {
        return Items;
    }
}
