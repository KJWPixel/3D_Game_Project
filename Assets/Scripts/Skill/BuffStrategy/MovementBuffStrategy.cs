using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBuffStrategy : IBuffBehavoprStrategy
{
    public BuffTargetType TargetType => BuffTargetType.Stat;
    public void ApplyBuff(PlayerStat playerStat, SkillData skillData)
    {
        foreach(var effect in skillData.Effects)
        {
            GameObject player = GameManager.Instance.Player;
            PlayerController playerController;
            playerController = player.GetComponent<PlayerController>();

            playerController.WalkSpeed *= effect.Power;
            playerController.RunningSpeed *= effect.Power;
            Debug.Log("이동속도 버프 적용");
        }
    }
    public void RemoveBuff(PlayerStat playerStat, SkillData skillData)
    {
        foreach(var effect in skillData.Effects)
        {
            GameObject player = GameManager.Instance.Player;
            PlayerController playerController;
            playerController = player.GetComponent<PlayerController>();

            playerController.WalkSpeed = playerController.WalkBaseSpeed;
            playerController.RunningSpeed = playerController.RunningBaseSpeed;
            Debug.Log("이동속도 버프 해제");
        }
    }
}
