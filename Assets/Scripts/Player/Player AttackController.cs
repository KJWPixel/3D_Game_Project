using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [Header("���� üũ")]
    [SerializeField] bool IsAttack = false;

    [Header("���� ������Ʈ")]
    [SerializeField] BoxCollider SowrdBoxCollider;
    [SerializeField] float AttackCollTimer = 0f;

    PlayerStat PlayerStat;
    PlayerAnimationController Anim;
    RaycastHit TestRay;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Enemy Enemy = other.GetComponent<Enemy>();
            Enemy.TakeDamage(PlayerStat.AttackDamage);
        }
    }

    private void Awake()
    {
        PlayerStat = GetComponent<PlayerStat>();
        Anim = GetComponent<PlayerAnimationController>();
        
    }

    void Start()
    {
        SowrdBoxCollider.enabled = false;
    }


    void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            Debug.Log("Player Attack");
            IsAttack = true;
            Anim.SetAttack(IsAttack);
        }
        else
        {
            IsAttack = false;
            Anim.SetAttack(IsAttack);
        }
    }

    public void EnabledAttackCollider()
    {
        SowrdBoxCollider.enabled = true;    
    }
    public void DisableAttackCollider()
    {
        SowrdBoxCollider.enabled = false;
    }

    #region
    //private void ShotRayBullet()
    //{
    //    if (Input.GetKeyDown(KeyCode.Mouse1))
    //    {
    //        Debug.Log("ShotRayBullet �߻�");
    //        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);//ī�޶� �ü��� �ٶ󺸴� ����

    //        RaycastHit RayHit = new RaycastHit();//Ray�� ���� ��ġ�� ���� ������ ��� �׸�
    //        //����� �ʱ�ȭ
    //        //RaycastHit RayHit; �Ͻ��� �ʱ�ȭ(�⺻���� ���� ������ �� �� ���� ������ �ʱ�ȭ�� ��ü)

    //        if (Physics.Raycast(ray, out RayHit))
    //        {
    //            GameObject EffectInstance = Instantiate(BulletEffect, RayHit.point, Quaternion.LookRotation(RayHit.normal));

    //            //(�ش� �ڵ��� ������ �ش� EffectPrefabs�� �ν���Ʈȭ ���� �ʱ� ������ ������ �����ص� Effect�� �������� Null�߻�)
    //            //Ray�� ��� ������ BulletEffect
    //            //BulletEffect.transform.position = RayHit.point;
    //            //BulletEffect.transform.forward = RayHit.normal;
    //            //���� ���� ���� �ش� ParticleSystem�� ���� �ν���Ʈȭ ���� �ʱ⶧���� Prefabs���� �����ϸ� ������ �ʴ� ������ ����
    //            //�׷��� Hierarchy���� EffectObject�� �����ؼ� ���� �ش� ������Ʈ�� ��ġ�� �ٲ�

    //            EffectInstance.transform.position = RayHit.point;
    //            EffectInstance.transform.forward = RayHit.normal;


    //            ParticleSystem psBulletInstance = EffectInstance.GetComponent<ParticleSystem>();
    //            AudioSource asBulletInstance = EffectInstance.GetComponent<AudioSource>();

    //            //Particle System ��� 
    //            if (psBulletInstance != null)//(psBullet != null);
    //            {
    //                Debug.Log("Particle System Bullet Play");
    //                psBulletInstance.Play();
    //                //psBullet.Play();
    //            }
    //            else
    //            {
    //                Debug.Log("psBullet Particle System Null!!!");
    //            }

    //            //Audio ���
    //            if (asBulletInstance != null)//(asBullet != null)
    //            {
    //                Debug.Log("AudioSource Bullet Play");
    //                asBulletInstance.Play();
    //                //asBullet.Play();
    //            }
    //            else
    //            {
    //                Debug.Log("asBullet AudioSource Null!!!");
    //            }
    //        }
    //        SeeRay();
    //    }
    //}
    #endregion
    private void SeeRay()
    {    
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit RayHit;

        bool RayHitCollision = Physics.Raycast(ray, out RayHit);

        if (RayHitCollision)
        {
            Debug.Log("ShotCollision");

            TestRay = RayHit;
        }
        else
        {
            Debug.Log("Not Collision");
        }
    }

    private void OnDrawGizmos()
    {
        //��ǥ���� Ȯ�ο� �����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(TestRay.point, 1f);
    }
}
