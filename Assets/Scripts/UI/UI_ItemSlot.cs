using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour
{
    [SerializeField] private Image BackGround;
    [SerializeField] private Image CurrentImage;
    [SerializeField] private TMP_Text QuantityText;
    [SerializeField] private float coolTimeDuration = 5.0f;
  
    private InventoryItem itemData;
    private float remainingCoolTime = 0f;
    private bool isCooling = false;

    private void Update()
    {
        IconCoolTimeUpdate();
    }

    public void SetItemSlot(InventoryItem item)
    {
        if(item == null || item.ItemData == null) return;

        itemData = item;

        Sprite icon = item.ItemData.Icon;
        if(icon != null)
        {
            BackGround.sprite = icon;
            CurrentImage.sprite = icon;
        }

        UpdateQuantityText();
        ResetCoolTime();

        //itemData = item;
        //if(item != null)
        //{
        //    BackGround.sprite = itemData.ItemData.Icon;
        //    CurrentImage.sprite = itemData.ItemData.Icon;
        //    QuantityText.text = itemData.Quantity.ToString();
        //}
    }

    public void UseItem()
    {
        if (itemData == null || itemData.ItemData.Type != ItemType.Consumable) return;

        if(isCooling)
        {
            Debug.Log("아직 쿨타임 중입니다.");
            return;
        }

        ConsumableData consumable = itemData.ItemData as ConsumableData;
        if (consumable == null) return;

        // 아이템 효과 적용
        consumable.Use(PlayerStat.Instance.gameObject);

        // 인벤토리에서 수량 감소
        InventoryManager.Instance.RemoveItem(itemData.ItemData, 1);

        // UI슬롯의 수량 갱신
        UpdateQuantityText();

        // 수량이 0이면 슬롯 비우기
        if (itemData.Quantity <= 0)
        {
            ClearSlot();
            return;
        }

        StartCoolTime();
    }

    private void StartCoolTime()
    {
        isCooling = true;
        remainingCoolTime = coolTimeDuration;
        CurrentImage.fillAmount = 1f;
    }

    private void ResetCoolTime()
    {
        isCooling = false;  
        remainingCoolTime = 0f;
        CurrentImage.fillAmount = 1f;
    }

    private void IconCoolTimeUpdate()
    {
        if (!isCooling) return;

        remainingCoolTime -= Time.deltaTime;

        if (remainingCoolTime <= 0f)
        {
            remainingCoolTime = 0f;
            isCooling = false;
            CurrentImage.fillAmount = 1f;
        }
        else
        {
            CurrentImage.fillAmount = remainingCoolTime / coolTimeDuration;
        }
    }

    public void UpdateQuantityText()
    {
        if (QuantityText != null && itemData != null)
        {
            QuantityText.text = itemData.Quantity.ToString();
        }
    }
    public void ClearSlot()
    {
        itemData = null;
        if(CurrentImage != null) CurrentImage.sprite = null;
        if(QuantityText != null) QuantityText.text = "";
        if(BackGround != null) BackGround.sprite = null;

        ResetCoolTime();
    }

    public bool IsEmpty()
    {
        return itemData == null;
    }

    public InventoryItem GetItemData()
    {
        return itemData;
    }
}
