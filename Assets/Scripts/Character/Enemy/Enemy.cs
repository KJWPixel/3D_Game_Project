using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using UnityEngine;

public class Enemy : EnemyCharacter
{
    [Header("AI")]
    [SerializeField] EnemyAI EnemyAI;

    [Header("TRPATH")]
    [SerializeField] public bool TRPATHCheck = false;
    [SerializeField] public Transform[] TRPATH;
    [SerializeField] public int CurrentPathIndex = 0;
    [SerializeField] float PatrolWaitTime = 0f;
    [SerializeField] float PatrolWaitStartTime= 0f;

    [Header("Prefabs")]
    [SerializeField] GameObject DamageTextPrefab;
    [SerializeField] GameObject DynamicObject;

    [Header("Player")]
    [SerializeField] GameObject Player;
    [SerializeField] PlayerStat PlayerStat;
    Animator Animator;
    ItemDrop ItemDrop;
    Collider Collider;
    public event Action<GameObject> OnDied;

    private Vector3 originalPosition;
    public Vector3 OriginalPosition => originalPosition;
    private void Awake()
    {
        Init();
        EnemyAI = GetComponent<EnemyAI>();
        Animator = GetComponent<Animator>();
        ItemDrop = GetComponent<ItemDrop>();
        Collider = GetComponent<Collider>();
    }

    public override void Init()
    {
        CurHp = MaxHp;
        originalPosition = transform.position;
    }

    private void Start()
    {
        if(Player == null)
        {
            Player = GameObject.FindWithTag("Player");
        }
        PlayerStat = Player.GetComponent<PlayerStat>();

        if(DynamicObject == null)
        {
            DynamicObject = GameObject.Find("DynamicObject");

        }
    }

    private void Update()
    {
        AnimatorUpdate(EnemyAI.CurrentAI);
    }

    private void AnimatorUpdate(AI _AI)
    {
        if (Animator == null) return;

        int State = _AI switch
        {
            AI.AI_CREATE => 0,
            AI.AI_IDLE => 1,
            AI.AI_SEARCH => 2,
            AI.AI_PATROL => 3,
            AI.AI_CHASE => 4,
            AI.AI_FLEE => 4,
            AI.AI_ATTACK => 6,
            AI.AI_DEAD => 7,
        };

        Animator.SetInteger("State", State);
    }

    public override void Idle()
    {
        //Debug.Log($"<b><color=orange>{Name}: Idle</color></b>");
    }

    public override void Search()
    {
        
    }
    public override void Patrol()
    {
        if(TRPATH == null || TRPATH.Length == 0 || !TRPATHCheck) return;

        Transform PathPoint = TRPATH[CurrentPathIndex];
        Vector3 Dir = PathPoint.position - transform.position;//방향벡터

        float Distance = Dir.magnitude;//벡터의 길이

        if(Distance > 0.2f)
        {
            transform.position += Dir.normalized * WalkSpeed * Time.deltaTime;

            if (Dir != Vector3.zero)
            {
                //방향벡터로 정면 방향으로 회전
                Quaternion PathRotation = Quaternion.LookRotation(Dir);

                transform.rotation = Quaternion.Slerp(transform.rotation, PathRotation, 5f * Time.deltaTime);
            }

            PatrolWaitStartTime = 0f;
        }
        else
        {
            PatrolWaitStartTime += Time.deltaTime;
            if (PatrolWaitStartTime >= PatrolWaitTime)
            {
                CurrentPathIndex = (CurrentPathIndex + 1) % TRPATH.Length;
                PatrolWaitStartTime = 0f;
            }
        }
    }

    public override void Chase()
    {
        if(Player == null) return;

        Vector3 Dir = Player.transform.position - transform.position;
        transform.position += Dir.normalized * RunningSpeed * Time.deltaTime;

        if(Dir != Vector3.zero)
        {
            Quaternion TargetRotation = Quaternion.LookRotation(Dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, 5f * Time.deltaTime);
        }
    }

    public override void Flee()
    {
        Vector3 targetPosition;

        if(TRPATH != null && TRPATH.Length > 0)
        {
            if (CurrentPathIndex >= 0 && CurrentPathIndex < TRPATH.Length && TRPATH[CurrentPathIndex] != null)
            {
                targetPosition = TRPATH[CurrentPathIndex].position;
            }
            else
            {
                targetPosition = originalPosition;
            }
        }
        else
        {
            targetPosition = originalPosition;
        }

        Vector3 Dir = targetPosition - transform.position;

        transform.position += Dir.normalized * runningSpeed * Time.deltaTime;

        if (Dir != Vector3.zero)
        {
            Quaternion TargetRotation = Quaternion.LookRotation(Dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, 5f * Time.deltaTime);
        }


    }
    public override void Attack()
    {
        if (Player == null) return;

        float Distance = Vector3.Distance(transform.position, Player.transform.position);
        if(Distance <= 3f)
        {
            PlayerStat.TakeDamage(Atk);
            Debug.Log($"PlayerTake Damage{Atk}");
        }
    }

    public override void TakeDamage(float finalDamage, bool isCritical)
    {
        if (IsDie) return;

        CurHp -= finalDamage;

        if(CurHp <= 0)
        {
            CurHp = 0;
            IsDie = true;
            Die();
        }

        EnemyAI.OnDamageByPlayer();

        if(CurHp > 0 )
        {
            ShowDamageText(finalDamage, isCritical);
        }
    }

    public override void ShowDamageText(float damage, bool isCritical)
    {
        Vector3 spawnPosition = transform.position + Vector3.up * 2f;

        GameObject DamageTextInstance = Instantiate(DamageTextPrefab, spawnPosition, Quaternion.identity, DynamicObject.transform);

        DamageText DamageText = DamageTextInstance.GetComponent<DamageText>();

        if (DamageText != null)
        {
            DamageText.SetDamageText(damage, isCritical);
        }
    }

    private void Die()
    {
        
        // AI 상태를 DEAD로 즉시 전환
        if (EnemyAI != null)
        {
            EnemyAI.SetDeadState();
        }

        // 물리 및 충돌 비활성화
        Collider.enabled = false;

        // 보상 로직 
        QuestManager.Instance.UpdateQuestPrecess(QuestClassification.Kill, id, 1);
        OnDied?.Invoke(gameObject);
        PlayerStat.AddGold(GainGold);
        PlayerStat.AddExp(GainExp); 
        ItemDrop.ItemsDrop();

        // 즉시 삭제
        Destroy(gameObject, 10.0f);
    }
}
