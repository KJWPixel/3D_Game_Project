using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]


[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableData : ItemData
{
    [Header("������ �ִ� ����")]
    [SerializeField] private int maxStackAmount = 999;

    [Header("ȸ�� ȿ��")]
    [SerializeField] private float restoreHp;
    [SerializeField] private float restoreMp;

    [Header("���� ȿ��")]
    [SerializeField] private float AtkBuff;
    [SerializeField] private float DefBuff;
    [SerializeField] private float CritBuff;
    [SerializeField] private float CritDmgBuff;

    public override ItemType type => ItemType.Consumable;
    
    public int MaxStackAmount => maxStackAmount;
    public float RestoreHp => restoreHp;
    public float RestoreMp => restoreMp;

    public void Use(GameObject _Target)
    {
        var Stat = _Target.GetComponent<PlayerStat>();
        if(Stat != null)
        {
            var RecoverValues = new Dictionary<StatusType, float>();

            if(restoreHp > 0) RecoverValues[StatusType.Hp] = restoreHp;
            if(restoreMp > 0) RecoverValues[StatusType.Mp] = restoreMp;

            if(RecoverValues.Count > 0)
            {
                Stat.RecoveryStat(RecoverValues);
            }
        }
    }
}
