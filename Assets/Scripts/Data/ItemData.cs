using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/New Item")]
public class ItemData : ScriptableObject
{
    //프로퍼티   
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

    //유니티 에디터 인스펙터
    [Header("Item 기본정보")]
    [SerializeField] int id;
    [SerializeField] string itemname;
    [SerializeField] Sprite icon;
    [SerializeField] string itemtooltip;

    [Header("장비전용 스탯")]
    [SerializeField] float hp;
    [SerializeField] float mp;
    [SerializeField] float stemina;
    [SerializeField] float atk;
    [SerializeField] float def;

    [Header("장비전용 보조스탯")]
    [SerializeField] float speed;
    [SerializeField] float critical;
    [SerializeField] float criticaldamage;

    [Header("소비전용")]
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