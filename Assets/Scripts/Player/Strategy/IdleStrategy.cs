using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStrategy : IMoveStrategy
{
    public void Move(PlayerController player)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if(Mathf.Abs(x) > 0 || Mathf.Abs(z) > 0)//키 입력이 있으면 상태변환
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {                
                player.SetState(PlayerState.Running);
                player.Anim.AnimationUpdate(x, z, player.VerticalVelocity);
            }
            else
            {
                player.SetState(PlayerState.Walking);
                player.Anim.AnimationUpdate(x, z, player.VerticalVelocity);
            }
        }

        //애니메이션 업데이트
        player.Anim.AnimationUpdate(0, 0, player.VerticalVelocity);
    }
}
