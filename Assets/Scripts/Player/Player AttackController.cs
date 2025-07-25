using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [Header("����")]
    [SerializeField] GameObject BulletObject;
    [SerializeField] GameObject BulletEffect;
    [SerializeField] Transform FirePosition;

    [SerializeField] Light FlashLight;

    ParticleSystem psBullet;
    AudioSource asBullet;

    [Header("���� ��Ÿ��")]
    [SerializeField] float AttackDelayTime = 1f;
    [SerializeField] float AttackCollTimer = 0f;

    RaycastHit TestRay;

    void Start()
    {

    }


    void Update()
    {
        ShotBullet();

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log("ShotRayBullet �߻�");
            ShotRayBullet();
            SeeRay();
        }

    }

    private void ShotBullet()
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

    private void ShotRayBullet()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);//ī�޶� �ü��� �ٶ󺸴� ����
        RaycastHit RayHit = new RaycastHit();//Ray�� ���� ��ġ�� ���� ������ ��� �׸�
        //����� �ʱ�ȭ
        //RaycastHit RayHit; �Ͻ��� �ʱ�ȭ(�⺻���� ���� ������ �� �� ���� ������ �ʱ�ȭ�� ��ü)

        if (Physics.Raycast(ray, out RayHit))
        {
            GameObject EffectInstance = Instantiate(BulletEffect, RayHit.point, Quaternion.LookRotation(RayHit.normal));

            //(�ش� �ڵ��� ������ �ش� EffectPrefabs�� �ν���Ʈȭ ���� �ʱ� ������ ������ �����ص� Effect�� �������� Null�߻�)
            //Ray�� ��� ������ BulletEffect
            //BulletEffect.transform.position = RayHit.point;
            //BulletEffect.transform.forward = RayHit.normal;
            //���� ���� ���� �ش� ParticleSystem�� ���� �ν���Ʈȭ ���� �ʱ⶧���� Prefabs���� �����ϸ� ������ �ʴ� ������ ����
            //�׷��� Hierarchy���� EffectObject�� �����ؼ� ���� �ش� ������Ʈ�� ��ġ�� �ٲ�

            EffectInstance.transform.position = RayHit.point;
            EffectInstance.transform.forward = RayHit.normal;

            
            ParticleSystem psBulletInstance = EffectInstance.GetComponent<ParticleSystem>();
            AudioSource asBulletInstance = EffectInstance.GetComponent<AudioSource>();

            //Particle System ��� 
            if (psBulletInstance != null)//(psBullet != null);
            {
                Debug.Log("Particle System Bullet Play");
                psBulletInstance.Play();
                //psBullet.Play();
            }
            else
            {
                Debug.Log("psBullet Particle System Null!!!");
            }

            //Audio ���
            if (asBulletInstance != null)//(asBullet != null)
            {
                Debug.Log("AudioSource Bullet Play");
                asBulletInstance.Play();
                //asBullet.Play();
            }
            else
            {
                Debug.Log("asBullet AudioSource Null!!!");
            }

        }
    }

    private void FlashControll()
    {

    }

    private void SeeRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit RayHit;

        bool RayHitCollision = Physics.Raycast(ray, out RayHit);

        if(RayHitCollision)
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
