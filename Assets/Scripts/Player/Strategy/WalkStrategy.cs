using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkStrategy : IMoveStrategy
{
    public void Move(PlayerController player)
    {

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(x) > 0 || Mathf.Abs(z) > 0) //x 또는 z 값이 절대적으로 0보다 크면 
        {
            if (player.CurrentState != PlayerState.Walking)
            {
                player.SetState(PlayerState.Walking);
            }
        }
        else
        {
            if (player.CurrentState == PlayerState.Walking)
            {
                player.SetState(PlayerState.Idle);
            }
        }

        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        player.MoveDir = (camForward * z + camRight * x).normalized;

        player.MoveSpeed = player.WalkSpeed;
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
    }
}
