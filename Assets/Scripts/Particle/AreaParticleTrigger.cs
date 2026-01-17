using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaParticleTrigger : MonoBehaviour
{
    public ParticleSystem targetParticle;

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 태그를 가진 오브젝트가 들어왔는지 확인
        if (other.CompareTag("Player"))
        {
            if (targetParticle != null)
            {
                targetParticle.Play();
                Debug.Log("3번째 영역 입장: 파티클 재생");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetParticle != null)
            {
                targetParticle.Stop();
                Debug.Log("3번째 영역 퇴장: 파티클 정지");
            }
        }
    }
}
