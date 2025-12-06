using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour
{
    [SerializeField] private Image BackGround;
    [SerializeField] private Image CurrentImage;
    [SerializeField] private TMP_Text QuantityText;

    [SerializeField] private InventoryItem itemData;

    private void Update()
    {
        IconCoolTimeUpdate();
    }

    public void SetItemSlot(InventoryItem item)
    {
        Debug.Log("SetItemSlot 호출됨. item = " + item);
        Debug.Log("ItemData = " + item?.ItemData);

        itemData = item;
        if(item != null)
        {
            BackGround.sprite = itemData.ItemData.Icon;
            CurrentImage.sprite = itemData.ItemData.Icon;
            QuantityText.text = itemData.Quantity.ToString();
        }
    }

    public void UseItem()
    {
        if (itemData == null) return;
        if (itemData.ItemData.Type != ItemType.Consumable) return;

        ConsumableData consumable = itemData.ItemData as ConsumableData;
        if (consumable == null) return;

        consumable.Use(PlayerStat.Instance.gameObject);

        // 인벤토리에서 1개 감소
        InventoryManager.Instance.RemoveItem(itemData.ItemData, 1);

        // 슬롯의 수량 갱신
        QuantityText.text = "x" + itemData.Quantity.ToString();

        // 만약 수량이 0이면 슬롯 비우기
        if (itemData.Quantity <= 0)
        {
            ClearSlot();
        }
    }
    public void ClearSlot()
    {
        itemData = null;
        CurrentImage.sprite = null;
        QuantityText.text = "";
    }

    private void IconCoolTimeUpdate()
    {
        if (itemData == null) return;

        float coolTIme = 0f;

        //if (coolTIme >= 0)
        //{
        //    CurrentImage.fillAmount = 1f - (coolTIme / 5f);
        //}
        //else
        //{
        //    CurrentImage.fillAmount = 1f;
        //}
    }

    public bool IsEmpty()
    {
        return itemData == null;
    }
}
