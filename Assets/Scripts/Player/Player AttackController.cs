using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [Header("공격")]
    [SerializeField] GameObject BulletObject;
    [SerializeField] GameObject BulletEffect;
    [SerializeField] Transform FirePosition;

    ParticleSystem psBullet;
    AudioSource asBullet;

    [Header("공격 쿨타임")]
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
            Debug.Log("ShotRayBullet 발사");
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
                Debug.Log("Attack함수: 총알 발사");

                GameObject Bullet = Instantiate(BulletObject, FirePosition.position, FirePosition.rotation);
                AttackCollTimer = AttackDelayTime;
            }
        }
    }

    private void ShotRayBullet()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);//카메라 시선이 바라보는 방향
        RaycastHit RayHit = new RaycastHit();//Ray가 닿은 위치에 대한 정보를 담는 그릇

        if (Physics.Raycast(ray, out RayHit))
        {
            //Ray가 닿는 지점에 BulletEffect
            BulletEffect.transform.position = RayHit.point;
            BulletEffect.transform.forward = RayHit.normal;
            //Particle System 재생 
            if (psBullet != null)
            {
                Debug.Log("psBullet Play");
                psBullet.Play();
            }
            else
            {
                Debug.Log("psBullet null");
            }

            //Audio 재생
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
        //목표지점 확인용 기즈모
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(TestRay.point, 1f);
    }
}
