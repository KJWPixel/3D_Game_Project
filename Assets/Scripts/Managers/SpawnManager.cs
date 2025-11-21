using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform[] SpawnPoints;
    [SerializeField] private Transform SpawnParent;
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject[] SpawnPrefabs;
    [Header("SpawnSettings")]
    [SerializeField] private float RespawnDelay = 0f;
    [SerializeField] private bool SpawnOnCehck = true;

    [SerializeField] private bool playerInside = false;
    [SerializeField] private bool isRespawning = false;
    [SerializeField] private List<GameObject> activeEnemies = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInside = true;
            if(SpawnOnCehck && activeEnemies.Count == 0)
            {
                if (!isRespawning)
                {
                    Spawn();
                }
            }
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInside = false;

            StopAllCoroutines();
            isRespawning = false;
        }
    }

    private void Start()
    {
        
    }

    private void Spawn()
    {
        if (SpawnPoints == null && SpawnPrefabs == null) return;

        if (activeEnemies.Count > 0) return;

        for(int i = 0; i < SpawnPoints.Length; i++)
        {
            GameObject enemyPrefab = SpawnPrefabs[i % SpawnPrefabs.Length];
            Transform spawnPoint = SpawnPoints[i];
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation, SpawnParent);

            activeEnemies.Add(newEnemy);

            Enemy enemy = newEnemy.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.OnDied += OnEnemyKilled;
            }
        }
    }

    private void OnEnemyKilled(GameObject deadEnemy)
    {
        activeEnemies.Remove(deadEnemy);
        CheckForRespawn();
    }

    private void CheckForRespawn()
    {
        //활성화된 Enemy에서 null인 경우 activeEnemies에서 삭제
        activeEnemies.RemoveAll(item => item == null);

        if (activeEnemies.Count == 0 && playerInside)
        {
            // Debug 로그 확인
            Debug.Log("--- CheckForRespawn: 조건 충족! StartRespawnTimer 호출 ---");
            StartRespawnTimer();
        }
    }

    private void StartRespawnTimer()
    {
        if(isRespawning) return;

        Debug.Log("--- StartRespawnTimer: 코루틴 시작 시도 ---");

        isRespawning = true;
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        isRespawning = true;

        Debug.Log($"[SpawnManager] Respawn 타이머 시작: {RespawnDelay}초 후 재스폰 시도");

        yield return new WaitForSeconds(RespawnDelay);

        //  수정: 코루틴 종료 시점에도 다시 한번 리스트를 정리하여 정확한 카운트 확인
        activeEnemies.RemoveAll(item => item == null);

        // 조건 확인
        if (playerInside && activeEnemies.Count == 0 && SpawnOnCehck)
        {
            Debug.Log($"[SpawnManager] 재스폰 조건 충족. Spawn() 호출");
            Spawn();
        }
        else
        {
            // 실패 원인을 명확히 로그로 남깁니다.
            Debug.LogWarning($"[SpawnManager] 재스폰 실패. 최종 상태: PlayerInside={playerInside}, ActiveCount={activeEnemies.Count}");
        }

        isRespawning = false;
    }    
}
