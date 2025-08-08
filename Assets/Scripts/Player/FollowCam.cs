using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//1��Ī, 3��Ī ����Ī�ϴ� ��� �� Offset���� ������ ������������ 
public class FollowCam : MonoBehaviour
{
    [Header("ī�޶� ��ġ ������")]
    [SerializeField] Vector3 PositionOffset = Vector3.zero;
    [SerializeField] Vector3 RotationOffset = Vector3.zero;

    [Header("ī�޶� ����")]
    [SerializeField] float LookSensitivity = 1f;//�⺻ 1

    [SerializeField] Transform Target;//Player Body_Spin Tarnsform

    float MouseXValue = 0f;
    float MouseYValue = 0f;
    float rotX = 0f;

    void Start()
    {

    }

    private void Update()
    {
        LookCamera();
    }
        

    private void LookCamera()
    {
        float MouseX = Input.GetAxisRaw("Mouse X") * LookSensitivity;
        float MouseY = Input.GetAxisRaw("Mouse Y") * LookSensitivity;

        MouseXValue = MouseXValue + MouseX;
        MouseYValue = MouseYValue + MouseY * (-1.0f);//����, ȭ�鿡�� ȸ���� ������Ű�� ���� -1.0f�� ����

        //ȭ���� ���� ���� 
        MouseYValue = Mathf.Clamp(MouseYValue, -80f, 80f);
        
        //Character Body ���콺 �����ӿ� ���� ���Ʒ� ȸ��
        //Spin.localRotation = Quaternion.Euler(MouseYValue, 0f, 0f);      

        //���콺 ������ ���� Cameraȸ��
        //this.transform.rotation = Quaternion.Euler(MouseYValue, MouseXValue, 0f);

        UpdateCameraPosition();
    }

    #region
    private void Look()
    {
        //Mouse X,Y: �� �����Ӹ��� ���콺�� ������ ����
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // �÷��̾� ��ü�� �¿� ȸ�� (Y��)
        transform.Rotate(Vector3.up * mouseX);

        // Head�� ���� ȸ�� (X��)
        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -90f, 90f);
        transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
    }
    #endregion

    private void UpdateCameraPosition()
    {
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
    }
}


