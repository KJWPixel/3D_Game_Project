using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkStrategy : IMoveStrategy
{
    public void Move(PlayerController _Player)
    {
        Vector3 CamForward = Camera.main.transform.forward;
        CamForward.y = 0f; // ���� ���� ����
        CamForward.Normalize();

        Vector3 CamRight = Camera.main.transform.right;
        CamRight.y = 0f;
        CamRight.Normalize();

        _Player.MoveDir = (CamForward * _Player.z + CamRight * _Player.x).normalized;

        _Player.MoveSpeed = _Player.WalkSpeed;//IsDash�� ���� Dash : Walk Speed ����
        _Player.VelocityValue = _Player.MoveDir.normalized * _Player.MoveSpeed;
        _Player.VelocityValue.y += _Player.VerticalVelocity;

        _Player.CharacterController.Move(_Player.VelocityValue * Time.deltaTime);

        //ī�޶� �ٶ󺸴� �������� ��ȸ �ڵ�
        #region

        //Vector3 Dir = Camera.main.transform.forward;
        //Dir.y = 0f;
        //Vector3 tLookAtPosition = this.transform.position + Dir;
        //this.transform.LookAt(tLookAtPosition);
        #endregion
        if (_Player.MoveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(_Player.MoveDir);
            _Player.transform.rotation = Quaternion.Slerp(_Player.transform.rotation, targetRot, Time.deltaTime * 10f);
        }

        Vector3 localMove = _Player.transform.InverseTransformDirection(_Player.MoveDir);
        //player.Anim.AnimationUpdate(localMove.x, localMove.z, player.VelocityValue.y);
    }
    
}
