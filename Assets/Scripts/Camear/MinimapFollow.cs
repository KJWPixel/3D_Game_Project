using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private float Height = 100f;
    [SerializeField] private float zTilt = 1f;
    [SerializeField] private float RotaionOffset = 5f;



    void LateUpdate()
    {
        if (!Target) return;

        Vector3 Pos = Target.position;
        Pos.y += Height;
        transform.position = Pos;

        Vector3 euler = Target.eulerAngles;

        transform.rotation = Quaternion.Euler(90f, euler.y, 0f);
    }

}