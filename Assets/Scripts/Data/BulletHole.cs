using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHole : MonoBehaviour
{
    [SerializeField] float DestroyTime;

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, DestroyTime);
    }
}
