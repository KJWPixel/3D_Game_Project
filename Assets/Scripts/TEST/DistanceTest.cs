using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DistanceTest : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] Transform Enemy;

    [Header("거리 테스트")]
    [SerializeField] bool VectorDistance = false;
    [SerializeField] bool Magnitude = false;
    [SerializeField] bool sqrMagnitude = false;

    [Header("Effect")]
    [SerializeField] bool EffectOn = false;
    [SerializeField] GameObject Prefab;

    Vector3 PlayerTrs;
    Vector3 EnemyTrs;

    void Start()
    {   PlayerTrs = Player.transform.position;
        EnemyTrs = Enemy.transform.position;
        Debug.Log($"Player Start Transforn: {PlayerTrs}");
        Debug.Log($"Enemy Start Transforn: {EnemyTrs}");
    }

    private void DistanceCheck()
    {
        PlayerTrs = Player.transform.position;
        EnemyTrs = Enemy.transform.position;


        if (VectorDistance)
        {
            float Distance = Vector3.Distance(PlayerTrs, EnemyTrs);
            Debug.Log($" VectorDistance: {Distance}");
        }
        if (Magnitude)
        {
            float Manitude = Vector3.Magnitude(PlayerTrs);
            Debug.Log($" Magnitude Player: {Manitude}");
        }
        if (sqrMagnitude)
        {
            float sqrMagnitude = Vector3.SqrMagnitude(EnemyTrs - PlayerTrs);
            Debug.Log($" sqrMagnitude: {sqrMagnitude}");
        }
    }
    
    void Update()
    {
        DistanceCheck();

        //Distance 스킬 예시

        //스킬 범위

        float Radius = 5f;
        Vector3 SkillArea = Player.transform.position + Vector3.forward * 3f + Vector3.up;
        Debug.Log($"<color=red>스킬시작 위치:</color> {SkillArea}");
        Debug.Log($"<color=red>스킬범위 sqrMagnitude:</color> {Radius * Radius}");
        if (!EffectOn)
        {
            if(Prefab != null)
            {
                GameObject go = Instantiate(Prefab, SkillArea, Quaternion.identity);
                Destroy(go, 2f);
            }      
            EffectOn = true;    
        }

        Enemy[] Enemys = FindObjectsOfType<Enemy>();

        for (int i = 0; i < Enemys.Length; i++)
        {
            Debug.Log($"<color=yellow>씬 전체 Enemy Find Enemy[]안:</color> {Enemys[i]}");
        }
      
        List<Enemy> EnemyList = new List<Enemy>();
        
        foreach(Enemy enemy in Enemys)
        {
            //거리측정스킬 사용시, 스킬시작위치 지정, 씬의 모든 Enemy를 배열로 가져옴
            //스킬시작위치 기준으로 Enemy.transform.positon을 sqrMagnitude로 계산
            //스킬의 범위를 비교 (enemyTrs <= Radius * Radius) = True라면 
            //EnemyList에 Add
            float TargetSqr = Vector3.SqrMagnitude(enemy.transform.position - SkillArea);
            if (TargetSqr <= Radius * Radius)
            {
                Debug.Log($"<color=blue>SqrMagnitube 거리측정:{enemy.name}:거리: {TargetSqr} </color>");
                
                EnemyList.Add(enemy);
                for (int i = 0; i < EnemyList.Count; i++)
                {
                    Debug.Log($"<color=white>스킬범위 안에 들어간 EnemyList: {EnemyList[i]}</color>");
                }
            }

            //EnemyList에 있는 Enemy는 Enemy.TakeDamage 전달

           
        }

    }
}
