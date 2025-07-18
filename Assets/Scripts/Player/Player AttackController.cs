using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [Header("무기")]
    [SerializeField] GameObject BulletObject;
    [SerializeField] Transform FirePosition;

    [Header("공격 쿨타임")]
    [SerializeField] float AttackCoolTime = 0f;
    [SerializeField] float AttackCollTimer = 0f;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Debug.Log("Attack함수: 총알 발사");
            Attack();
        }
        
    }

    private void Attack()
    {
        GameObject Bullet = Instantiate(BulletObject, FirePosition.position, FirePosition.rotation);
        
    }
}
