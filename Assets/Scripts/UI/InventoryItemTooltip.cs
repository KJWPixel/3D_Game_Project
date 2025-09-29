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
    [SerializeField] private GameObject UseButton;

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
        ItemName.text = _InventoryItem.ItemData.ItemName;
        ItemQuantity.text = _InventoryItem.Quantity.ToString();
        ItemDescription.text = _InventoryItem.ItemData.Description;
        ItemIcon.sprite = _InventoryItem.ItemData.Icon;

        var GradeIndex = (int)_InventoryItem.ItemData.Grade;
        ItemPrame.color = GradeColors[GradeIndex];

        UseButton.GetComponent<Button>().onClick.RemoveAllListeners();

        if (_InventoryItem.ItemData.Type == ItemType.Consumable)
        {
            UseButton.SetActive(true);
            UseButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                ConsumableData consumable = _InventoryItem.ItemData as ConsumableData;
                if (consumable != null)
                {
                    consumable.Use(PlayerStat.Instance.gameObject);
                    InventoryManager.Instance.RemoveItem(_InventoryItem.ItemData, 1);
                    InventoryUI.Instance.RefreshUI();
                }
                gameObject.SetActive(false); // ���� �ݱ�
            });
        }
        else if (_InventoryItem.ItemData.Type == ItemType.Equipment)
        {
            UseButton.SetActive(true);
            UseButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                _InventoryItem.IsEquipped = !_InventoryItem.IsEquipped;
                InventoryUI.Instance.RefreshUI();
                gameObject.SetActive(false); // ���� �ݱ�
            });
        }
        else
        {
            UseButton.SetActive(false);
        }
    }    
}
