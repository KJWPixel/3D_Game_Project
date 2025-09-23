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
    public static InventoryManager Instance; //싱글톤

    [SerializeField] private int MaxSlot = 100;//인벤토리 슬롯 사이즈
    private List<InventoryItem> Items = new List<InventoryItem>();//인벤토리아이템 리스트
    

    private void Awake()
    {
        Instance = this;
    }
    public void AddItem(ItemData _ItemData, int _Amount = 1)
    {
        //InventoryItem에서 기존과 같은 ID를 가지고 있는지 Find
        InventoryItem existingItem = Items.Find(i => i.ItemData.ID == _ItemData.ID);

        //기존에 아이템이 존재한다면 +1
        if(existingItem != null)
        {
            existingItem.Quantity += _Amount;
        }
        else
        {
            if(Items.Count >= MaxSlot)
            {
                Debug.Log("인벤토리 슬롯 부족");
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
