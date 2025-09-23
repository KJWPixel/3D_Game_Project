using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Equiment")]
public class EquipemntData : ItemData
{
    [Header("장비 능력치")]
    public float Hp;
    public float Atk;
    public float Def;
    public float Crit;
    public float CritDmg;
}
