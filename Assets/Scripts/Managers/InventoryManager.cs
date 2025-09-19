using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance; //싱글톤

    public int MaxSlot = 100;//인벤토리 슬롯 사이즈

    private List<InventoryItem> Items = new List<InventoryItem>();//인벤토리아이템 리스트

    private void Awake()
    {
        Instance = this;
    }
    private void AddItme(ItemData _itemData, int _Amount = 1)
    {
        //InventoryItem에서 기존과 같은 ID를 가지고 있는지 Find
        InventoryItem existingItem = Items.Find(i => i.itemdata.ID == _itemData.ID);

        //기존에 아이템이 존재한다면 +1
        if(existingItem != null)
        {
            existingItem.Quantity = _Amount;
        }

        //기존에 아이템이 존재하지 않는다면
        if(existingItem == null)
        {
            Items.Add(existingItem);
        }
    }

    private void RemoveItem(ItemData _id, int _Amount = 1)
    {
        //해당 아이템이 존재하지 않는다면 Return

        //해당 아이템이 존재한다면 -1
    }

    public ItemData GetItem(ItemData _Item)
    {
        return _Item;
    }
}
