using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1인칭, 3인칭 스위칭하는 기능 및 Offset값을 가지고 세부조정가능 
public class FollowCam : MonoBehaviour
{
    [Header("카메라 위치, 후방거리 ")]
    [SerializeField] GameObject LookTransform = null;
    [SerializeField] float RearBackDistance = 0f;
    
    [Header("카메라 위치 오프셋")]
    [SerializeField] Vector3 Offset = Vector3.zero;

    [Header("카메라 1,3 인칭 스위치")]
    [SerializeField] bool firstPerson = true;

    float MouseXValue = 0f;
    float MouseYValue = 0f;

    void Start()
    {
        //Offset = new Vector3(0f, 0f, -1f * RearBackDistance);

    }

    private void Update()
    {
        float MouseX = Input.GetAxisRaw("Mouse X");
        float MouseY = Input.GetAxisRaw("Mouse Y");

        MouseXValue = MouseXValue + MouseX;
        MouseYValue = MouseYValue + MouseY * (-1.0f);
        //화면에서 회전을 반전시키기 위해 -1.0f을 곱함

        this.transform.rotation = Quaternion.Euler(MouseYValue, MouseXValue, 0f);
    }


    private void LateUpdate()
    {
        //transform.position = LookTransform.position + Offset;
        //transform.rotation = LookTransform.rotation;
        this.transform.position = LookTransform.transform.position + this.transform.rotation * Offset;
    }

}
