using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private bool IsOnCoolDown = false;

    PlayerStat PlayerStat;
    PlayerAnimationController PlayerAnimationController;

    /* ��ų ���� SkillManger
     * ��ų�� ����� �� ����Ǵ� ����
     * ��Ÿ�� üũ
     * ���� �Ҹ� üũ
     * Ÿ�� ����
     * �ִϸ��̼� ����
     * ����Ʈ ����
     * ������/ȿ�� ����
    */

    private void Awake()
    {
        PlayerStat = GetComponent<PlayerStat>();
        PlayerAnimationController = GetComponent<PlayerAnimationController>();
    }

    public void UseSkill(SkillData _Skill, Transform _Target = null)
    {
        if (!CanUse(_Skill)) return;
        StartCoroutine(CastSkill(_Skill, _Target));
    }

    private bool CanUse(SkillData _Skill)
    {
        //��Ÿ��, ����, ���� üũ
        if (IsOnCoolDown) return false;
        if (PlayerStat == null) return false;
        if(PlayerStat.CurrentMp < _Skill.Cost) return false;
        
        return true;
    }

    private IEnumerator CastSkill(SkillData _Skill, Transform _Target)
    {
        if(!PlayerStat.ConsumeMp(_Skill.Cost))
        {
            //MP���� �� false �ڷ�ƾ ����
            yield break;
        }

        IsOnCoolDown = true;

        yield return new WaitForSeconds(_Skill.CastTime);

        //ȿ���� ���� ó�� �κ�(������, ȸ��, ���� ����)

        if(_Skill.type == SkillType.Heal)//�Ű����� ��ųŸ���� Heal�̸� 
        {
            PlayerStat.Heal(_Skill.Power);
        }

        //��ų����Ʈ Prefab ���� 
        if(_Skill.EffectPrefab != null)
        {
            if(_Skill.type == SkillType.Heal)
            {
                Instantiate(_Skill.EffectPrefab, PlayerStat.transform.position, Quaternion.identity);
            }
            else if(_Target != null)
            {
                Instantiate(_Skill.EffectPrefab, _Target.transform.position, Quaternion.identity);
            }
        }

        yield return new WaitForSeconds(_Skill.Cooldown);
        IsOnCoolDown = false;
    }


    /* 
     * ��ų �ߵ� �帧
     * 1. �÷��̾� �Է�: Ű����, ���콺 > SkillManager ȣ��
     * 2. ��� ���� ���� Ȯ��: ��Ÿ��, MP, Ÿ�� ��ȿ
     * 3. ����(cast)����: �ִϸ��̼� ����, ����Ʈ �غ�
     * 4. ȿ������: Ÿ�ٿ��� ������, ȸ��, �����̻� �ο�
     * 5. ��Ÿ�� ����
     * 
     * 
     * ��ų Ÿ�Ժ� ó��
     * ���� Ÿ�� ��ų > �� ��� ������ ����
     * ������ų > ���� �� ����� ����
     * ä�θ���ų > ���� �ð� ���� ���� �ߵ�
     * ����/���ۺ� > �ɷ�ġ ����, ���� �ð� �� ����
     * Ÿ�Ժ� ������ SkillType enum�� switch�� �Ǵ� ���/�������̽��� Ȱ���� �и��մϴ�.
     * 
     * ��ų �����͸� ScriptableObject�� ���� > �������� �뷱�� ���� ����
     * ��Ÿ��, �ڿ��Ҹ�� Ŭ���̾�Ʈ + �������� ����
     * �ִϸ��̼� �̺�Ʈ�� Ÿ�̹� ���� ������ ����
     * ���� ��ų�� Physics.OverlapSphere���� �Լ��� ���
     * ȿ��(������,�� ����)�� StatSystem�̳� SatusEffectManager�� ���� ����
     * 
     * 
     * ����
     * �÷��̾ "FireBall" ����
     * SkillManager.UseSkill(FireBall, Target)
     * CanUse üũ (��Ÿ��,MP)
     * �ִϸ��̼� ��� & ĳ���� �ð� ���
     * FireBall Prefab����
     * �浹 �� ������ ��� & Ÿ�� ü�� ���� 
     * ��Ÿ�� ����
    */
}
