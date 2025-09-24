using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [SerializeField] Transform SlotParent;
    [SerializeField] GameObject SlotPrefab;
    [SerializeField] int MaxSlot = 100;

    private ItemSlot[] Slots;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreateSlots();
        InventoryManager.Instance.OnInventoryChanged += RefreshUI;     
        RefreshUI(InventoryManager.Instance.GetAllItems());
    }

    private void CreateSlots()
    {
        Slots = new ItemSlot[MaxSlot];

        for (int i = 0; i < MaxSlot; i++)
        {
            GameObject SlotObj = Instantiate(SlotPrefab, SlotParent);
            Slots[i] = SlotObj.GetComponent<ItemSlot>();
            Slots[i].ClearSlot();
        }
    }

    public void RefreshUI()
    {
        RefreshUI(InventoryManager.Instance.GetAllItems());
    }

    public void RefreshUI(List<InventoryItem> _ItemsToShow)
    {
        foreach(var Slot in Slots)
        {
            Slot.ClearSlot();
        }

        for(int i = 0; i < _ItemsToShow.Count; i++)
        {
            Slots[i].SetItem(_ItemsToShow[i]);
        }
    }

    public void ItemFilter(int _TypeIndex)
    {
        ItemType Type = (ItemType)_TypeIndex;

        var filteredItems = InventoryManager.Instance.GetItemByType(Type);

        RefreshUI(filteredItems);       
    }
}
