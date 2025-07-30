using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// AI ����(STATE)����, AI ����
/// ���� ����, �̵� + ���� ����
/// 
/// ���� ���¿� �ٶ� �� ���� �Ǵ�
/// �ʿ� �� Enemy.cs�� ��� ȣ�� (Move, Attack, Animation)
/// </summary>
public class AI_Enemy : MonoBehaviour
{
    [SerializeField] Enemy Enemy;
    [SerializeField] Character Character;

    [SerializeField] Transform[] TRPATH;

    protected AI AI = AI.AI_CREATE;
    public AI CurrentAI => AI;

    [Header("Enemy")]
    [SerializeField] public bool PlayerChese = false;
    [SerializeField] public bool PlayerAttack = false;
    [SerializeField] float SeachRange = 0f;

    SphereCollider SphereCollider;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerChese = true;
            AI = AI.AI_CHASE;
            Debug.Log("Player Chase");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerChese = false;
            AI = AI.AI_SEARCH;
            Debug.Log("Not Player Chase");
        }
    }

    public void State()
    {
        switch(AI)//enum AI ���� ���� State��ȭ
        {           
            case AI.AI_SEARCH:
                Enemy.Search();
                break;
            case AI.AI_CHASE:
                Enemy.Chase();
                break;
            case AI.AI_ATTACK:
                Enemy.Attack();
                break;
            case AI.AI_RESET:
                Enemy.Reset();  
                break;  
        }            
    }

    public AI GetAIState()
    {
        return AI;
    }

    void Awake()
    {
        SphereCollider = GetComponent<SphereCollider>();
        Enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        SphereCollider.radius = SeachRange;
    }

    void Update()
    {
        Debug.Log(AI);

        State();
        GetAIState();
        AttackDistance();
    }

    private void AttackDistance()
    {
        if(CurrentAI == AI.AI_CHASE)
        {
            //Enemy�� �Ÿ��� �÷��̾��� �Ÿ��� ���̰� 2���� �۰� ���ٸ� ����
            float TargetDir = Vector3.Distance(transform.position, Character.transform.position);

            if (TargetDir <= 1f)
            {
                AI = AI.AI_ATTACK;
            }
            else
            {
                AI = AI.AI_CHASE;
            }
        }      
    }

    
}
