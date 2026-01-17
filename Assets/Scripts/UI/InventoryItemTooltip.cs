using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class InventoryItemTooltip : MonoBehaviour
{
    //아이템이름, 아이템등급 프레임, 아이템아아콘, 아이템수량, 아이템설명, 아이템에 따른 버튼 활성화
    [SerializeField] private TMP_Text ItemName;
    [SerializeField] private TMP_Text ItemQuantity;
    [SerializeField] private TMP_Text ItemDescription;
    [SerializeField] private Image ItemPrame;
    [SerializeField] private Image ItemIcon;
    [SerializeField] private GameObject Button;
    private Button UseButton;

    private const string UITable = "UI Table";
    private const string ITEMTable = "ITEM Table";

    private void Awake()
    {
        UseButton = Button.GetComponent<Button>();
    }


    [Header("아이템 등급 컬러")]
    [SerializeField] private Color[] GradeColors =
    {
        Color.white,
        Color.green,
        Color.blue,
        new Color(0.6f, 0f, 1f),
        Color.yellow,
    };

    public void ItemTooltipSetup(InventoryItem inventoryItem)
    {
        //인벤토리아이템클래스 데이터에서 툴팁에 데이터 참조

        //로컬라이제이션 ItemKey, DescKey
        //ItemName.text = _InventoryItem.ItemData.ItemName;
        ItemQuantity.text = "x"+ inventoryItem.Quantity.ToString();
        ItemIcon.sprite = inventoryItem.ItemData.Icon;

        if(inventoryItem.ItemData.Type == ItemType.Equipment)
        {
            EquipementData equipData = inventoryItem.ItemData as EquipementData;
            object[] args = GetFormattedArgs(equipData);

            ItemDescription.text = LocalizationSettings.StringDatabase.GetLocalizedString(ITEMTable,inventoryItem.ItemData.DescKey,args);
        }
        else
        {
            ItemDescription.text = LocalizationSettings.StringDatabase.GetLocalizedString(ITEMTable, inventoryItem.ItemData.DescKey);
        }
            
        var GradeIndex = (int)inventoryItem.ItemData.Grade;
        ItemPrame.color = GradeColors[GradeIndex];

        UseButton.onClick.RemoveAllListeners();

        if (inventoryItem.ItemData.Type == ItemType.Consumable)
        {
            Button.SetActive(true);

            SetButtonText(inventoryItem);

            UseButton.onClick.AddListener(() =>
            {
                //인벤토리에서 즉시 소비아이템 사용 코드
                //ConsumableData consumable = _InventoryItem.ItemData as ConsumableData;
                //if (consumable != null)
                //{
                //    consumable.Use(PlayerStat.Instance.gameObject);
                //    InventoryManager.Instance.RemoveItem(_InventoryItem.ItemData, 1);

                //    ItemQuantity.text = "x" + _InventoryItem.Quantity.ToString();
                //    InventoryUI.Instance.RefreshUI();
                //}

                UI_ItemSlotManager.Instance.RegisterConsumable(inventoryItem);
                InventoryUI.Instance.RefreshUI();

            });
        }
        else if (inventoryItem.ItemData.Type == ItemType.Equipment)
        {
            Button.SetActive(true);

            SetButtonText(inventoryItem);

            InventoryUI.Instance.RefreshUI();

            UseButton.onClick.AddListener(() =>
            {
                EquipementData Equipemnet = inventoryItem.ItemData as EquipementData;
                if (Equipemnet == null) return;

                if (inventoryItem.IsEquipped )
                {
                    InventoryManager.Instance.UnequipItem(inventoryItem);
                    Debug.Log($"{inventoryItem.ItemData.name} 해제");                
                }
                else
                {
                    InventoryManager.Instance.EquipItem(inventoryItem);
                    Debug.Log($"{inventoryItem.ItemData.name} 장착");
                }
                SetButtonText(inventoryItem);
                InventoryUI.Instance.RefreshUI();              
            }) ;
        }

        //장비 부위 머리, 몸통, 손, 발, 무기
        //해당 부위의 장비가 착용 중이다 그러면 UnequipItem,
        //해당 부위에 장비가 착용중이지 않다 그러면 EquipItem
        else
        {
            Button.SetActive(false);
        }
    }  
    
    private void SetButtonText(InventoryItem _Item)
    {
        TextMeshProUGUI ButtonText = UseButton.GetComponentInChildren<TextMeshProUGUI>();

        if(_Item.ItemData.Type == ItemType.Consumable)
        {
            ButtonText.text = LocalizationSettings.StringDatabase.GetLocalizedString(UITable, "UI_EQUIPSLOT");
        }
        else if(_Item.ItemData.Type == ItemType.Equipment)
        {
            string key = _Item.IsEquipped ? "UI_UNEQUIP" : "UI_EQUIP";
            ButtonText.text = LocalizationSettings.StringDatabase.GetLocalizedString(UITable, key);
        }
    }

    private object[] GetFormattedArgs(EquipementData data)
    {
        if (data == null || data.EquipementStatus == null || data.EquipementStatus.Count == 0)
        {
            return new object[] { 0f };
        }

        float statValue = 0f;

        switch (data.EquipmentType)
        {
            case EquipmentType.Weapon:
            case EquipmentType.Helmet:
            case EquipmentType.Armor:
                statValue = data.EquipementStatus[0].Stat;
                break;
            case EquipmentType.Glove:
            case EquipmentType.Shoes:
                statValue = data.EquipementStatus[0].Stat * 100f;
                break;          
        }

        return new object[] { statValue };
    }
}
