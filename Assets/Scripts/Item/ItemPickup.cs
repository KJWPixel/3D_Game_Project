using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    //기존에 아이템이 각각 드롭되는걸 List로 변경
    [SerializeField] private List<ItemData> ItemDatas = new List<ItemData>();
    public void SetItems(List<ItemData> _Items)
    {
        ItemDatas = _Items;
    }

    //public ItemData ItemData
    //{
    //    get => itemData;
    //    set => itemData = value;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            foreach(var Item in ItemDatas)
            {
                InventoryManager.Instance.AddItem(Item);
                Debug.Log($"아이템 획득: {Item.ItemName}");
            }
            Destroy(gameObject);
        }
    }
}
