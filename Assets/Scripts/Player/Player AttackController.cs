using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [Header("����")]
    [SerializeField] GameObject BulletObject;
    [SerializeField] Transform FirePosition;

    [Header("���� ��Ÿ��")]
    [SerializeField] float AttackDelayTime= 1f;
    [SerializeField] float AttackCollTimer = 0f;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        Attack();


    }

    private void Attack()
    {
        AttackCollTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (AttackCollTimer <= 0f)
            {
                Debug.Log("Attack�Լ�: �Ѿ� �߻�");

                GameObject Bullet = Instantiate(BulletObject, FirePosition.position, FirePosition.rotation);
                AttackCollTimer = AttackDelayTime;
            }
        }
           
    }
}
