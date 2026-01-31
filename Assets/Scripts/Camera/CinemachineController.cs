using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineController : MonoBehaviour
{
    private CinemachineFreeLook freeLookCam;

    [Header("카메라 줌 설정")]
    public float zoomSensitivity = 1f;
    public float minRadius = 2f;
    public float maxRadius = 7f;

    [Header("입력 축 이름")]
    [SerializeField] private string xAxisName = "Mouse X";
    [SerializeField] private string yAxisName = "Mouse Y";

    void Awake()
    {
        freeLookCam = GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        // 1. UIManager의 커서 활성화 상태 확인
        bool isCursorActive = UIManager.Instance.IsActiveCursor;

        if (isCursorActive)
        {
            // --- 마우스 커서가 활성화된 경우 (UI 조작 중) ---
            DisableCameraControl();
        }
        else
        {
            // --- 마우스 커서가 비활성화된 경우 (게임 플레이 중) ---
            EnableCameraControl();
            HandleMouseZoom(); // 줌 기능 실행
        }
    }

    private void DisableCameraControl()
    {
        // 입력을 끊기 위해 축 이름을 비우고, 현재 입력값을 0으로 강제 고정
        freeLookCam.m_XAxis.m_InputAxisName = "";
        freeLookCam.m_YAxis.m_InputAxisName = "";

        freeLookCam.m_XAxis.m_InputAxisValue = 0f;
        freeLookCam.m_YAxis.m_InputAxisValue = 0f;
    }

    private void EnableCameraControl()
    {
        // 다시 마우스 입력을 받을 수 있도록 축 이름 복구
        if (freeLookCam.m_XAxis.m_InputAxisName == "")
        {
            freeLookCam.m_XAxis.m_InputAxisName = xAxisName;
            freeLookCam.m_YAxis.m_InputAxisName = yAxisName;
        }
    }

    private void HandleMouseZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
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
