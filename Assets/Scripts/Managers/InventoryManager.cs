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

    Dictionary<ItemType, List<InventoryItem>> ItemByType = new Dictionary<ItemType, List<InventoryItem>>()
    {
        {ItemType.Equipment, new List<InventoryItem>()},
        {ItemType.Consumable, new List<InventoryItem>()},
        {ItemType.Quest, new List<InventoryItem>()},
        {ItemType.Material, new List<InventoryItem>()}
    };

    //아이템 수가 많으면 Dictionary, 아이템을 필터링을 쉽게하려면 Type에 따라 List를 다르게 분리
    private List<InventoryItem> EquipmentItems = new List<InventoryItem>();
    private List<InventoryItem> ConsumableItems = new List<InventoryItem>();
    private List<InventoryItem> QuestItems = new List<InventoryItem>();
    private List<InventoryItem> MaterialItems = new List<InventoryItem>();  
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddItem(ItemData _ItemData, ItemType _Type, int _Amount = 1)
    {
        //InventoryItem에서 기존과 같은 ID를 가지고 있는지 Find
        InventoryItem ExistingItem = Items.Find(i => i.ItemData.ID == _ItemData.ID);

        //기존에 아이템이 존재한다면 +1
        if (ExistingItem != null)
        {
            ExistingItem.Quantity += _Amount;            
        }
        else
        {
            if (Items.Count >= MaxSlot)
            {
                Debug.Log("인벤토리 슬롯 부족");
                return;
            }

            Items.Add(new InventoryItem(_ItemData, _Amount));
        }    
    }

    public void AddItemDic(ItemData _ItemData, ItemType _Type, int _Amount = 1)//딕셔너리방식 Add
    {
        //딕셔너리 방식 아이템 관리
        //딕셔너리 선언 타입별 List >> 아이템타입이 존재하는지 확인 >> 존재한다면 
        if(!ItemByType.ContainsKey(_Type))
        {
            Debug.Log("해당 아이템타입 Key가 존재하지 않음");
            return;
        }      
    }

    public void RemoveItem(ItemData _ItemData, int _Amount = 1)
    {
        InventoryItem existingItem = Items.Find(i => i.ItemData.ID == _ItemData.ID);

        if (existingItem == null) return;

        existingItem.Quantity -= _Amount;

        if(existingItem.Quantity <= 0)
        {
            Items.Remove(existingItem);
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
