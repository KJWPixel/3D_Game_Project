using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1인칭, 3인칭 스위칭하는 기능 및 Offset값을 가지고 세부조정가능 
public class FollowCam : MonoBehaviour
{
    [Header("카메라 위치")]
    [SerializeField] Transform FollowTransform;

    [Header("카메라 위치 오프셋")]
    [SerializeField] Vector3 Offset;

    [Header("카메라 1,3 인칭 스위치")]
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
