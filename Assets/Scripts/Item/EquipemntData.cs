using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Equiment")]
public class EquipemntData : ItemData
{
    [Header("��� �ɷ�ġ")]
    public float Hp;
    public float Atk;
    public float Def;
    public float Crit;
    public float CritDmg;
}
