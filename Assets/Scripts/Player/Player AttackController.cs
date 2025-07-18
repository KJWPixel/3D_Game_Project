using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [Header("����")]
    [SerializeField] GameObject BulletObject;
    [SerializeField] Transform FirePosition;

    [Header("���� ��Ÿ��")]
    [SerializeField] float AttackCoolTime = 0f;
    [SerializeField] float AttackCollTimer = 0f;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Debug.Log("Attack�Լ�: �Ѿ� �߻�");
            Attack();
        }
        
    }

    private void Attack()
    {
        GameObject Bullet = Instantiate(BulletObject, FirePosition.position, FirePosition.rotation);
        
    }
}
