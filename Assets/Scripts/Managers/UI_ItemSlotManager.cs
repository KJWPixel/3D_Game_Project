using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class UI_ItemSlotManager : MonoBehaviour
{
    public static UI_ItemSlotManager Instance;

    [SerializeField] private List<UI_ItemSlot> itemSlots = new List<UI_ItemSlot>();
    [SerializeField] private UI_ItemSlot[] slots;

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterConsumable(InventoryItem item)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItemSlot(item);
                return;
            }
        }

        Debug.Log("ºó ½½·ÔÀÌ ¾ø½À´Ï´Ù.");
    }

    public UI_ItemSlot GetSlot(int index)
    {
        if (index < 0 || index >= slots.Length) return null;
        return slots[index];
    }
}
