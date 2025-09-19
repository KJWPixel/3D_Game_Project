using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/New Item")]
public class ItemData : ScriptableObject
{
    //������Ƽ   
    public int ID => id;
    public string ItemName => itemname;
    public Sprite Icon => icon;
    public string ItemTooltip => itemtooltip;

    public float Hp => hp;
    public float Mp => mp;
    public float Stemina => stemina;
    public float Atk => atk;
    public float Def => def;
    public float Speed => speed;
    public float Critical => critical;
    public float CriticalDamage => criticaldamage;

    public float RestoreHp => restoreHp;
    public float RestoreMp => restoreMp;

    //����Ƽ ������ �ν�����
    [Header("Item �⺻����")]
    [SerializeField] int id;
    [SerializeField] string itemname;
    [SerializeField] Sprite icon;
    [SerializeField] string itemtooltip;

    [Header("������� ����")]
    [SerializeField] float hp;
    [SerializeField] float mp;
    [SerializeField] float stemina;
    [SerializeField] float atk;
    [SerializeField] float def;

    [Header("������� ��������")]
    [SerializeField] float speed;
    [SerializeField] float critical;
    [SerializeField] float criticaldamage;

    [Header("�Һ�����")]
    [SerializeField] float restoreHp;
    [SerializeField] float restoreMp;
}   

public class InventoryItem
{
    public ItemData itemdata;
    public int Quantity;

    public InventoryItem(ItemData _data, int _Quantity)
    {
        itemdata = _data;
        Quantity = _Quantity;
    }
}