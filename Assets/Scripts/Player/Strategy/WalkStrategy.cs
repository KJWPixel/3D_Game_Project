using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkStrategy : IMoveStrategy
{
    const float DeadZone = 0.01f;
    public void Move(PlayerController player)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if(Mathf.Abs(x) < DeadZone  && Mathf.Abs(z) < DeadZone)
        {
            player.SetState(PlayerState.Idle);
            return;
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            player.SetState(PlayerState.Running);
            return;
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
         
        Debug.Log($"localMove.x: {localMove.x}, localMove.z: {localMove.z}, magnitude: {player.MoveDir.magnitude}");
        player.Anim.AnimationUpdate(localMove.x, localMove.z, player.VelocityValue.y);

    }
}
