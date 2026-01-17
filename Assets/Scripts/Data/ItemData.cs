using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemData : ScriptableObject
{
    //유니티 에디터 인스펙터
    [Header("Item 공통 정보")]
    [SerializeField] private int id;
    [SerializeField] private string itemKey;
    //[SerializeField] private string itemName;
    [SerializeField] private ItemGrade grade;
    [SerializeField] private string descKey;
    //[SerializeField] private string description;    
    [SerializeField] private int price;
    [SerializeField] private Sprite icon;

    //추상 변수
    public abstract ItemType Type { get; }
    public abstract int MaxStackAmount { get; }

    //프로퍼티   
    public int ID => id;
    public string Itemkey => itemKey;
    //public string ItemName => itemName;
    public ItemGrade Grade => grade;
    public string DescKey => descKey;
    //public string Description => description;
    public int Price => price;
    public Sprite Icon => icon;
}   

