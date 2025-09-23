using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image Icon;
    [SerializeField] private TMP_Text QuantityText;

    private InventoryItem CurrentItem;

    public void SetItem(InventoryItem _Item)
    {
        CurrentItem = _Item;

        if (_Item != null)
        {
            Icon.sprite = _Item.ItemData.Icon;
            Icon.gameObject.SetActive(true);

            QuantityText.text = _Item.Quantity > 1 ? _Item.Quantity.ToString() : "";
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        CurrentItem = null;
        Icon.sprite = null;
        Icon.gameObject.SetActive(false);
        QuantityText.text = "";
    }

    public void OnclickSlot()
    {
        if (CurrentItem == null) return;

        switch(CurrentItem.ItemData.Type)
        {
            case ItemType.Consumable:
                Debug.Log($"사용: {CurrentItem.ItemData.name}");
                InventoryManager.Instance.RemoveItem(CurrentItem.ItemData, 1);
                InventoryUI.Instance.RefreshUI();
                break;
            case ItemType.Equipment:
                Debug.Log($"장착: {CurrentItem.ItemData.ItemName}");
                CurrentItem.IsEquipped = !CurrentItem.IsEquipped;
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
