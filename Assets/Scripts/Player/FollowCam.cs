using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1��Ī, 3��Ī ����Ī�ϴ� ��� �� Offset���� ������ ������������ 
public class FollowCam : MonoBehaviour
{
    [Header("ī�޶� ��ġ, �Ĺ�Ÿ� ")]
    [SerializeField] GameObject LookTransform = null;
    [SerializeField] float RearBackDistance = 0f;

    [Header("ī�޶� ��ġ ������")]
    [SerializeField] Vector3 Offset = Vector3.zero;

    [Header("ī�޶� ����")]
    [SerializeField] float LookSensitivity = 1f;//�⺻ 1

    [SerializeField] Transform Head;//Player Head Transform
    [SerializeField] Transform Body;//Player Body Tarnsform

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
        float MouseX = Input.GetAxisRaw("Mouse X") * LookSensitivity;
        float MouseY = Input.GetAxisRaw("Mouse Y") * LookSensitivity;

        MouseXValue = MouseXValue + MouseX;
        MouseYValue = MouseYValue + MouseY * (-1.0f);//����
        //ȭ�鿡�� ȸ���� ������Ű�� ���� -1.0f�� ����

        //ȭ���� ���� ���� 
        MouseYValue = Mathf.Clamp(MouseYValue, -80f, 80f);

        //MouseXValue�� ���� Head.rotaion x���� ����, ������ �ٶ󺸴� �������� ��,�Ʒ���  ȸ��
        Head.localRotation = Quaternion.Euler(MouseYValue, 0f, 0f);
        //MouseXValue�� ���� Body.rotaion y���� ����, ������ �ٶ󺸴� �������� ����,���������� ȸ��
        Body.localRotation = Quaternion.Euler(0f, MouseXValue, 0f);
        
        this.transform.rotation = Quaternion.Euler(MouseYValue, MouseXValue, 0f);
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
    private void LateUpdate()
    {
        //transform.position = LookTransform.position + Offset;
        //transform.rotation = LookTransform.rotation;

        //this.transform.position = LookTransform.transform.position + this.transform.rotation * Offset;
        this.transform.position = LookTransform.transform.position + Offset;
        this.transform.rotation = Head.rotation;
    }
}


