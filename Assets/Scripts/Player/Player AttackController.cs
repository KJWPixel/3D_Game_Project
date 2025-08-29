using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] Weapon_Sword WeaponSword;
    [SerializeField] List<SkillData> OwnedSkill = new List<SkillData>();

    [Header("공격 체크")]
    [SerializeField] float AttackTimer = 0f;
    [SerializeField] float AttackDuration = 0f;

    [Header("공격 카운트")]
    [SerializeField] int ComboCount = 0;
    [SerializeField] float ComboTimer = 0f;
    [SerializeField] float ComboMaxTime = 1f;

    PlayerStat PlayerStat;
    PlayerController PlayerController;
    PlayerAnimationController Anim;
    SkillManager SkillManager;
    RaycastHit TestRay;

    private void Awake()
    {
        PlayerStat = GetComponent<PlayerStat>();
        PlayerController = GetComponent<PlayerController>();

        Anim = GetComponent<PlayerAnimationController>();
        if(WeaponSword == null)
        {
            //인스펙터에서 할당하지 않았다면 자식에서 할당
            WeaponSword = GetComponentInChildren<Weapon_Sword>();
        }

        SkillManager = GetComponent<SkillManager>();
        
    }

    void Update()
    {
        //Attack();
    }

    private void Attack()
    {
        if(Input.GetKey(KeyCode.Mouse0) && PlayerController.CurrentState != PlayerState.Attacking)
        {
            Debug.Log("Player Attack");
            PlayerController.SetState(PlayerState.Attacking);
            Anim.SetAttack(true);//애니메이션 재생 
        }
        else
        {
            PlayerController.SetState(PlayerState.Idle);
            Anim.SetAttack(false);//애니메이션 종료 
        }
    }

    public void OnAttackEnable()
    {
        if (WeaponSword != null) WeaponSword.SwordEnableCollider();
    }

    // Animation Event에서 호출
    public void OnAttackDisable()
    {
        if (WeaponSword != null) WeaponSword.SwordDisableCollider();
    }


    #region
    //private void ShotRayBullet()
    //{
    //    if (Input.GetKeyDown(KeyCode.Mouse1))
    //    {
    //        Debug.Log("ShotRayBullet 발사");
    //        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);//카메라 시선이 바라보는 방향

    //        RaycastHit RayHit = new RaycastHit();//Ray가 닿은 위치에 대한 정보를 담는 그릇
    //        //명시적 초기화
    //        //RaycastHit RayHit; 암시적 초기화(기본값이 들어가기 때문에 둘 다 같은 값으로 초기화된 객체)

    //        if (Physics.Raycast(ray, out RayHit))
    //        {
    //            GameObject EffectInstance = Instantiate(BulletEffect, RayHit.point, Quaternion.LookRotation(RayHit.normal));

    //            //(해당 코드의 문제는 해당 EffectPrefabs가 인스턴트화 되지 않기 떄문에 동작은 가능해도 Effect가 에러없는 Null발생)
    //            //Ray가 닿는 지점에 BulletEffect
    //            //BulletEffect.transform.position = RayHit.point;
    //            //BulletEffect.transform.forward = RayHit.normal;
    //            //위에 문제 보완 해당 ParticleSystem이 씬에 인스턴트화 되지 않기때문에 Prefabs에서 연결하면 보이지 않는 문제가 생김
    //            //그래서 Hierarchy에서 EffectObject를 연결해서 쓰면 해당 오브젝트의 위치만 바뀜

    //            EffectInstance.transform.position = RayHit.point;
    //            EffectInstance.transform.forward = RayHit.normal;


    //            ParticleSystem psBulletInstance = EffectInstance.GetComponent<ParticleSystem>();
    //            AudioSource asBulletInstance = EffectInstance.GetComponent<AudioSource>();

    //            //Particle System 재생 
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

    //            //Audio 재생
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
        //목표지점 확인용 기즈모
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(TestRay.point, 1f);
    }
}
