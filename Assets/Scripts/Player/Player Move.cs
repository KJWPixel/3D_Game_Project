using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("�÷��̾� �̵�")]
    [SerializeField] float MoveSpeed = 0f;
    [SerializeField] float JumpForce = 0f;

    [Header("�÷��̾� ���콺 ȸ�� ����")]
    [SerializeField] float MouseSensitivity = 2f;

    [Header("���콺 ��ġ ��")]
    [SerializeField] float MouseX = 0f;
    [SerializeField] float MouseY = 0f;
    float rotX = 0f;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponentInChildren<Rigidbody>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Move();
        Jump();
    }

    private void Rotate()
    {
        MouseX += Input.GetAxis("Mouse X");//* MouseSensitivity;
        MouseY += Input.GetAxis("Mouse Y");// * MouseSensitivity;

        transform.localEulerAngles = new Vector3(-MouseY, MouseX, 0);
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 Move = transform.right * x + transform.forward * z;
        transform.position += Move * MoveSpeed * Time.deltaTime;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float VerticalVelocity = 0f;
            VerticalVelocity += JumpForce;
            rigid.velocity = new Vector3(0f, VerticalVelocity, 0f);
        }
    }

}
