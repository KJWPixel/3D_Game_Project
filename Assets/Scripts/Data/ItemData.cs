using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class ItemData : ScriptableObject
{
    //유니티 에디터 인스펙터
    [Header("Item 공통 정보")]
    [SerializeField] int id;
    [SerializeField] string itemname;
    [SerializeField] Sprite icon;
    [SerializeField] string itemtooltip;

    //프로퍼티   
    public int ID => id;
    public string ItemName => itemname;
    public Sprite Icon => icon;
    public string ItemTooltip => itemtooltip;
    public abstract ItemType type { get; }
}   

