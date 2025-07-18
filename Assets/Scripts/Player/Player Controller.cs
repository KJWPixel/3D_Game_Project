using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rotate ȸ��
//X�� �� �Ʒ�, Y�� �� ��, Z �� �� 
public class PlayerController : MonoBehaviour
{
    //ChracterController ������Ʈ ���
    [SerializeField] CharacterController CharController = null;

    [Header("�÷��̾� �̵��ӵ� ��")]
    [SerializeField] float MoveSpeed = 0f;
    [SerializeField] float JumpForce = 0f;

    [Header("�÷��̾� �ӵ�")]
    [SerializeField] Vector3 VelocityValue = Vector3.zero;

    [Header("�߷� ���ӵ�")]
    [SerializeField] float Gravity = -9.81f;

    [Header("�÷��̾� ���콺 Ŀ�� ����")]
    [SerializeField] CursorLockMode CoursorLock = CursorLockMode.None;

    private void Awake()
    {

    }
    void Start()
    {
    }
    

    void Update()
    {
        //Look();
        Move();
        Jump();
    }

    //private void Look()
    //{
    //    //Mouse X,Y: �� �����Ӹ��� ���콺�� ������ ����
    //    float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
    //    float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

    //    // �÷��̾� ��ü�� �¿� ȸ�� (Y��)
    //    transform.Rotate(Vector3.up * mouseX);

    //    // Head�� ���� ȸ�� (X��)
    //    rotX -= mouseY;
    //    rotX = Mathf.Clamp(rotX, -90f, 90f);
    //    Head.localRotation = Quaternion.Euler(rotX, 0f, 0f);
    //}

    private void Move()
    {
        if(CharController.isGrounded)//ĳ���� ��Ʈ�ѷ��� ���鿡 ��Ҵٸ�
        {
            //������ �Է�
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            //Vector3 move = transform.right * x + transform.forward * z;
            //�Է°��� ������� xz��鿡���� �ӵ� ����
            Vector3 velocity = new Vector3(x, 0f, z);
            velocity = CharController.transform.TransformDirection(velocity);
            VelocityValue = velocity.normalized * MoveSpeed;
        }
        else//ĳ���� ��Ʈ�ѷ��� ���鿡 ���� �ʾҴٸ�(�����̶��)
        {
            //���� �ӵ� = �����ӵ� + ���ӵ� * ����
            VelocityValue.y = VelocityValue.y + Gravity * Time.deltaTime;
        }

        //CharacterController���� �����ϴ� Move�Լ��� ���
        CharController.Move(VelocityValue * Time.deltaTime);


        //ī�޶� �ٶ󺸴� �������� ��ȸ
        Vector3 tDir = Camera.main.transform.forward;
        tDir.y = 0f;
        //ī�޶� ���� ������ ������ ������ ���Ѵ�.
        Vector3 tLookAtPosition = this.transform.position + tDir;
        //ī�޶� ���� ������ ������ ������ �ٶ󺸰� �Ѵ�.
        this.transform.LookAt(tLookAtPosition);

    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            VelocityValue.y = JumpForce;
        }
    }


}
