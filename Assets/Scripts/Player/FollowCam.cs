using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//1인칭, 3인칭 스위칭하는 기능 및 Offset값을 가지고 세부조정가능 
public class FollowCam : MonoBehaviour
{
    [Header("카메라 줌 오프셋")]
    [SerializeField] private float MinZoomZ = -7f;
    [SerializeField] private float MaxZoomZ = -2f;
    [SerializeField] private float ZoomSensitivity = 0.5f;

    [Header("카메라 위치 회전 값")]
    [SerializeField] private Vector3 PositionOffset = Vector3.zero;
    [SerializeField] private Vector3 RotationOffset = Vector3.zero;

    [Header("카메라 감도")]
    [SerializeField] private float LookSensitivity = 1f;//기본 1

    [SerializeField] private Transform Target;//Player Body_Spin Tarnsform

    private float MouseXValue = 0f;
    private float MouseYValue = 0f;
    private float rotX = 0f;

    UIManager UIManager;

    private void Start()
    {
        UIManager = UIManager.Instance;
    }

    private void LookCamera()
    {
        if (UIManager.Instance.IsActiveCursor == true || UIManager.Instance.isMenuPanel == true || UIManager.Instance.isSkillTree == true || UIManager.Instance.IsInventoryOpen == true)
        {
            return;
        }
           
        float MouseX = Input.GetAxisRaw("Mouse X") * LookSensitivity;
        float MouseY = Input.GetAxisRaw("Mouse Y") * LookSensitivity;

        MouseXValue = MouseXValue + MouseX;
        MouseYValue = MouseYValue + MouseY * (-1.0f);//반전, 화면에서 회전을 반전시키기 위해 -1.0f을 곱함

        //화면의 각도 제한 
        MouseYValue = Mathf.Clamp(MouseYValue, -10f, 60f);

        //Character Body 마우스 움직임에 따라 위아래 회전
        //Spin.localRotation = Quaternion.Euler(MouseYValue, 0f, 0f);      

        //마우스 움직에 따라 Camera회전
        //this.transform.rotation = Quaternion.Euler(MouseYValue, MouseXValue, 0f);

        // --- 줌 기능 추가 ---
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if(scrollInput != 0)
        {
            PositionOffset.z += scrollInput * ZoomSensitivity;

            PositionOffset.z = Mathf.Clamp(PositionOffset.z, MinZoomZ, MaxZoomZ);
        }

        UpdateCameraPosition();
    }

    #region
    private void Look()
    {
        //Mouse X,Y: 매 프레임마다 마우스의 움직임 정도
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 플레이어 전체를 좌우 회전 (Y축)
        transform.Rotate(Vector3.up * mouseX);

        // Head만 상하 회전 (X축)
        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -90f, 90f);
        transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
    }
    #endregion

    private void UpdateCameraPosition()
    {
        if (UIManager.IsActiveCursor) return;

        Quaternion CameraRotation = Quaternion.Euler(MouseYValue, MouseXValue, 0f);

        Vector3 CameraPosition = Target.position + CameraRotation * PositionOffset;

        transform.position = CameraPosition;
        transform.LookAt(Target.position + Vector3.up);
    }

    private void LateUpdate()
    {
        //this.transform.position = LookTransform.transform.position + this.transform.position + Offset;
        //this.transform.localPosition = PositionOffset;
        //this.transform.localRotation = Quaternion.Euler(RotationOffset);
        LookCamera();
    }
}


