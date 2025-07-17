using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1��Ī, 3��Ī ����Ī�ϴ� ��� �� Offset���� ������ ������������ 
public class FollowCam : MonoBehaviour
{
    [Header("ī�޶� ��ġ")]
    [SerializeField] Transform FollowTransform;

    [Header("ī�޶� ��ġ ������")]
    [SerializeField] Vector3 Offset;

    [Header("ī�޶� 1,3 ��Ī ����ġ")]
    [SerializeField] bool firstPerson = true;



    void Start()
    {
        this.transform.position = FollowTransform.position;
    }


    private void LateUpdate()
    {
        transform.position = FollowTransform.position + Offset;
        transform.rotation = FollowTransform.rotation;
    }
}
