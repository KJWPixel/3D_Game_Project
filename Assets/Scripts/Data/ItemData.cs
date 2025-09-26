using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class ItemData : ScriptableObject
{
    //����Ƽ ������ �ν�����
    [Header("Item ���� ����")]
    [SerializeField] int id;
    [SerializeField] string itemname;
    [SerializeField] Sprite icon;
    [SerializeField] string itemtooltip;

    public abstract ItemType Type { get; }
    public abstract int MaxStackAmount { get; }

    //������Ƽ   
    public int ID => id;
    public string ItemName => itemname;
    public Sprite Icon => icon;
    public string ItemTooltip => itemtooltip;
    
}   

