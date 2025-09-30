using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemTooltip : MonoBehaviour
{
    //�������̸�, �����۵�� ������, �����۾ƾ���, �����ۼ���, �����ۼ���, �����ۿ� ���� ��ư Ȱ��ȭ
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


    [Header("������ ��� �÷�")]
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
        //�κ��丮������Ŭ���� �����Ϳ��� ������ ������ ����
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
                if (_InventoryItem.IsEquipped)
                {
                    //��������
                    InventoryManager.Instance.UnequipItem(_InventoryItem);
                    Debug.Log("������ ����");
                }
                else
                {
                    //��� ����
                    InventoryManager.Instance.EquipItem(_InventoryItem);
                    Debug.Log("������ ����");
                }

                SetButtonText(_InventoryItem);

                InventoryUI.Instance.RefreshUI();
               
            }) ;
        }
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
            ButtonText.text = "���";
        }
        else if(_Item.ItemData.Type == ItemType.Equipment)
        {
            string buttonText = _Item.IsEquipped ? "����" : "����";
            ButtonText.text = buttonText;
        }
    }
}
