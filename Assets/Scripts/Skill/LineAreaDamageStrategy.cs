using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAreaDamageStrategy : ISkillBehaviorStrategy
{
    public void Execute(PlayerController _Player, SkillData _SkillData, Transform _Target)
    {
        foreach(var Effect in _SkillData.Effects)
        {
            Vector3 Origin = _Player.transform.position + Vector3.up * 1.0f;
            Vector3 Area = Vector3.one * Effect.Radius;
            Vector3 Dir = _Player.transform.forward;

            // 디버그용 레이 (씬 뷰에서 확인 가능)
            Debug.DrawRay(Origin, Dir * Effect.Distance, Color.red, 5f);
            Debug.Log($"{_SkillData.name} Ray Test On");

            RaycastHit[] Hits;
            Hits = Physics.BoxCastAll(Origin, Area, Dir, Quaternion.identity, Effect.Distance);
            foreach(RaycastHit Hit in Hits)
            {
                if(Hit.collider.CompareTag("Enemy"))
                {
                    Enemy enemy = Hit.collider.GetComponent<Enemy>();

                    enemy.TakeDamage(Effect.Power);

                    if(_SkillData.EffectPrefab != null)
                    {
                        EffectManager.Instance.Spawn(_SkillData.EffectPrefab, enemy.transform.position);
                    }
                }
            }
        }
    }

    

    
}
