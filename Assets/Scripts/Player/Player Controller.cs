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
    [SerializeField] int  BaseJumpCount = 0;
    [SerializeField] int JumpCount = 0;

    [Header("�÷��̾� �ӵ�")]
    [SerializeField] Vector3 VelocityValue = Vector3.zero;

    [Header("�߷� ���ӵ�")]
    [SerializeField] float Gravity = -9.81f;

    [Header("�÷��̾� ���콺 Ŀ�� ����")]
    [SerializeField] CursorLockMode CoursorLock = CursorLockMode.None;

    private void Awake()
    {
        //�ν����Ϳ��� CursorLockMode ����
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start()
    {
        
    }
    

    void Update()
    {
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
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            //Vector3 move = transform.right * x + transform.forward * z;
            //�Է°��� ������� xz��鿡���� �ӵ� ����
            Vector3 velocity = new Vector3(x, 0f, z);
            velocity = CharController.transform.TransformDirection(velocity);
            VelocityValue = velocity.normalized * MoveSpeed;

            //����ī���� �ٽ� ����
            JumpCount = BaseJumpCount;
        }
        else//ĳ���� ��Ʈ�ѷ��� ���鿡 ���� �ʾҴٸ�(�����̶��)
        {
            //���� �ӵ� = �����ӵ� + ���ӵ� * ����
            VelocityValue.y = VelocityValue.y + Gravity * Time.deltaTime;
        }

        //CharacterController���� �����ϴ� Move�Լ��� ���
        CharController.Move(VelocityValue * Time.deltaTime);


        //ī�޶� �ٶ󺸴� �������� ��ȸ
        Vector3 Dir = Camera.main.transform.forward;
        Dir.y = 0f;
        //ī�޶� ���� ������ ������ ������ ���Ѵ�.
        Vector3 tLookAtPosition = this.transform.position + Dir;
        //ī�޶� ���� ������ ������ ������ �ٶ󺸰� �Ѵ�.
        this.transform.LookAt(tLookAtPosition);

    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && JumpCount > 0)
        {
            JumpCount--;
            VelocityValue.y = JumpForce;
        }
    }
}
