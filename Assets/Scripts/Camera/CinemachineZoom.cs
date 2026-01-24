using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineZoom : MonoBehaviour
{
    private CinemachineFreeLook freeLookCam;
    public float zoomSensitivity = 1f;
    public float minRadius = 2f;
    public float maxRadius = 7f;

    void Start()
    {
        freeLookCam = GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        // UI가 열려있을 때 줌 방지 (기존 코드 로직 유지)
        if (UIManager.Instance.IsActiveCursor) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            // 모든 궤도(Rig)의 반지름을 동시에 조절하여 줌 효과를 냅니다.
            for (int i = 0; i < 3; i++)
            {
                float currentRadius = freeLookCam.m_Orbits[i].m_Radius;
                currentRadius -= scroll * zoomSensitivity * 10f;
                currentRadius = Mathf.Clamp(currentRadius, minRadius, maxRadius);
                freeLookCam.m_Orbits[i].m_Radius = currentRadius;
            }
        }
    }
}
