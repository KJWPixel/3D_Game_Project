using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : EnemyCharacter
{
    [Header("BossSettings")]
    public BossState CurrentState = BossState.IDLE;
    public LayerMask PlayerLayer;
    public float AttackRange;
    public bool isPlayerInRange = false;
    public float detectionRange = 50f;

    [Header("Boss Phase 2 Loop Settings")]
    [SerializeField] public float flyCoolTime = 30f;
    [SerializeField] private float flyTimer = 0f;
    [SerializeField] public bool isPhase2Active = false;
    [SerializeField] private int aerialAttackCount = 0;

    [Header("Boss Effects, Prefab")]
    [SerializeField] private GameObject ScreamAttackPrefab;
    [SerializeField] private GameObject FrameAttackPrefab;
    [SerializeField] private GameObject FlyFreamAttackPrefab;
    [SerializeField] private GameObject circleIndicator;
    [SerializeField] private GameObject DamageTextPrefab;
    [SerializeField] private GameObject DynamicObject;

    public GameObject GetCircleIndicator() => circleIndicator;
    public GameObject GetScreamExplosion() => ScreamAttackPrefab;
    public GameObject GetFrameExplosion() => FrameAttackPrefab;
    public GameObject GetFlyFreamExplosion() => FlyFreamAttackPrefab;

    [Header("Boss Audio Clip")]
    [SerializeField] private AudioClip BossBGMClip;
    [SerializeField] private AudioClip AttackMouthClip;
    [SerializeField] private AudioClip AttackHandClip;
    [SerializeField] private AudioClip AttackFrameClip;
    [SerializeField] private AudioClip AttackScreamClip; // AttackFlyScream과 동일 클립 사용
    [SerializeField] private AudioClip AttackFlyFrameClip;
    [SerializeField] private AudioClip TakeOffClip;
    [SerializeField] private AudioClip DieClip;
    [SerializeField] private AudioClip ScreamClip;
    public AudioClip GetBossBGM() => BossBGMClip;
    public AudioClip GetAttackMouthClip() => AttackMouthClip;
    public AudioClip GetAttackHandClip() => AttackHandClip;
    public AudioClip GetAttackFrameClip() => AttackFrameClip;
    public AudioClip GetAttackScreamClip() => AttackScreamClip;
    public AudioClip GetAttackFlyFrameClip() => AttackFlyFrameClip;
    

    [Header("Gizmo Debug")]
    public bool ShowDebugGizmo = true;
    private Color gizmoColor = Color.red;
    private System.Action drawGizmoAction;
    
    private List<IBossAttack> groundAttacks = new List<IBossAttack>();
    private List<IBossAttack> aerialAttacks = new List<IBossAttack>();
    private IBossAttack currentRunningStrategy;
    [SerializeField] public bool isActionRunning = false;
    private bool hasPhaseChanged = false;
    private Transform player;
    private Animator animator;



    private void Awake()
    {
        animator = GetComponent<Animator>();
        Init();
        InitAttacks();
    }

    public override void Init()
    {
        CurHp = MaxHp;
        player = GameObject.FindWithTag("Player").transform;      
    }

    private void InitAttacks()
    {
        // 지상 공격 등록
        groundAttacks.Add(new AttackFrameStrategy());
        groundAttacks.Add(new AttackHandStrategy());
        groundAttacks.Add(new AttackMouthStrategy());
        groundAttacks.Add(new AttackScreamStrategy());

        // 공중 공격 등록
        aerialAttacks.Add(new AttackFlyScreamStrategy());
        aerialAttacks.Add(new AttackFlyFlameStrategy());
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        bool wasInRange = isPlayerInRange; // wasInRange는 false
        isPlayerInRange = distance <= detectionRange;

        if (isPlayerInRange != wasInRange) // isPlayerInRange가 true가 된다면 시네머신
        {
            if (isPlayerInRange)
            {
                // 플레이어 감지 범위 진입
                // 예: 시네머신 트리거, 첫 인사 대사, BGM 변경 등
                // CinemachineVirtualCamera.Priority = 10;  // 예시
            }
            else
            {
                // 플레이어 감지 범위 이탈
                // 예: 시네머신 복귀, 상태 초기화 등
                ChangeState(BossState.IDLE);
            }
        }

        if (IsDie || CurrentState == BossState.PHASE_TRANSITION) return;

        if(isPhase2Active &&  CurrentState != BossState.PHASE_TRANSITION)
        {
            flyTimer += Time.deltaTime;

            // 이륙 조건: 타이머 완료 AND 현재 다른 행동(공격 등)을 하고 있지 않음
            if (flyTimer >= flyCoolTime && !isActionRunning)
            {
                StartCoroutine(ProcessTakeOff());
            }
        }
    }

    public void ChangeState(BossState newState)
    {
        if(isActionRunning)
        {
            // 사망이나 페이즈 전환은 언제나 허용
            bool isCriticalState = (newState == BossState.DEAD || newState == CurrentState);

            bool isAllowedState = ((int)newState >= 5 && (int)newState <= 13);

            // 중요 상태도 아니고, 공격 상태도 아니라면 (예: Chase나 Idle로 바꾸려 한다면) 거절
            if (!isCriticalState && !isAllowedState)
            {
                return;
            }
        }

        if (CurrentState == newState || CurrentState == BossState.DEAD) return;

        CurrentState = newState;
        animator.SetInteger("State", (int)newState); // 상태패턴 애니메이션 int 제어
        Debug.Log($"State Changed to: {newState}"); 
    }

    public IEnumerator StartRandomAttack()
    {
        if (isActionRunning && (int)CurrentState < 10) yield break;

        // [수정] 10(이륙), 11(공중프레임), 12(공중스크림) 상태면 공중 공격 풀 사용
        bool isFlying = ((int)CurrentState >= 10 && (int)CurrentState <= 12);
        var currentPool = isFlying ? aerialAttacks : groundAttacks;

        // 1. 전략패턴 선택
        if (currentPool.Count == 0) yield break;

        var selectedAttack = currentPool[Random.Range(0, currentPool.Count)];
        currentRunningStrategy = selectedAttack;

        // 2. 상태를 변경 
        isActionRunning = true;
        ChangeState((BossState)selectedAttack.AttackIndex);
        
        // 전략 실행 및 종료
        yield return StartCoroutine(currentRunningStrategy.Execute(this, player, () => {
            currentRunningStrategy = null;
        }));

        if(!isFlying)
        {
            isActionRunning = false;
            ChangeState(BossState.CHASE);
        }
    }


    public IEnumerator ProcessTakeOff()
    {
        Debug.Log("ProcessTakeOff 이륙 시퀀스");
        isActionRunning = true;
        flyTimer = 0;

        // 이륙 상태로 전환
        ChangeState(BossState.PHASE_TRANSITION); // Take Off 애니메이션 실행
        yield return new WaitForSeconds(8.0f); // 이륙 애니메이션 시간

        // 이제 StartRandomAttack이 실행될 때 공중 공격을 뽑도록 설정
        foreach (var attackStrategy in aerialAttacks)
        {
            // 해당 전략의 Index로 상태 변경 (Flame: 11, Scream: 12 등)
            ChangeState((BossState)attackStrategy.AttackIndex);

            // 전략 실행 및 완료 대기
            // onComplete 콜백은 필요에 따라 사용 (여기서는 코루틴 종료를 기다림)
            yield return StartCoroutine(attackStrategy.Execute(this, player, null));

            // 공격 후 잠깐 FlyIdle 상태로 대기 (공격 사이 간격)
            // ChangeState(BossState.FLY_IDLE); // 만약 FlyIdle(10) 상태가 따로 있다면 호출
            yield return new WaitForSeconds(1.0f);
        }

        yield return StartCoroutine(ProcessLanding());
    }

    public IEnumerator ProcessLanding()
    {

        ChangeState(BossState.LAND); // 착륙 애니메이션 실행
        yield return new WaitForSeconds(5f); // 착륙 애니메이션 시간

        isActionRunning = false;
        flyTimer = 0; // 지상 타이머 초기화
        ChangeState(BossState.CHASE); // 다시 지상 추격 및 공격 패턴 시작
    }

    public void OnBossAttackHit()
    {
        currentRunningStrategy?.OnEffectEvent();
    }

    public void ResetBoss()
    {
        // 1. 상태 및 변수 초기화
        StopAllCoroutines(); // 공격 중단
        isActionRunning = false;
        isPhase2Active = false;
        hasPhaseChanged = false;
        flyTimer = 0f;
        IsDie = false;

        // 2. 능력치 초기화
        CurHp = MaxHp;
        ChangeState(BossState.IDLE);

        // 3. 위치 초기화
        transform.position = new Vector3(345f, 30f, 90f);

        // 4. 사운드 및 UI 정리
        SoundManager.Instance.ApplyInGameBGM();
        UIManager.Instance.HideBossHealth();

        Debug.Log("보스가 초기화되었습니다.");
    }

    public override void TakeDamage(float fianldamage, bool isCritical)
    {
        if (IsDie) return;
        CurHp -= fianldamage;

        
        // 페이즈 전환 체크 (HP 50%미만)
        if (!hasPhaseChanged && CurHp < MaxHp * 0.5f)
        {
            Debug.Log("TakeDamage: 페이즈 전환");
            hasPhaseChanged = true;
            isPhase2Active  = true;

            StopAllCoroutines();
            StartCoroutine(ProcessTakeOff()); 
            return;
        }

        if(CurHp <= 0) { IsDie = true; Die(); }

        UIManager.Instance.UpdateBossHealth(curHp, maxHp);

        ShowDamageText(fianldamage, isCritical);
    }

    public override void Chase()
    {
        // 1. 공격중 또는 행동중이라면 중지
        if (isActionRunning) return;

        // 2. Chase상태가 아니라면 중지
        if (CurrentState != BossState.CHASE) return;

        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        transform.position += dir * RunningSpeed * Time.deltaTime;

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 5f * Time.deltaTime);
        }
    }

    public void ShowIndicator(IndicatorType type, float duration)
    {
        Debug.Log($"{type} 인디케이터 표시 {duration}초");
    }

    public void ShowIndicatorAtPos(IndicatorType type, Vector3 pos, float durationn)
    {

    }
    public void Die()
    {
        if (CurrentState == BossState.DEAD) return;

        ChangeState(BossState.DEAD);

        // [추가] 보스 체력바 숨기기
        UIManager.Instance.HideBossHealth();

        // [추가] BGM 복구
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ApplyInGameBGM();
        }

        // [추가] 투명벽 제거 등 이벤트 처리 (CameraControl에 알림)
        GetComponentInChildren<BossCameraControl>()?.OnBossDefeated();

        Destroy(gameObject, 10.0f);
    }

    // 나머지 추상함수 (보스에선 전략 패턴이 Attack을 대신함)
    public override void Idle()
    {
    }
    public override void Search()
    {
    }
    public override void Patrol()
    {
    }
    public override void Attack()
    {
    }
    public override void Flee()
    {
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

    public void TriggerScream()
    {
        if(animator != null)
        {
            animator.SetTrigger("Scream");
            SoundManager.Instance.PlayBossSFX(ScreamClip);
        }
    }
 

    public void SetGizmoAction(System.Action action)
    {
        drawGizmoAction = action;
    }

    public void ClearGizmoAction()
    {
        this.drawGizmoAction = null;
    }

    public void DrawFanGizmo(Vector3 center, Vector3 forward, float radius, float angle)
    {
        Gizmos.color = Color.red;

        // 1. 좌우 끝 선 계산
        Vector3 leftDir = Quaternion.Euler(0, -angle / 2f, 0) * forward;
        Vector3 rightDir = Quaternion.Euler(0, angle / 2f, 0) * forward;

        // 2. 중심에서 바깥으로 뻗어나가는 선
        Gizmos.DrawLine(center, center + leftDir * radius);
        Gizmos.DrawLine(center, center + rightDir * radius);

        // 3. 앞쪽 테두리 (곡선 묘사)
        int segments = 10;
        Vector3 prevPoint = center + leftDir * radius;
        for (int i = 1; i <= segments; i++)
        {
            float currentAngle = Mathf.Lerp(-angle / 2f, angle / 2f, (float)i / segments);
            Vector3 nextDir = Quaternion.Euler(0, currentAngle, 0) * forward;
            Vector3 nextPoint = center + nextDir * radius;

            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }

    public void SetCollisionIgnore(bool ignore)
    {
        // 1. 플레이어의 CharacterController 찾기 (이것 자체가 Collider 역할을 함)
        CharacterController playerController = GameObject.FindWithTag("Player")?.GetComponent<CharacterController>();

        // 2. 보스의 모든 자식 콜라이더 배열로 가져오기
        Collider[] bossColliders = GetComponentsInChildren<Collider>();

        if (playerController != null && bossColliders.Length > 0)
        {
            foreach (Collider bossCol in bossColliders)
            {
                // Physics.IgnoreCollision은 CharacterController를 Collider 인자로 받아들입니다.
                Physics.IgnoreCollision(bossCol, playerController, ignore);
            }
        }
    }
}
