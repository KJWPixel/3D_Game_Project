using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemData ItemData;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            InventoryManager.Instance.AddItem(ItemData);
            Debug.Log($"������ ��� {ItemData.ItemName}");

            Destroy(gameObject);
        }
    }
}
