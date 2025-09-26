using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConsumableEffcet
{
    public ConsumableType ConsumableType;
    public float Amount;
}

[System.Serializable]
public class ConsumableBuffEffect
{
    public ConsumableBuffType BuffType;
    public float Amount;
    public float Duration;
}

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableData : ItemData
{
    [Header("최대수량")]
    [SerializeField] private int maxStackAmount;
    public override int MaxStackAmount => maxStackAmount;
    public override ItemType Type => ItemType.Consumable;

    [Header("회복 효과")]
    [SerializeField] ConsumableEffcet[] ConsumRecovery;
    [Header("버프 효과")]
    [SerializeField] ConsumableBuffEffect[] ConsumBuff;

    IBuffBehavoprStrategy Strategy;
    public void Use(GameObject _Target)
    {
        PlayerStat Stat = _Target.GetComponent<PlayerStat>();
        
        if(Stat != null)
        {
            if(ConsumRecovery.Length > 0)
            {
                foreach (var Effect in ConsumRecovery)
                {
                    Stat.RecoveryStat(Effect.ConsumableType, Effect.Amount);
                }
            }
        }           
    }
}
