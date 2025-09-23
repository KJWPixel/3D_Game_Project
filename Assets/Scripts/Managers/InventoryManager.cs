using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InventoryItem
{
    public ItemData ItemData;
    public int Quantity;
    public bool IsEquipped;

    public InventoryItem(ItemData _data, int _Quantity = 1)
    {
        ItemData = _data;
        Quantity = _Quantity;
        IsEquipped = false;
    }
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance; //�̱���

    [SerializeField] private int MaxSlot = 100;//�κ��丮 ���� ������
    private List<InventoryItem> Items = new List<InventoryItem>();//�κ��丮������ ����Ʈ

    Dictionary<ItemType, List<InventoryItem>> ItemByType = new Dictionary<ItemType, List<InventoryItem>>()
    {
        {ItemType.Equipment, new List<InventoryItem>()},
        {ItemType.Consumable, new List<InventoryItem>()},
        {ItemType.Quest, new List<InventoryItem>()},
        {ItemType.Material, new List<InventoryItem>()}
    };

    //������ ���� ������ Dictionary, �������� ���͸��� �����Ϸ��� Type�� ���� List�� �ٸ��� �и�
    private List<InventoryItem> EquipmentItems = new List<InventoryItem>();
    private List<InventoryItem> ConsumableItems = new List<InventoryItem>();
    private List<InventoryItem> QuestItems = new List<InventoryItem>();
    private List<InventoryItem> MaterialItems = new List<InventoryItem>();  
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddItem(ItemData _ItemData, ItemType _Type, int _Amount = 1)
    {
        //InventoryItem���� ������ ���� ID�� ������ �ִ��� Find
        InventoryItem ExistingItem = Items.Find(i => i.ItemData.ID == _ItemData.ID);

        //������ �������� �����Ѵٸ� +1
        if (ExistingItem != null)
        {
            ExistingItem.Quantity += _Amount;            
        }
        else
        {
            if (Items.Count >= MaxSlot)
            {
                Debug.Log("�κ��丮 ���� ����");
                return;
            }

            Items.Add(new InventoryItem(_ItemData, _Amount));
        }    
    }

    public void AddItemDic(ItemData _ItemData, ItemType _Type, int _Amount = 1)//��ųʸ���� Add
    {
        //��ųʸ� ��� ������ ����
        //��ųʸ� ���� Ÿ�Ժ� List >> ������Ÿ���� �����ϴ��� Ȯ�� >> �����Ѵٸ� 
        if(!ItemByType.ContainsKey(_Type))
        {
            Debug.Log("�ش� ������Ÿ�� Key�� �������� ����");
            return;
        }      
    }

    public void RemoveItem(ItemData _ItemData, int _Amount = 1)
    {
        InventoryItem existingItem = Items.Find(i => i.ItemData.ID == _ItemData.ID);

        if (existingItem == null) return;

        existingItem.Quantity -= _Amount;

        if(existingItem.Quantity <= 0)
        {
            Items.Remove(existingItem);
        }
    }

    public InventoryItem GetItem(ItemData _ItemData)
    {
        return Items.Find(i => i.ItemData.ID == _ItemData.ID);
    }

    public List<InventoryItem> GetAllItems()
    {
        return Items;
    }
}
