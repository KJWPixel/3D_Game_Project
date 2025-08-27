using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunStrategy : IMoveStrategy
{
    public void Move(PlayerController player)
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && player.PlayerStat.CurrentStamina > 10)
        {
            if(player.CurrentState != PlayerState.Running)
            {
                player.SetState(PlayerState.Running);
            }
            player.IsRunning = true;
            player.Anim.SetRunning(player.IsRunning);

            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            Vector3 camForward = Camera.main.transform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = Camera.main.transform.right;
            camRight.y = 0f;
            camRight.Normalize();

            player.MoveDir = (camForward * z + camRight * x).normalized;

            player.MoveSpeed = player.RunningSpeed;
            player.VelocityValue = player.MoveDir * player.MoveSpeed;
            player.VelocityValue.y += player.VerticalVelocity;

            player.CharacterController.Move(player.VelocityValue * Time.deltaTime);

            if (player.MoveDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(player.MoveDir);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRot, Time.deltaTime * 10f);
            }

            Vector3 localMove = player.transform.InverseTransformDirection(player.MoveDir);
            player.Anim.AnimationUpdate(localMove.x, localMove.z, player.VelocityValue.y);

            player.PlayerStat.ReduceStamina(player.RunningReductionAmount * Time.deltaTime);
        }
        else
        {
            if(player.CurrentState == PlayerState.Running)
            {
                player.SetState(PlayerState.Walking);
            }
            player.IsRunning = false;
            player.Anim.SetRunning(false);
        }
    }
}
