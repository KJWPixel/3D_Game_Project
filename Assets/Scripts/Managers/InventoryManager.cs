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

    //기존 Items에서 딕셔너리 구조로 변경 Key: ItemType, Value: List<InventoryItem> 아이템 타입에 따라 리스트 인벤토리 아이템으로 들어감
    Dictionary<ItemType, List<InventoryItem>> ItemByType = new Dictionary<ItemType, List<InventoryItem>>()
    {
        {ItemType.Equipment, new List<InventoryItem>()},
        {ItemType.Consumable, new List<InventoryItem>()},
        {ItemType.Quest, new List<InventoryItem>()},
        {ItemType.Material, new List<InventoryItem>()}
    };

    public event Action OnInventoryChanged;

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

    public bool AddItem(ItemData _ItemData, int _Amount = 1)
    {
        if (_ItemData == null || _Amount == 0) return false;

        var TypeList = ItemByType[_ItemData.Type];

        //InventoryItem에서 기존과 같은 ID를 가지고 있는지 탐색
        InventoryItem ExistingItem = TypeList.Find(i => i.ItemData.ID == _ItemData.ID);

        //장비 아이템의 경우 최대스택은 언제나1, 중복은 허용하게 
        if(_ItemData.Type == ItemType.Equipment)
        {
            if (GetTotalItemCount() >= MaxSlot)
            {
                Debug.Log("인벤토리 슬롯 부족");
                return false;
            }
            else
            {
                TypeList.Add(new InventoryItem(_ItemData, _Amount));             
            }
            OnInventoryChanged?.Invoke();
            return true;
        }
 
        //기존에 아이템이 존재한다면 +1
        if (ExistingItem != null)
        {
            if(ExistingItem.Quantity >= _ItemData.MaxStackAmount)//아이템 MaxStack확인
            {
                Debug.Log($"{_ItemData.name}아이템 최대수량 초과");
                return  false;                
            }

            switch(_ItemData.Type)
            {
                case ItemType.Consumable:
                    ExistingItem.Quantity += _Amount;
                    break;
                case ItemType.Quest:
                    ExistingItem.Quantity += _Amount;
                    break;
                case ItemType.Material:
                    ExistingItem.Quantity += _Amount;
                    break;
            }
        }
        else
        {
            if (GetTotalItemCount() >= MaxSlot)
            {
                Debug.Log("인벤토리 슬롯 부족");
                return false;
            }

            TypeList.Add(new InventoryItem(_ItemData, _Amount));
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool RemoveItem(ItemData _ItemData, int _Amount = 1)
    {
        if (_ItemData == null || _Amount == 0 ) return false;

        var TypeList = ItemByType[_ItemData.Type];

        InventoryItem ExisitngItem = TypeList.Find(i => i.ItemData.ID == _ItemData.ID);

        if(ExisitngItem == null) return false;

        ExisitngItem.Quantity -= _Amount;

        if(ExisitngItem.Quantity <= 0)
        {
            TypeList.Remove(ExisitngItem);
        }

        OnInventoryChanged.Invoke();
        return true;
    }
    
    public void EquipItem(InventoryItem _Item)
    {
        if (_Item.ItemData.Type != ItemType.Equipment) return;

        _Item.IsEquipped = true;

        EquipementData Equip = _Item.ItemData as EquipementData; //(EquipementData)ItemData;

        if(Equip != null)
        {
            Debug.Log("아이템 스탯 장착");
            foreach (var stat in Equip.GetEquipStats())
            {
                PlayerStat.Instance.ApplyStat(stat.ItemStatus, stat.Stat);
            }
        }
    }

    public void UnequipItem(InventoryItem _Item)
    {
        if (_Item.ItemData.Type != ItemType.Equipment) return;

        _Item.IsEquipped = false;

        EquipementData Equip = _Item.ItemData as EquipementData;

        if (Equip != null)
        {
            Debug.Log("아이템 스탯해제");
            foreach (var stat in Equip.GetEquipStats())
            {
                PlayerStat.Instance.RemoveStat(stat.ItemStatus, stat.Stat);
            }
        }
    }

    public List<InventoryItem> GetItemByType(ItemType _Type)
    {
        return ItemByType[_Type];
    }

    public List<InventoryItem> GetAllItems()
    {
        List<InventoryItem> All = new List<InventoryItem>();
        foreach (var Item in ItemByType)
        {
            All.AddRange(Item.Value);
        }
        return All;
    }

    public int GetTotalItemCount()
    {
        int count = 0;
        foreach (var item in ItemByType)
        {
            count += item.Value.Count;
        }
        return count;
    }
}
