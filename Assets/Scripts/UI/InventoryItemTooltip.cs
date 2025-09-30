using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    public void ItemTooltipSetup(InventoryItem _InventoryItem)
    {
        //인벤토리아이템클래스 데이터에서 툴팁에 데이터 참조
        ItemName.text = _InventoryItem.ItemData.ItemName;
        ItemQuantity.text = "x"+_InventoryItem.Quantity.ToString();
        ItemDescription.text = _InventoryItem.ItemData.Description;
        ItemIcon.sprite = _InventoryItem.ItemData.Icon;

        var GradeIndex = (int)_InventoryItem.ItemData.Grade;
        ItemPrame.color = GradeColors[GradeIndex];

        UseButton.onClick.RemoveAllListeners();

        if (_InventoryItem.ItemData.Type == ItemType.Consumable)
        {
            Button.SetActive(true);

            SetButtonText(_InventoryItem);

            UseButton.onClick.AddListener(() =>
            {
                ConsumableData consumable = _InventoryItem.ItemData as ConsumableData;
                if (consumable != null)
                {
                    consumable.Use(PlayerStat.Instance.gameObject);
                    InventoryManager.Instance.RemoveItem(_InventoryItem.ItemData, 1);

                    ItemQuantity.text = "x" + _InventoryItem.Quantity.ToString();
                    InventoryUI.Instance.RefreshUI();
                }
                
            });
        }
        else if (_InventoryItem.ItemData.Type == ItemType.Equipment)
        {
            Button.SetActive(true);

            SetButtonText(_InventoryItem);

            InventoryUI.Instance.RefreshUI();

            UseButton.onClick.AddListener(() =>
            {
                EquipementData Equipemnet = _InventoryItem.ItemData as EquipementData;
                if (Equipemnet == null) return;

                if (_InventoryItem.IsEquipped )
                {
                    switch(Equipemnet.EquipmentType)
                    {
                        case EquipmentType.Weapon:
                            InventoryManager.Instance.UnequipItem(_InventoryItem);
                            Debug.Log($"{_InventoryItem.ItemData.name} 해제");
                            break;
                        case EquipmentType.Head:
                            InventoryManager.Instance.UnequipItem(_InventoryItem);
                            Debug.Log($"{_InventoryItem.ItemData.name} 해제");
                            break;
                        case EquipmentType.Armor:
                            InventoryManager.Instance.UnequipItem(_InventoryItem);
                            Debug.Log($"{_InventoryItem.ItemData.name} 해제");
                            break;
                        case EquipmentType.Shoes:
                            InventoryManager.Instance.UnequipItem(_InventoryItem);
                            Debug.Log($"{_InventoryItem.ItemData.name} 해제");
                            break;
                        case EquipmentType.Glove:
                            InventoryManager.Instance.UnequipItem(_InventoryItem);
                            Debug.Log($"{_InventoryItem.ItemData.name} 해제");
                            break;
                    }                   
                }
                else
                {
                    //추후 


                    switch(Equipemnet.EquipmentType)
                    {
                        case EquipmentType.Weapon:
                            InventoryManager.Instance.EquipItem(_InventoryItem);
                            Debug.Log($"{_InventoryItem.ItemData.name} 장착");
                            break;
                        case EquipmentType.Head:
                            InventoryManager.Instance.EquipItem(_InventoryItem);
                            Debug.Log($"{_InventoryItem.ItemData.name} 장착");
                            break;
                        case EquipmentType.Armor:
                            InventoryManager.Instance.EquipItem(_InventoryItem);
                            Debug.Log($"{_InventoryItem.ItemData.name} 장착");
                            break;
                        case EquipmentType.Shoes:
                            InventoryManager.Instance.EquipItem(_InventoryItem);
                            Debug.Log($"{_InventoryItem.ItemData.name} 장착");
                            break;
                        case EquipmentType.Glove:
                            InventoryManager.Instance.EquipItem(_InventoryItem);
                            Debug.Log($"{_InventoryItem.ItemData.name} 장착");
                            break;
                    }                 
                }
                SetButtonText(_InventoryItem);

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
            ButtonText.text = "사용";
        }
        else if(_Item.ItemData.Type == ItemType.Equipment)
        {
            string buttonText = _Item.IsEquipped ? "해제" : "착용";
            ButtonText.text = buttonText;
        }
    }
}
