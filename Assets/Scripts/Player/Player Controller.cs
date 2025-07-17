using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rotate ȸ��
//X�� �� �Ʒ�, Y�� �� ��, Z �� �� 
public class PlayerController : MonoBehaviour
{
    [Header("�÷��̾� ī�޶� ��ġ")]
    [SerializeField] Transform Head;

    [Header("�÷��̾� �̵�")]
    [SerializeField] float playerMoveSpeed = 0f;
    [SerializeField] float playerJumpForce = 0f;

    [Header("�÷��̾� ���콺 ȸ�� ����")]
    [SerializeField] float lookSensitivity = 2f;
    private float rotX = 0f;

    Rigidbody rigid;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        Look();
        Move();
    }

    private void Look()
    {
        //Mouse X,Y: �� �����Ӹ��� ���콺�� ������ ����
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        // �÷��̾� ��ü�� �¿� ȸ�� (Y��)
        transform.Rotate(Vector3.up * mouseX);

        // Head�� ���� ȸ�� (X��)
        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -90f, 90f);
        Head.localRotation = Quaternion.Euler(rotX, 0f, 0f);
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        transform.position += move * playerMoveSpeed * Time.deltaTime;
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {

        }
    }


}
