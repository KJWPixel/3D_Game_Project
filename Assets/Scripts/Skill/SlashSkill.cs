using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSkill : ISkillBehavior
{
    public void Execute(PlayerController _Player, SkillData _SkillData)
    {
        foreach (var Effect in _SkillData.Effects)
        {
            Debug.Log("SlashSkill 실행!");
            // 플레이어 위치 (약간 위에서 발사되게 오프셋)
            Vector3 start = _Player.transform.position + Vector3.up * 1.0f;
            Vector3 dir = _Player.transform.forward;

            // 디버그용 레이 (씬 뷰에서 확인 가능)
            Debug.DrawRay(start, dir * Effect.Distance, Color.red, 5f);
            Debug.Log("DrawRay 실행!");
            // 실제 공격 판정
            if (Physics.Raycast(start, dir, out RaycastHit hit, Effect.Distance, LayerMask.GetMask("Enemy")))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(Effect.Power);

                    if (_SkillData.EffectPrefab != null)
                    {
                        EffectManager.Instance.Spawn(_SkillData.EffectPrefab, enemy.transform.position);
                    }
                }
            }
        }
    }
}
