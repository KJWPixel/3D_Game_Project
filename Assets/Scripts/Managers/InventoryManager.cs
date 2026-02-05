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

    private Dictionary<EquipmentType, InventoryItem> EquipmentItems = new Dictionary<EquipmentType, InventoryItem>();

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
    
    public void EquipItem(InventoryItem item)
    {
        if (item.ItemData.Type != ItemType.Equipment) return;
      
        EquipementData Equip = item.ItemData as EquipementData; //(EquipementData)ItemData;
        EquipmentType Type = Equip.EquipmentType;
        SoundManager.Instance.PlaySystemSFX(SFXType.Equipment);

        if (EquipmentItems.TryGetValue(Type, out InventoryItem EquippedItem))
        {
            UnequipItem(EquippedItem);
        }

        EquipmentItems[Type] = item;
        item.IsEquipped = true;

        if (Equip != null)
        {
            Debug.Log("아이템 스탯 장착");
            foreach (var stat in Equip.GetEquipStats())
            {
                PlayerStat.Instance.ApplyStat(stat.ItemStatus, stat.Stat);
            }
        }
    }

    public bool IsItemEquipped(ItemData itemdata)
    {
        if (itemdata == null || itemdata.Type != ItemType.Equipment) return false;

        var TypeList = ItemByType[ItemType.Equipment];

        InventoryItem item = TypeList.Find(i => i.ItemData.ID == itemdata.ID && i.IsEquipped);

        return item != null;
    }

    public void UnequipItem(InventoryItem item)
    {
        if (item.ItemData.Type != ItemType.Equipment) return;

        EquipementData Equip = item.ItemData as EquipementData;
        EquipmentType Type = Equip.EquipmentType;
        SoundManager.Instance.PlaySystemSFX(SFXType.Equipment);

        if (EquipmentItems.ContainsKey(Type) && EquipmentItems[Type] == item)
        {
            EquipmentItems.Remove(Type);
        }

        item.IsEquipped = false;

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

    public void ClearInventory()
    {
        foreach (var list in ItemByType.Values)
        {
            list.Clear();
        }
        EquipmentItems.Clear();
    }

    // 저장된 데이터를 바탕으로 아이템 생성 및 리스트 추가
    public void LoadItem(ItemData _data, int _quantity, bool _isEquipped)
    {
        InventoryItem newItem = new InventoryItem(_data, _quantity);
        newItem.IsEquipped = _isEquipped;

        // 해당 타입 리스트에 추가
        ItemByType[_data.Type].Add(newItem);

        // 장착 상태였다면 장착 딕셔너리에도 등록
        if (_isEquipped && _data.Type == ItemType.Equipment)
        {
            EquipementData equip = _data as EquipementData;
            if (equip != null)
            {
                EquipmentItems[equip.EquipmentType] = newItem;
                // 스탯 적용이 필요하다면 여기서 호출 가능
            }
        }

        OnInventoryChanged?.Invoke();
    }
}
