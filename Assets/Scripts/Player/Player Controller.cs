using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rotate 회전
//X축 위 아래, Y축 좌 우, Z 좌 우 
public class PlayerController : MonoBehaviour
{
    //ChracterController 컴포넌트 사용
    [SerializeField] CharacterController CharController = null;

    [Header("플레이어 이동속도 값")]
    [SerializeField] float MoveSpeed = 0f;
    [SerializeField] float JumpForce = 0f;

    [Header("플레이어 속도")]
    [SerializeField] Vector3 VelocityValue = Vector3.zero;

    [Header("중력 가속도")]
    [SerializeField] float Gravity = -9.81f;

    [Header("플레이어 마우스 커서 제어")]
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

    private void Move()
    {
        if(CharController.isGrounded)//캐릭터 컨트롤러가 지면에 닿았다면
        {
            //방향축 입력
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            //Vector3 move = transform.right * x + transform.forward * z;
            //입력값을 기반으로 xz평면에서의 속도 결정
            Vector3 velocity = new Vector3(x, 0f, z);
            velocity = CharController.transform.TransformDirection(velocity);
            VelocityValue = velocity.normalized * MoveSpeed;
        }
        else//캐릭터 컨트롤러가 지면에 닿지 않았다면(공중이라면)
        {
            //현재 속도 = 이전속도 + 가속도 * 간격
            VelocityValue.y = VelocityValue.y + Gravity * Time.deltaTime;
        }

        //CharacterController에서 제공하는 Move함수를 사용
        CharController.Move(VelocityValue * Time.deltaTime);


        //카메라가 바라보는 방향으로 선회
        Vector3 tDir = Camera.main.transform.forward;
        tDir.y = 0f;
        //카메라 전방 방향의 임의의 지점을 구한다.
        Vector3 tLookAtPosition = this.transform.position + tDir;
        //카메라 전방 방향의 임의의 지점을 바라보게 한다.
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
