using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image Icon;
    [SerializeField] private TMP_Text QuantityText;
    [SerializeField] private TMP_Text EquipText;

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

        if(_Item.IsEquipped)
        {
            EquipText.text = "ÀåÂø";
        }
        else
        {
            EquipText.text = "";
        }
    }

    public void ClearSlot()
    {
        CurrentItem = null;
        Icon.sprite = null;
        Icon.enabled = false;
        QuantityText.text = "";
        EquipText.text = "";
    }

    public void OnclickSlot()
    {
        if (CurrentItem == null) return;

        InventoryUI.Instance.ShowTooltip(CurrentItem); 
    }   
}
