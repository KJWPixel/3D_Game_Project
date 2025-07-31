using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    [SerializeField] float MaxHp;
    [SerializeField] float CurrentHp;
    [SerializeField] float MaxMp;
    [SerializeField] float CurrentMp;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float _Damage)
    {
        CurrentHp -= _Damage;
    }
}
