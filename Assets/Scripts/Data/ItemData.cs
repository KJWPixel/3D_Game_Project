using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class ItemData : ScriptableObject
{
    //����Ƽ ������ �ν�����
    [Header("Item ���� ����")]
    [SerializeField] private int id;
    [SerializeField] private string itemname;
    [SerializeField] private Sprite icon;
    [SerializeField] private ItemGrade grade;
    [SerializeField] private string description;

    //�߻� ����
    public abstract ItemType Type { get; }
    public abstract int MaxStackAmount { get; }

    //������Ƽ   
    public int ID => id;
    public string ItemName => itemname;
    public Sprite Icon => icon;
    public ItemGrade Grade => grade;
    public string Description => description;
    
}   

