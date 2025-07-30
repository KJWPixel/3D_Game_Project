using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// AI 상태(STATE)관리, AI 로직
/// 상태 변경, 이동 + 공격 결정
/// 
/// 현재 상태에 다라 뭘 할지 판단
/// 필요 시 Enemy.cs의 기능 호출 (Move, Attack, Animation)
/// </summary>
public class AI_Enemy : MonoBehaviour
{
    [SerializeField] Enemy Enemy;
    [SerializeField] Character Character;
    [SerializeField] Transform[] TRPATH;

    [Header("Enemy")]
    [SerializeField] public bool PlayerChese = false;
    [SerializeField] float SeachRange = 0f;

    SphereCollider SphereCollider;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerChese = true;
            Debug.Log("Player Chase");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerChese = false;
            Debug.Log("Not Player Chase");
        }
    }
    void Awake()
    {
        SphereCollider = GetComponent<SphereCollider>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        SphereCollider.radius = SeachRange;
        
    }






}
