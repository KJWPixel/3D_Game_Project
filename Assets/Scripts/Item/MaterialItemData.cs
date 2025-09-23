using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Material")]
public class MaterialItemData : ItemData
{
    [Header("재료 정보")]
    [SerializeField] string Category; //강화재료, 제작재료
}
