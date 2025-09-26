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

    //������Ƽ   
    public int ID => id;
    public string ItemName => itemname;
    public Sprite Icon => icon;
    public string ItemTooltip => itemtooltip;
    public abstract ItemType type { get; }
}   

