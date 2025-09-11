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
            //Magnitude(����) ����(������ǥ 0)������ �Ÿ�, ���� ����� �Ÿ��� �˷��� ( a - b )�� ���, Distance�� ����
            float Manitude = Vector3.Magnitude(EnemyTrs - PlayerTrs);
            Debug.Log($" Magnitude Player: {Manitude}");
        }
        if (sqrMagnitude)
        {
            //���� �Ÿ��� ��� �Ÿ��� ��Ʈ�� �� ���� ��� ��Ʈ 5 = 25�� �����
            float sqrMagnitude = Vector3.SqrMagnitude(EnemyTrs - PlayerTrs);
            Debug.Log($" sqrMagnitude: {sqrMagnitude}");
        }
    }
    
    void Update()
    {
        DistanceCheck();//�Ÿ�üũ �Լ� �׽�Ʈ

        //��ų ����
        float Radius = 5f;
        float RadiusSqr = Radius * Radius;

        //��ų���� ���� 
        Vector3 SkillArea = Player.transform.position + Vector3.forward * 5f + Vector3.up;
        Debug.Log($"<color=red>��ų���� ��ġ:</color> {SkillArea}");
        Debug.Log($"<color=red>��ų���� sqrMagnitude:</color> {Radius * Radius}");

        if (!EffectOn)//��ų���� ���� ȿ��
        {
            if(Prefab != null)
            {
                GameObject go = Instantiate(Prefab, SkillArea, Quaternion.identity);
                Destroy(go, 2f);
            }      
            EffectOn = true;    
        }

        //��ų���� �� �� ��ü Enemy Ž��, �Ǵ� EnemyManger�� List�� ���� EnemyManager.Instance.GetEnemy();
        Enemy[] Enemys = FindObjectsOfType<Enemy>();
        List<Enemy> EnemyList = new List<Enemy>();

        Debug.Log("FindObjectsOfType���� �� ��ü Enemy�� ã��");

        for (int i = 0; i < Enemys.Length; i++)
        {
            Debug.Log($"<color=yellow>�� ��ü Enemy[] :</color> {Enemys[i]}");
        }
               
        foreach(Enemy enemy in Enemys)//�� ��ü Enemys[]�� �ϳ��� ���� �Ÿ�����
        {
            //�Ÿ�������ų ����, ��ų������ġ ����, ���� ��� Enemy�� �迭�� ������
            //��ų������ġ �������� Enemy.transform.positon�� sqrMagnitude�� ���
            //��ų�� ������ �� (enemyTrs <= Radius * Radius) = True��� 
            //EnemyList�� Add

            float TargetSqr = Vector3.SqrMagnitude(enemy.transform.position - SkillArea);//����ġ - ��ų�������� �Ÿ����� 
            if (TargetSqr <= RadiusSqr)
            {
                Debug.Log($"<color=blue> ��ų���� �ȿ� Enemy ����! SqrMagnitube �Ÿ�����:{enemy.name}:�Ÿ�: {TargetSqr} </color>");
                
                EnemyList.Add(enemy);//��ų���� �ȿ� �����Ѵٸ� List.Add�߰�
                for (int i = 0; i < EnemyList.Count; i++)
                {
                    Debug.Log($"<color=green>��ų���� �ȿ� �� EnemyList: {EnemyList[i]}</color>");
                }
            }
            //EnemyList�� �ִ� Enemy�� Enemy.TakeDamage ����
            //for���� �̿��Ͽ� TakeDamage�� ��ȸ
        }
    }
}
