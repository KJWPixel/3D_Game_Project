using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

//Rotate 회전
//X축 위 아래, Y축 좌 우, Z 좌 우 
public class PlayerController : MonoBehaviour
{
    [SerializeField] public CharacterController CharacterController;

    [Header("플레이어 이동")]
    [SerializeField] public float MoveSpeed = 0f;
    [SerializeField] public float WalkSpeed = 0f;
    [SerializeField] public float RunningSpeed = 0f;
    [SerializeField] public float RunningReductionAmount = 0f;
    [SerializeField] float JumpForce = 0f;
    [SerializeField] int BaseJumpCount = 0;
    [SerializeField] int JumpCount = 0;
    public Vector3 MoveDir;
    public float x = 0;
    public float z = 0;
    public float VerticalVelocity = 0f;

    [Header("플레이어 동작 체크")]
    [SerializeField] public bool IsRunning = false;
    [SerializeField] public bool IsDashing = false;
    [SerializeField] public bool IsGround = false;

    [Header("플레이어 현재 속도측정")]
    [SerializeField] public Vector3 VelocityValue = Vector3.zero;

    [Header("RayDistance / Gizmo 제어")]
    [SerializeField] float GroundCheckDistance = 0f;
    [SerializeField] bool GizmoOnOffCheck = false;

    public PlayerStat PlayerStat;  
    public PlayerAnimationController Anim;

    IMoveStrategy IMoveStrategy;
    public PlayerState CurrentState { get; set; }

    private void Awake()
    {
        //인스펙터에서 CursorLockMode 제어        
        PlayerStat = GetComponent<PlayerStat>();
        Anim = GetComponent<PlayerAnimationController>();
        SetState(PlayerState.Idle);
    }

    void Update()
    {
        //플레이어 환경체크
        GroundCheck();
        GravityCheck();
        
        //플레이어 입력
        HandleMoveInput();
        HandleSkillInput();

        if (IMoveStrategy != null)
        {
            IMoveStrategy.Move(this);
        }

        switch (CurrentState)
        {
            case PlayerState.Walking:
                IMoveStrategy = new WalkStrategy();
                break;
            case PlayerState.Running:
                IMoveStrategy = new RunStrategy();
                break;

        }

        //기본 움직임 동작
        //Move();
        //Running();
        Jump();
        Anim.AnimationUpdate(x, z, VelocityValue.y);
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

    public void SetState(PlayerState _State)
    {
        if(CurrentState == _State)
        {
            return;
        }

        CurrentState = _State;

        Debug.Log("Player State 상태 전환:" + _State);
    }

    private void HandleMoveInput()
    {
        //Player의 입력값을 받아 상태만을 변경

        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        if(Mathf.Abs(x) == 0 && Mathf.Abs(z) == 0)
        {
            SetState(PlayerState.Idle);
            IsRunning = false;
        }

        if(Mathf.Abs(x) > 0 || Mathf.Abs(z) > 0)
        {
            SetState(PlayerState.Walking);
        }

        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            SetState(PlayerState.Running);
            IsRunning = true;
        }
    }
    #region
    //전략패턴으로 Move함수가 쓰이므로 주석처리
    private void Move()
    {
        if (CurrentState != PlayerState.Idle && CurrentState != PlayerState.Walking)
        {
            return;
        }
        //WASD에 입력하여 값을 받음
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //MoveDir = new Vector3(x, 0f, z);
        //MoveDir = CharacterController.transform.TransformDirection(MoveDir);

        Vector3 CamForward = Camera.main.transform.forward;
        CamForward.y = 0f; // 상하 기울기 제거
        CamForward.Normalize();

        Vector3 CamRight = Camera.main.transform.right;
        CamRight.y = 0f;
        CamRight.Normalize();

        MoveDir = (CamForward * z + CamRight * x).normalized;

        MoveSpeed = IsRunning ? RunningSpeed : WalkSpeed;//IsDash에 따라 Dash : Walk Speed 결정
        VelocityValue = MoveDir.normalized * MoveSpeed;
        VelocityValue.y += VerticalVelocity;

        CharacterController.Move(VelocityValue * Time.deltaTime);

        //카메라가 바라보는 방향으로 선회 코드
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
    }
    #endregion


    #region
    ////전략패턴에서 분리하여 처리하므로 주석처리
    private void Running()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && PlayerStat.CurrentStamina > 10)
        {
            IsRunning = true;
            //Anim.SetRunning(IsRunning);
            PlayerStat.ReduceStamina(RunningReductionAmount * Time.deltaTime);
        }
        else
        {
            IsRunning = false;
            //Anim.SetRunning(IsRunning);
        }
    }
    #endregion

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
            //Ground에 닿으면 JumpCount 초기화
            JumpCount = BaseJumpCount;
        }
    }

    private void GroundCheck()
    {
        //RayCast를 이용하여 Ground만 Check
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
            //Debug.Log("Ray 아무것도 맞지 않음");
        }

        if (GizmoOnOffCheck == true)
        {
            //Gizmo 거리확인용
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

    public void LockMove()
    {

    }

    private void HandleSkillInput()//스킬슬롯 입력
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseSkillFormSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseSkillFormSlot(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseSkillFormSlot(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UseSkillFormSlot(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            UseSkillFormSlot(4);
        }

    }

    private void UseSkillFormSlot(int _SlotIndex)
    {
        SkillData SkillData = UI_Manager.Instance.GetSkillFromSlot(_SlotIndex);
        if(SkillData != null)
        {
            SkillManager.Instance.UseSkill(SkillData, null);
        }
    }


    //이전 문제가되는 Move()함수 Backup
    #region
    //Backup용 기존 Move()함수, 문제: Unity CharacterController를 사용해서 iisGrounded의 체크가 명확하지 않아서 모델을 사용할 시 
    //isGrounded 명확하지 않아 점프나 VerticalVelocity에 문제가 됨
    private void MoveBackup_Function()
    {
        //if (CharController.isGrounded)//캐릭터 컨트롤러가 지면에 닿았다면
        //{
        //    //방향축 입력
        //    //Move만 동작 추후 직접 Raycast를 이용한 isGrond Check하여 Move 구분
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
        //    //입력값을 기반으로 xz평면에서의 속도 결정
        //    Vector3 velocity = new Vector3(x, 0f, z);
        //    velocity = CharController.transform.TransformDirection(velocity);
        //    VelocityValue = velocity.normalized * MoveSpeed;

        //    //점프카운터 다시 리셋
        //    //JumpCount = BaseJumpCount
        //}
        //else//캐릭터 컨트롤러가 지면에 닿지 않았다면(공중이라면)
        //{
        //    //현재 속도 = 이전속도 + 가속도 * 간격
        //    VelocityValue.y = VelocityValue.y + Gravity * Time.deltaTime;
        //    IsGround = false;
        //}

        ////CharacterController에서 제공하는 Move함수를 사용
        //CharController.Move(VelocityValue * Time.deltaTime);

        ////카메라가 바라보는 방향으로 선회
        //Vector3 Dir = Camera.main.transform.forward;
        //Dir.y = 0f;
        ////카메라 전방 방향의 임의의 지점을 구한다.
        //Vector3 tLookAtPosition = this.transform.position + Dir;

        ////카메라의 방향대로 움직이게 한다.
        ////주석처리할 경우 WASD에 맞쳐서 Trasnform.position만 움직임, 4방향으로 밖에 움직이지 않음
        //this.transform.LookAt(tLookAtPosition);
    }
    #endregion
}
