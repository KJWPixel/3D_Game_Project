using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [SerializeField] Transform SlotParent;
    [SerializeField] GameObject SlotPrefab;

    private ItemSlot[] Slots;
    private List<InventoryItem> FilteringItem = new List<InventoryItem>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Slots = new ItemSlot[InventoryManager.Instance.GetAllItems().Count];
        CreateSlots();
        RefreshUI();
    }

    private void CreateSlots()
    {
        int MaxSlot = 100;
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
        var Items = InventoryManager.Instance.GetAllItems();

        foreach(var Slot in Slots)
        {
            Slot.ClearSlot();
        }

        for(int i = 0; i < Items.Count; i++)
        {
            Slots[i].SetItem(Items[i]);
        }
    }

    public void ItemFilter(ItemType _Type)
    {
        
        
    }
}
