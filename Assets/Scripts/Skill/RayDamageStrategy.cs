using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayDamageSkillStrategy : ISkillBehaviorStrategy
{
    public void Execute(PlayerController _Player, SkillData _SkillData, Transform _Target)
    {
        foreach (var Effect in _SkillData.Effects)
        {
            // �÷��̾� ��ġ (�ణ ������ �߻�ǰ� ������)
            Vector3 Origin = _Player.transform.position + Vector3.up * 1.0f;
            Vector3 Dir = _Player.transform.forward;

            // ����׿� ���� (�� �信�� Ȯ�� ����)
            Debug.DrawRay(Origin, Dir * Effect.Distance, Color.red, 5f);
            Debug.Log($"{_SkillData.name} Ray Test On");

            //�ټ� ������� ���� 
            //RaycastHit[] ArrayHit;
            //ArrayHit = Physics.RaycastAll(Origin, Dir, Effect.Distance);
            //foreach(RaycastHit hit in ArrayHit)
            //{
            //    if(hit.collider.CompareTag("Enemy"))
            //    {
            //        Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();

            //        if (enemy != null)
            //        {
            //            enemy.TakeDamage(Effect.Power);

            //            if (_SkillData.EffectPrefab != null)
            //            {
            //                EffectManager.Instance.Spawn(_SkillData.EffectPrefab, enemy.transform.position);
            //            }
            //        }
            //    }
            //}

            //���� ��������
            if (Physics.Raycast(Origin, Dir, out RaycastHit Hit, Effect.Distance))
            {
                if(Hit.collider.CompareTag("Enemy"))
                {
                    Enemy enemy = Hit.collider.gameObject.GetComponent<Enemy>();

                    if (enemy != null)
                    {
                        enemy.TakeDamage(Effect.Power);

                        if (_SkillData.HitEffectPrefab != null)
                        {
                            EffectManager.Instance.Spawn(_SkillData.HitEffectPrefab, enemy.transform.position);
                        }
                    }
                }
            }
        }
    }
}
