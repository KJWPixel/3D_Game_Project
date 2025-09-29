using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class ItemData : ScriptableObject
{
    //유니티 에디터 인스펙터
    [Header("Item 공통 정보")]
    [SerializeField] private int id;
    [SerializeField] private string itemname;
    [SerializeField] private Sprite icon;
    [SerializeField] private ItemGrade grade;
    [SerializeField] private string description;

    //추상 변수
    public abstract ItemType Type { get; }
    public abstract int MaxStackAmount { get; }

    //프로퍼티   
    public int ID => id;
    public string ItemName => itemname;
    public Sprite Icon => icon;
    public ItemGrade Grade => grade;
    public string Description => description;
    
}   

