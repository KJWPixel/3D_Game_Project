using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSkill : ISkillBehavior
{
    public void Execute(PlayerController _Player, SkillData _SkillData, Transform _Target)
    {     
        Ray Ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit Hit;

        if (Physics.Raycast(Ray, out Hit, _SkillData.Range, LayerMask.NameToLayer("Enemy")))
        {
            Enemy Enemy = Hit.collider.GetComponent<Enemy>();
            _Target = _Target.transform;

            foreach (var Effect in _SkillData.Effects)
            {
                Enemy.TakeDamage(Effect.Power);
            }         
        }
    }
}
