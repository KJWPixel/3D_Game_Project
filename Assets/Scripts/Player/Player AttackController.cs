using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [Header("공격")]
    [SerializeField] GameObject BulletObject;
    [SerializeField] GameObject BulletEffect;
    [SerializeField] Transform FirePosition;

    [SerializeField] Light FlashLight;

    ParticleSystem psBullet;
    AudioSource asBullet;

    [Header("공격 쿨타임")]
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
        //명시적 초기화
        //RaycastHit RayHit; 암시적 초기화(기본값이 들어가기 때문에 둘 다 같은 값으로 초기화된 객체)

        if (Physics.Raycast(ray, out RayHit))
        {
            GameObject EffectInstance = Instantiate(BulletEffect, RayHit.point, Quaternion.LookRotation(RayHit.normal));

            //(해당 코드의 문제는 해당 EffectPrefabs가 인스턴트화 되지 않기 떄문에 동작은 가능해도 Effect가 에러없는 Null발생)
            //Ray가 닿는 지점에 BulletEffect
            //BulletEffect.transform.position = RayHit.point;
            //BulletEffect.transform.forward = RayHit.normal;
            //위에 문제 보완 해당 ParticleSystem이 씬에 인스턴트화 되지 않기때문에 Prefabs에서 연결하면 보이지 않는 문제가 생김
            //그래서 Hierarchy에서 EffectObject를 연결해서 쓰면 해당 오브젝트의 위치만 바뀜

            EffectInstance.transform.position = RayHit.point;
            EffectInstance.transform.forward = RayHit.normal;

            
            ParticleSystem psBulletInstance = EffectInstance.GetComponent<ParticleSystem>();
            AudioSource asBulletInstance = EffectInstance.GetComponent<AudioSource>();

            //Particle System 재생 
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

            //Audio 재생
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
        //목표지점 확인용 기즈모
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(TestRay.point, 1f);
    }
}
