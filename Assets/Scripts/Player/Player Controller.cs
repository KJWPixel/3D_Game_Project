using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rotate 회전
//X축 위 아래, Y축 좌 우, Z 좌 우 
public class PlayerController : MonoBehaviour
{
    [Header("플레이어 카메라 위치")]
    [SerializeField] Transform Head;

    [Header("플레이어 이동")]
    [SerializeField] float playerMoveSpeed = 0f;
    [SerializeField] float playerJumpForce = 0f;

    [Header("플레이어 마우스 회전 감도")]
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
        //Mouse X,Y: 매 프레임마다 마우스의 움직임 정도
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        // 플레이어 전체를 좌우 회전 (Y축)
        transform.Rotate(Vector3.up * mouseX);

        // Head만 상하 회전 (X축)
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
