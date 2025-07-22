using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [Header("����")]
    [SerializeField] GameObject BulletObject;
    [SerializeField] GameObject BulletEffect;
    [SerializeField] Transform FirePosition;

    ParticleSystem psBullet;
    AudioSource asBullet;

    [Header("���� ��Ÿ��")]
    [SerializeField] float AttackDelayTime = 1f;
    [SerializeField] float AttackCollTimer = 0f;

    void Start()
    {
        psBullet = BulletEffect.GetComponent<ParticleSystem>();
        asBullet = BulletEffect.GetComponent<AudioSource>();

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

        if (Physics.Raycast(ray, out RayHit))
        {
            //Ray�� ��� ������ BulletEffect
            BulletEffect.transform.position = RayHit.point;
            BulletEffect.transform.forward = RayHit.normal;
            //Particle System ��� 
            if (psBullet != null)
            {
                Debug.Log("psBullet Play");
                psBullet.Play();
            }
            else
            {
                Debug.Log("psBullet null");
            }

            //Audio ���
            if (asBullet != null)
            {
                asBullet.Play();
            }
            else
            {
                Debug.Log("asBullet null");
            }

        }
    }

    RaycastHit TestRay;
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
