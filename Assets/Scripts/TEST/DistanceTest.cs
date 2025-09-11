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
            //Magnitude(인자) 원점(월드좌표 0)에서의 거리, 대상과 대상의 거리를 알려면 ( a - b )로 계산, Distance와 동일
            float Manitude = Vector3.Magnitude(EnemyTrs - PlayerTrs);
            Debug.Log($" Magnitude Player: {Manitude}");
        }
        if (sqrMagnitude)
        {
            //대상과 거리를 계산 거리에 루트를 한 값을 출력 루트 5 = 25로 출력함
            float sqrMagnitude = Vector3.SqrMagnitude(EnemyTrs - PlayerTrs);
            Debug.Log($" sqrMagnitude: {sqrMagnitude}");
        }
    }
    
    void Update()
    {
        DistanceCheck();//거리체크 함수 테스트

        //스킬 범위
        float Radius = 5f;
        float RadiusSqr = Radius * Radius;

        //스킬시작 지점 
        Vector3 SkillArea = Player.transform.position + Vector3.forward * 5f + Vector3.up;
        Debug.Log($"<color=red>스킬시작 위치:</color> {SkillArea}");
        Debug.Log($"<color=red>스킬범위 sqrMagnitude:</color> {Radius * Radius}");

        if (!EffectOn)//스킬시작 지점 효과
        {
            if(Prefab != null)
            {
                GameObject go = Instantiate(Prefab, SkillArea, Quaternion.identity);
                Destroy(go, 2f);
            }      
            EffectOn = true;    
        }

        //스킬시전 시 씬 전체 Enemy 탐색, 또는 EnemyManger로 List로 관리 EnemyManager.Instance.GetEnemy();
        Enemy[] Enemys = FindObjectsOfType<Enemy>();
        List<Enemy> EnemyList = new List<Enemy>();

        Debug.Log("FindObjectsOfType으로 씬 전체 Enemy를 찾기");

        for (int i = 0; i < Enemys.Length; i++)
        {
            Debug.Log($"<color=yellow>씬 전체 Enemy[] :</color> {Enemys[i]}");
        }
               
        foreach(Enemy enemy in Enemys)//씬 전체 Enemys[]를 하나씩 꺼내 거리측정
        {
            //거리측정스킬 사용시, 스킬시작위치 지정, 씬의 모든 Enemy를 배열로 가져옴
            //스킬시작위치 기준으로 Enemy.transform.positon을 sqrMagnitude로 계산
            //스킬의 범위를 비교 (enemyTrs <= Radius * Radius) = True라면 
            //EnemyList에 Add

            float TargetSqr = Vector3.SqrMagnitude(enemy.transform.position - SkillArea);//적위치 - 스킬시작지점 거리측정 
            if (TargetSqr <= RadiusSqr)
            {
                Debug.Log($"<color=blue> 스킬범위 안에 Enemy 존재! SqrMagnitube 거리측정:{enemy.name}:거리: {TargetSqr} </color>");
                
                EnemyList.Add(enemy);//스킬범위 안에 존재한다면 List.Add추가
                for (int i = 0; i < EnemyList.Count; i++)
                {
                    Debug.Log($"<color=green>스킬범위 안에 들어간 EnemyList: {EnemyList[i]}</color>");
                }
            }
            //EnemyList에 있는 Enemy는 Enemy.TakeDamage 전달
            //for문을 이용하여 TakeDamage를 순회
        }
    }
}
