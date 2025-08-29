using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSkill : ISkillBehavior
{
    public void Execute(PlayerController _Player, SkillData _SkillData)
    {
        foreach (var Effect in _SkillData.Effects)
        {
            Debug.Log("SlashSkill ����!");
            // �÷��̾� ��ġ (�ణ ������ �߻�ǰ� ������)
            Vector3 start = _Player.transform.position + Vector3.up * 1.0f;
            Vector3 dir = _Player.transform.forward;

            // ����׿� ���� (�� �信�� Ȯ�� ����)
            Debug.DrawRay(start, dir * Effect.Distance, Color.red, 5f);
            Debug.Log("DrawRay ����!");
            // ���� ���� ����
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
