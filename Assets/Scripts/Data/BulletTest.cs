using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTest : BaseProjectile
{
    [SerializeField] float BulletDamage;
    [SerializeField] float BulletSpeed;

    Rigidbody rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        rigid.useGravity = true;
        rigid.velocity = transform.forward * BulletSpeed;
        Destroy(gameObject, 5f);
    }

    private void TekeDamage()
    {

    }
}
