using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRayDebug : MonoBehaviour
{
    public float distance = 10f;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);
    }

}
