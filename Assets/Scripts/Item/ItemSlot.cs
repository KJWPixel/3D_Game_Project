using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image Icon;
    [SerializeField] private TMP_Text QuantityText;

    private InventoryItem CurrentItem;

    public void SetItem(InventoryItem _Item)
    {
        if (_Item == null) return;

        CurrentItem = _Item;

        Icon.sprite = _Item.ItemData.Icon;
        Icon.enabled = true;

        if(_Item.Quantity > 1)
        {
            QuantityText.text = _Item.Quantity.ToString();
        }
        else
        {
            QuantityText.text = "";
        }
    }

    public void ClearSlot()
    {
        CurrentItem = null;
        Icon.sprite = null;
        Icon.enabled = false;
        QuantityText.text = "";
    }

    public void OnclickSlot()
    {
        if (CurrentItem == null) return;

        switch(CurrentItem.ItemData.type)
        {
            case ItemType.Consumable:
                Debug.Log($"사용: {CurrentItem.ItemData.name} {CurrentItem.ItemData.ItemTooltip}");

                ConsumableData Consumable = CurrentItem.ItemData as ConsumableData;
                if(Consumable != null)
                {
                    Consumable.Use(PlayerStat.Instance.gameObject);
                }
                InventoryManager.Instance.RemoveItem(CurrentItem.ItemData, 1);
                InventoryUI.Instance.RefreshUI();
                break;
            case ItemType.Equipment:
                Debug.Log($"장착: {CurrentItem.ItemData.ItemName}");
                CurrentItem.IsEquipped = !CurrentItem.IsEquipped;
                InventoryUI.Instance.RefreshUI();
                break;

            case ItemType.Quest:
                Debug.Log($"퀘스트 아이템: {CurrentItem.ItemData.ItemName}");
                break;

            case ItemType.Material:
                Debug.Log($"재료 아이템: {CurrentItem.ItemData.ItemName}");
                break;
        }
    }   
}
