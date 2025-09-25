using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    //������ �������� ���� ��ӵǴ°� List�� ����
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
                Debug.Log($"������ ȹ��: {Item.ItemName}");
            }
            Destroy(gameObject);
        }
    }
}
