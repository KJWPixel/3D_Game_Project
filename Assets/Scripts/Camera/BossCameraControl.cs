using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BossCameraControl : MonoBehaviour
{
    // 우리가 만든 보스용 가상 카메라를 여기에 연결할 거예요.
    public CinemachineVirtualCamera bossCam;
    public float waitTime = 3f; // 몇 초 동안 보여줄지 설정

    private EnemyBoss boss;

    private void Start()
    {
        boss = GetComponentInParent<EnemyBoss>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 트리거에 들어왔는지 확인 (플레이어 태그가 "Player"여야 함)
        if (other.CompareTag("Player"))
        {
            // 보스 카메라의 우선순위를 확 높여서 화면을 전환시킵니다.
            bossCam.Priority = 20;

            if (boss != null)
            {
                UIManager.Instance.ShowBossHealth(boss.Name, boss.CurHp, boss.MaxHp);
            }

            // 일정 시간 후 다시 원래대로 돌려놓는 함수 실행
            Invoke("ReturnCamera", waitTime);

            // 한 번만 발동하도록 트리거를 끕니다.
            GetComponent<Collider>().enabled = false;
        }
    }

    void ReturnCamera()
    {
        // 우선순위를 낮추면 메인 카메라는 다시 원래(플레이어) 카메라를 쳐다봅니다.
        bossCam.Priority = 5;
    }
}
