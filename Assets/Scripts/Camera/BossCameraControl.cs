using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BossCameraControl : MonoBehaviour
{
    // 우리가 만든 보스용 가상 카메라를 여기에 연결할 거예요.
    public CinemachineVirtualCamera bossCam;
    public float waitTime = 3f; // 몇 초 동안 보여줄지 설정

    [SerializeField] private GameObject invisableWall;

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
                // 1. UI 표시
                UIManager.Instance.ShowBossHealth(boss.Name, boss.CurHp, boss.MaxHp);

                // 2. BGM 변경
                SoundManager.Instance.ChangeBGM(boss.GetBossBGM());

                // 3. 보스 Scream 및 사운드 실행
                boss.TriggerScream();
            }

            // 일정 시간 후 다시 원래대로 돌려놓는 함수 실행
            Invoke("ReturnCamera", waitTime);
            // 한 번만 발동하도록 트리거를 끕니다.
            GetComponent<Collider>().enabled = false;
        }
    }

    private void ReturnCamera()
    {
        // 우선순위를 낮추면 메인 카메라는 다시 원래(플레이어) 카메라를 쳐다봅니다.
        bossCam.Priority = 5;
    }

    private void InvisableWallActive()
    {
        
    }

    public void SetEncounterActive(bool active)
    {
        GetComponent<Collider>().enabled = active;

        if(invisableWall != null)
        {
            invisableWall.SetActive(!active); // 진행중이면 키고 아니면 초기화 시엔 꺼짐
        }
    }

    public void OnBossDefeated()
    {
        if (invisableWall != null)
        {
            invisableWall.SetActive(false);
        }
            

        // 2. BGM을 다시 인게임용으로 변경 (추가)
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ApplyInGameBGM();
        }

        Debug.Log("보스 처치: BGM이 원래대로 복구됩니다.");
    }
}
