using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;

//Rotate ȸ��
//X�� �� �Ʒ�, Y�� �� ��, Z �� �� 
public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController CharacterController;

    [Header("�÷��̾� �̵�")]
    [SerializeField] float MoveSpeed = 0f;
    [SerializeField] float WalkSpeed = 0f;
    [SerializeField] float DashSpeed = 0f;
    [SerializeField] float DashReductionAmount = 0f;
    [SerializeField] float JumpForce = 0f;
    [SerializeField] int BaseJumpCount = 0;
    [SerializeField] int JumpCount = 0;
    private Vector3 MoveDir;
    private float VerticalVelocity = 0f;

    [Header("�÷��̾� ���� üũ")]
    [SerializeField] bool IsDash = false;
    [SerializeField] bool IsGround = false;

    [Header("�÷��̾� ���� �ӵ�����")]
    [SerializeField] Vector3 VelocityValue = Vector3.zero;

    [Header("RayDistance / Gizmo ����")]
    [SerializeField] float GroundCheckDistance = 0f;
    [SerializeField] bool GizmoOnOffCheck = false;

    [Header("�÷��̾� ���콺 Ŀ�� ����")]
    [SerializeField] CursorLockMode CoursorLock = CursorLockMode.None;

    PlayerStat PlayerStat;
    Animator Animator;
    PlayerAnimationController Anim;

    private void Awake()
    {
        //�ν����Ϳ��� CursorLockMode ����        
        PlayerStat = GetComponent<PlayerStat>();
        Animator = GetComponent<Animator>();
        Anim = GetComponent<PlayerAnimationController>();
    }
    void Start()
    {

    }


    void Update()
    {
        GroundCheck();
        Jump();
        GravityCheck();
        Move();
        MoveDash();
    }

    #region
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
    #endregion

    private void Move()
    {
        //WASD�� �Է��Ͽ� ���� ����
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //MoveDir = new Vector3(x, 0f, z);
        //MoveDir = CharacterController.transform.TransformDirection(MoveDir);

        Vector3 CamForward = Camera.main.transform.forward;
        CamForward.y = 0f; // ���� ���� ����
        CamForward.Normalize();

        Vector3 CamRight = Camera.main.transform.right;
        CamRight.y = 0f;
        CamRight.Normalize();

        MoveDir = (CamForward * z + CamRight * x).normalized;

        MoveSpeed = IsDash ? DashSpeed : WalkSpeed;//IsDash�� ���� Dash : Walk Speed ����
        VelocityValue = MoveDir.normalized * MoveSpeed;
        VelocityValue.y += VerticalVelocity;

        CharacterController.Move(VelocityValue * Time.deltaTime);

        //ī�޶� �ٶ󺸴� �������� ��ȸ �ڵ�
        #region

        //Vector3 Dir = Camera.main.transform.forward;
        //Dir.y = 0f;
        //Vector3 tLookAtPosition = this.transform.position + Dir;
        //this.transform.LookAt(tLookAtPosition);
        #endregion
        if (MoveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(MoveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }
     
        Vector3 localMove = transform.InverseTransformDirection(MoveDir);
        Anim.AnimationUpdate(localMove.x, localMove.z, VelocityValue.y);

        //Anim.AnimationUpdate(x, z, VerticalVelocity);
    }

    private void MoveDash()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && PlayerStat.CurrentStamina > 10)
        {
            IsDash = true;
            Anim.SetDash(IsDash);
            PlayerStat.ReduceStamina(DashReductionAmount * Time.deltaTime);
        }
        else
        {
            IsDash = false;
            Anim.SetDash(IsDash);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && JumpCount > 0)
        {
            JumpCount--;
            VerticalVelocity = 0f;
            VerticalVelocity = JumpForce;                    
        }
        if (IsGround)
        {
            //Ground�� ������ JumpCount �ʱ�ȭ
            JumpCount = BaseJumpCount;
        }
    }

    private void GroundCheck()
    {
        //RayCast�� �̿��Ͽ� Ground�� Check
        RaycastHit RayHit = new RaycastHit();
        Vector3 RayOrigin = transform.position;

        bool Hit = Physics.Raycast(RayOrigin, Vector3.down, out RayHit, GroundCheckDistance);

        if (Hit)
        {
            if (RayHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                IsGround = true;
                //Debug.Log("Ray Layer Ground Hit");
            }
            else
            {
                IsGround = false;
                //Debug.Log("Ray Not Layer Ground");
            }
        }
        else
        {
            IsGround = false;
            //Debug.Log("Ray �ƹ��͵� ���� ����");
        }

        if (GizmoOnOffCheck == true)
        {
            //Gizmo �Ÿ�Ȯ�ο�
            Debug.DrawRay(RayOrigin, Vector3.down * GroundCheckDistance, Color.red);
        }
    }

    private void GravityCheck()
    {
        if (!IsGround)
        {
            VerticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        else if(VerticalVelocity < 0)
        {
            VerticalVelocity = 0f;
        }
    }

    //���� �������Ǵ� Move()�Լ� Backup
    #region
    //Backup�� ���� Move()�Լ�, ����: Unity CharacterController�� ����ؼ� iisGrounded�� üũ�� ��Ȯ���� �ʾƼ� ���� ����� �� 
    //isGrounded ��Ȯ���� �ʾ� ������ VerticalVelocity�� ������ ��
    private void MoveBackup_Function()
    {
        //if (CharController.isGrounded)//ĳ���� ��Ʈ�ѷ��� ���鿡 ��Ҵٸ�
        //{
        //    //������ �Է�
        //    //Move�� ���� ���� ���� Raycast�� �̿��� isGrond Check�Ͽ� Move ����
        //    float x = Input.GetAxisRaw("Horizontal");
        //    float z = Input.GetAxisRaw("Vertical");

        //    Animator.SetFloat("xDir", x);
        //    Animator.SetFloat("yDir", z);

        //    if (x != 0 || z != 0)
        //    {
        //        Animator.SetBool("IsMove", true);
        //    }
        //    else
        //    {
        //        Animator.SetBool("IsMove", false);
        //    }

        //    //Vector3 move = transform.right * x + transform.forward * z;
        //    //�Է°��� ������� xz��鿡���� �ӵ� ����
        //    Vector3 velocity = new Vector3(x, 0f, z);
        //    velocity = CharController.transform.TransformDirection(velocity);
        //    VelocityValue = velocity.normalized * MoveSpeed;

        //    //����ī���� �ٽ� ����
        //    //JumpCount = BaseJumpCount
        //}
        //else//ĳ���� ��Ʈ�ѷ��� ���鿡 ���� �ʾҴٸ�(�����̶��)
        //{
        //    //���� �ӵ� = �����ӵ� + ���ӵ� * ����
        //    VelocityValue.y = VelocityValue.y + Gravity * Time.deltaTime;
        //    IsGround = false;
        //}

        ////CharacterController���� �����ϴ� Move�Լ��� ���
        //CharController.Move(VelocityValue * Time.deltaTime);

        ////ī�޶� �ٶ󺸴� �������� ��ȸ
        //Vector3 Dir = Camera.main.transform.forward;
        //Dir.y = 0f;
        ////ī�޶� ���� ������ ������ ������ ���Ѵ�.
        //Vector3 tLookAtPosition = this.transform.position + Dir;

        ////ī�޶��� ������ �����̰� �Ѵ�.
        ////�ּ�ó���� ��� WASD�� ���ļ� Trasnform.position�� ������, 4�������� �ۿ� �������� ����
        //this.transform.LookAt(tLookAtPosition);
    }
    #endregion
}
