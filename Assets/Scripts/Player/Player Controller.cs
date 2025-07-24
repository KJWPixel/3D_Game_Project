using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Rotate 회전
//X축 위 아래, Y축 좌 우, Z 좌 우 
public class PlayerController : MonoBehaviour
{
    //ChracterController 컴포넌트 사용
    [SerializeField] CharacterController CharController = null;

    [Header("플레이어 이동")]
    [SerializeField] float MoveSpeed = 0f;
    [SerializeField] float DashSpeed = 0f;
    [SerializeField] float JumpForce = 0f;
    [SerializeField] int BaseJumpCount = 0;
    [SerializeField] int JumpCount = 0;
    private float VerticalVelocity = 0f;

    [Header("플레이어 동작 체크")]
    [SerializeField] bool Dashing = false;
    [SerializeField] bool IsGround = false;

    [Header("플레이어 현재 속도측정")]
    [SerializeField] Vector3 VelocityValue = Vector3.zero;

    [Header("RayDistance / Gizmo 제어")]
    [SerializeField] float GroundCheckDistance = 0f;
    [SerializeField] bool GizmoOnOffCheck = false;


    [Header("중력 가속도")]
    [SerializeField] float Gravity = -9.81f;

    [Header("플레이어 마우스 커서 제어")]
    [SerializeField] CursorLockMode CoursorLock = CursorLockMode.None;

    Animator Animator;
    Rigidbody Rigidbody;

    private void Awake()
    {
        //인스펙터에서 CursorLockMode 제어
        Cursor.lockState = CursorLockMode.Locked;
        Animator = GetComponent<Animator>();
    }
    void Start()
    {

    }


    void Update()
    {
        Move();
        Jump();
        GroundCheck();
    }

    #region
    //private void Look()
    //{
    //    //Mouse X,Y: 매 프레임마다 마우스의 움직임 정도
    //    float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
    //    float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

    //    // 플레이어 전체를 좌우 회전 (Y축)
    //    transform.Rotate(Vector3.up * mouseX);

    //    // Head만 상하 회전 (X축)
    //    rotX -= mouseY;
    //    rotX = Mathf.Clamp(rotX, -90f, 90f);
    //    Head.localRotation = Quaternion.Euler(rotX, 0f, 0f);
    //}
    #endregion

    private void Move()
    {
        //WASD에 입력하여 값을 받음
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //x, z값이 0이 아니면 True    
        Animator.SetBool("IsMove", x != 0 || z != 0);
        //해당 xDir,yDir값을 Animator에 전달
        Animator.SetFloat("xDir", x);
        Animator.SetFloat("yDir", z);

        //velocity에 x,z값을 대입
        Vector3 velocity = new Vector3(x, 0f, z);
        //CharController의 local좌표값을 World좌표로 전환
        velocity = CharController.transform.TransformDirection(velocity);
        VelocityValue = velocity.normalized * MoveSpeed;

        //중력 적용 및 y축에 속도 대입
        VerticalVelocity += Gravity * Time.deltaTime;
        VelocityValue.y = VerticalVelocity;

        //Ground에 닿았을 떄 점프, VerticlaVelocity 초기화
        if (IsGround)
        {
            VerticalVelocity = 0f;
            JumpCount = BaseJumpCount;          
        }

        CharController.Move(VelocityValue * Time.deltaTime);
        
        //카메라가 바라보는 방향으로 선회
        Vector3 Dir = Camera.main.transform.forward;
        Dir.y = 0f;
        Vector3 tLookAtPosition = this.transform.position + Dir;

        this.transform.LookAt(tLookAtPosition);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && JumpCount > 0 )
        {
            JumpCount--;
            VerticalVelocity = JumpForce;

        }

        
    }

    private void GroundCheck()
    {
        //RayCast를 이용하여 GroundCheck
        RaycastHit RayHit = new RaycastHit();
        Vector3 RayOrigin = transform.position;

        bool Hit = Physics.Raycast(RayOrigin, Vector3.down, out RayHit, GroundCheckDistance);

        if(Hit)
        {
            if(RayHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                IsGround = true;
                //Debug.Log("Ray Layer Ground Hit");
            }
            else
            {
                IsGround= false;
                //Debug.Log("Ray Not Layer Ground");
            }
        }
        else
        {
            IsGround = false;
            //Debug.Log("Ray 아무것도 맞지 않음");
        }

        if (GizmoOnOffCheck == true)
        {
            //Gizmo 거리확인용
            Debug.DrawRay(RayOrigin, Vector3.down * GroundCheckDistance, Color.red);
        }    
    }

    //이전 문제가되는 Move()함수 Backup
    #region
    //Backup용 기존 Move()함수, 문제: Unity CharacterController를 사용해서 iisGrounded의 체크가 명확하지 않아서 모델을 사용할 시 
    //isGrounded 명확하지 않아 점프나 VerticalVelocity에 문제가 됨
    private void MoveBackup_Function()
    {
        if (CharController.isGrounded)//캐릭터 컨트롤러가 지면에 닿았다면
        {
            //방향축 입력
            //Move만 동작 추후 직접 Raycast를 이용한 isGrond Check하여 Move 구분
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            Animator.SetFloat("xDir", x);
            Animator.SetFloat("yDir", z);

            if (x != 0 || z != 0)
            {
                Animator.SetBool("IsMove", true);
            }
            else
            {
                Animator.SetBool("IsMove", false);
            }

            //Vector3 move = transform.right * x + transform.forward * z;
            //입력값을 기반으로 xz평면에서의 속도 결정
            Vector3 velocity = new Vector3(x, 0f, z);
            velocity = CharController.transform.TransformDirection(velocity);
            VelocityValue = velocity.normalized * MoveSpeed;

            //점프카운터 다시 리셋
            //JumpCount = BaseJumpCount
        }
        else//캐릭터 컨트롤러가 지면에 닿지 않았다면(공중이라면)
        {
            //현재 속도 = 이전속도 + 가속도 * 간격
            VelocityValue.y = VelocityValue.y + Gravity * Time.deltaTime;
            IsGround = false;
        }

        //CharacterController에서 제공하는 Move함수를 사용
        CharController.Move(VelocityValue * Time.deltaTime);

        //카메라가 바라보는 방향으로 선회
        Vector3 Dir = Camera.main.transform.forward;
        Dir.y = 0f;
        //카메라 전방 방향의 임의의 지점을 구한다.
        Vector3 tLookAtPosition = this.transform.position + Dir;

        //카메라의 방향대로 움직이게 한다.
        //주석처리할 경우 WASD에 맞쳐서 Trasnform.position만 움직임, 4방향으로 밖에 움직이지 않음
        this.transform.LookAt(tLookAtPosition);
    }
    #endregion

    private void Dash()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Dashing = true;
        }

    }
}
