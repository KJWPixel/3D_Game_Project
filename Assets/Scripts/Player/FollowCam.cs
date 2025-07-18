using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1��Ī, 3��Ī ����Ī�ϴ� ��� �� Offset���� ������ ������������ 
public class FollowCam : MonoBehaviour
{
    [Header("ī�޶� ��ġ, �Ĺ�Ÿ� ")]
    [SerializeField] GameObject LookTransform = null;
    [SerializeField] float RearBackDistance = 0f;
    
    [Header("ī�޶� ��ġ ������")]
    [SerializeField] Vector3 Offset = Vector3.zero;

    [Header("ī�޶� 1,3 ��Ī ����ġ")]
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
        //ȭ�鿡�� ȸ���� ������Ű�� ���� -1.0f�� ����

        this.transform.rotation = Quaternion.Euler(MouseYValue, MouseXValue, 0f);
    }


    private void LateUpdate()
    {
        //transform.position = LookTransform.position + Offset;
        //transform.rotation = LookTransform.rotation;
        this.transform.position = LookTransform.transform.position + this.transform.rotation * Offset;
    }

}
