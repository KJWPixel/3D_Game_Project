using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
