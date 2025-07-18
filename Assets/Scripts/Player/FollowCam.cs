using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1인칭, 3인칭 스위칭하는 기능 및 Offset값을 가지고 세부조정가능 
public class FollowCam : MonoBehaviour
{
    [Header("카메라 위치, 후방거리 ")]
    [SerializeField] GameObject LookTransform = null;
    [SerializeField] float RearBackDistance = 0f;

    [Header("카메라 위치 오프셋")]
    [SerializeField] Vector3 Offset = Vector3.zero;

    [Header("카메라 감도")]
    [SerializeField] float LookSensitivity = 0f;

    float MouseXValue = 0f;
    float MouseYValue = 0f;
    float rotX;

    void Start()
    {
        //Offset = new Vector3(0f, 0f, -1f * RearBackDistance);

    }

    private void Update()
    {
        LookCamera();
        //Look();
    }
        

    private void LookCamera()
    {
        float MouseX = Input.GetAxisRaw("Mouse X");
        float MouseY = Input.GetAxisRaw("Mouse Y");

        MouseXValue = MouseXValue + MouseX;
        MouseYValue = MouseYValue + MouseY * (-1.0f);
        //화면에서 회전을 반전시키기 위해 -1.0f을 곱함

        //화면의 각도 제한 
        MouseYValue = Mathf.Clamp(MouseYValue, -80f, 80f);
        this.transform.rotation = Quaternion.Euler(MouseYValue, MouseXValue, 0f);
    }


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
    private void LateUpdate()
    {
        //transform.position = LookTransform.position + Offset;
        //transform.rotation = LookTransform.rotation;

        this.transform.position = LookTransform.transform.position + this.transform.rotation * Offset;
    }
}


