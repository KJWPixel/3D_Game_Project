using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform을 연결하세요.
    public float height = 100f; // 미니맵 카메라의 높이

    void LateUpdate()
    {
        if (player == null) return;

        // 위치는 플레이어를 따라가되
        Vector3 newPosition = player.position;
        newPosition.y += height;
        transform.position = newPosition;

        // 회전은 고정 (X축 90도(아래 보기), Y와 Z는 0으로 고정)
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

}