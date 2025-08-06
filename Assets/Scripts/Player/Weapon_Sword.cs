using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sword : BaseWeapon
{
    [SerializeField] private float Damage = 0f;

    private HashSet<Enemy> HitEnemies = new HashSet<Enemy>();

    BoxCollider BoxCollider;



    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
        if(BoxCollider == null)
        {
            Debug.Log("Weapon_Sword: BoxCollider Not");
        }
        else
        {
            BoxCollider.enabled = false;
        }
    }

    private void OnEnable()
    {
        HitEnemies.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
        {
            return;//태그가 Enemy가 아니면 리턴
        }

        Enemy Enemy = other.GetComponent<Enemy>();
        if(Enemy == null)
        {
            return;
        }

        //동일한 Enemny Attack 중복방지
        if(HitEnemies.Contains(Enemy))
        {
            return;
        }
        HitEnemies.Add(Enemy);

        Enemy.TakeDamage(Damage);
        Debug.Log($"Weapon Hit {Enemy.name} for {Damage}");

    }

    public void SwordEnableCollider()
    {
        HitEnemies.Clear();
        if (BoxCollider != null) BoxCollider.enabled = true;
    }

    public void SwordDisableCollider()
    {
        if (BoxCollider != null) BoxCollider.enabled = false;
    }
}
