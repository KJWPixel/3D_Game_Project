using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DistanceTest : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] Transform Enemy;

    [Header("�Ÿ� �׽�Ʈ")]
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

        //Distance ��ų ����

        //��ų ����

        float Radius = 5f;
        Vector3 SkillArea = Player.transform.position + Vector3.forward * 3f + Vector3.up;
        Debug.Log($"<color=red>��ų���� ��ġ:</color> {SkillArea}");
        Debug.Log($"<color=red>��ų���� sqrMagnitude:</color> {Radius * Radius}");
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
            Debug.Log($"<color=yellow>�� ��ü Enemy Find Enemy[]��:</color> {Enemys[i]}");
        }
      
        List<Enemy> EnemyList = new List<Enemy>();
        
        foreach(Enemy enemy in Enemys)
        {
            //�Ÿ�������ų ����, ��ų������ġ ����, ���� ��� Enemy�� �迭�� ������
            //��ų������ġ �������� Enemy.transform.positon�� sqrMagnitude�� ���
            //��ų�� ������ �� (enemyTrs <= Radius * Radius) = True��� 
            //EnemyList�� Add
            float TargetSqr = Vector3.SqrMagnitude(enemy.transform.position - SkillArea);
            if (TargetSqr <= Radius * Radius)
            {
                Debug.Log($"<color=blue>SqrMagnitube �Ÿ�����:{enemy.name}:�Ÿ�: {TargetSqr} </color>");
                
                EnemyList.Add(enemy);
                for (int i = 0; i < EnemyList.Count; i++)
                {
                    Debug.Log($"<color=white>��ų���� �ȿ� �� EnemyList: {EnemyList[i]}</color>");
                }
            }

            //EnemyList�� �ִ� Enemy�� Enemy.TakeDamage ����

           
        }

    }
}
