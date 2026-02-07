using System.Collections;
using System.Collections.Generic;
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

    // 소비아이템 등록
    // 동일 아이템이 있다면 수량만 증가, 없으면 빈 슬롯에 새로 등록
    public void RegisterConsumable(InventoryItem item)
    {
        if(item == null || item.ItemData == null) 
        {
            Debug.Log("RegisterConsumable: 유효하지 않은 아이템");
            return;
        }

        // 1. 이미 같은 아이템이 있는 슬롯 찾기
        foreach(var slot in itemSlots)
        {
            if(!slot.IsEmpty() && slot.GetItemData() != null &&  slot.GetItemData().ItemData == item.ItemData)
            {
                // 이미 장착됨 -> 중복 방지 return 또는 수량 UI만 갱신
                slot.UpdateQuantityText(); //수량 UI 갱신
                return;

            }
        }

        //슬롯을 멤버변수로 가지고 있음 -> 슬롯이 비어있는지 확인하고 SetItemSlot을 호출
        foreach (var slot in itemSlots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItemSlot(item);
                Debug.Log($"새 슬롯에 소비아이템 등록: {item.ItemData.name} x {item.Quantity}");
                return;
            }
        }

        Debug.Log("빈 슬롯이 없습니다.");
    }

    public UI_ItemSlot GetSlot(int index)
    {
        if (index < 0 || index >= slots.Length) return null;
        return slots[index];
    }
}
